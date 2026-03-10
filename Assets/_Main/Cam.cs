using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Cam : MonoBehaviour, IEntity
{
    [SerializeReference] public CameraComponent[] components;
    [HideInInspector] public bool isActive;
    
    public MonoBehaviour Instance => this;

    public CamConfig Config {get; private set;}
    public CameraPreferences Prefs { get; private set; }
    public Camera GameCamera { get; private set; }
    public Transform Target { get; private set; }
    public CinemachineCamera CmCamera { get; private set; }
    public CinemachineOrbitalFollow CmOrbitFollow { get; private set; }
    public CinemachineInputAxisController CmInputAxisController { get; private set; }

    public void Init(IEntityConfig config)
    { if (config is not CamConfig camConf) return; Config = camConf; Prefs = Core.Data.Get.preferences.camera; }
    
    public void OnCreate()
    {
        isActive = true;
        
        var cameraObj = new GameObject("Camera");
        cameraObj.transform.SetParent(transform, false);
        
        var cameraScr = cameraObj.AddComponent<Camera>();
        cameraScr.GetUniversalAdditionalCameraData().renderPostProcessing = true;
        cameraScr.clearFlags = CameraClearFlags.SolidColor;
        cameraScr.backgroundColor = Prefs.backgroundColor;
        cameraScr.orthographic = false;
        cameraScr.tag = "MainCamera";
        GameCamera = cameraScr;
        
        var targetObj = new GameObject("Target");
        targetObj.transform.SetParent(transform, false);
        Target = targetObj.transform;
        
        var cmCameraObj = new GameObject("CMCamera");
        cmCameraObj.transform.SetParent(transform, false);
        
        var cmCamera = cmCameraObj.AddComponent<CinemachineCamera>();
        cameraObj.AddComponent<CinemachineBrain>();
        cmCamera.Follow = Target;
        cmCamera.Lens.FarClipPlane = Prefs.farClip;
        cmCamera.Lens.FieldOfView = 60;
        CmCamera = cmCamera;

        var cmOrbitFollow = cmCameraObj.AddComponent<CinemachineOrbitalFollow>();
        cmOrbitFollow.TrackerSettings.BindingMode = BindingMode.WorldSpace;
        cmOrbitFollow.TrackerSettings.PositionDamping = Vector3.zero;
        cmOrbitFollow.TrackerSettings.RotationDamping = Vector3.zero;
        var maxAngle = Prefs.maxVerticalAngle;
        var minAngle = Prefs.minVerticalAngle;
        cmOrbitFollow.VerticalAxis.Range = new Vector2(minAngle, maxAngle);
        cmOrbitFollow.Radius = (Prefs.minZoom + Prefs.maxZoom) / 2;
        CmOrbitFollow = cmOrbitFollow;
        
        var cmComposer = cmCameraObj.AddComponent<CinemachineRotationComposer>();
        cmComposer.Damping = Vector2.zero;
        
        var cmInputAxisController = cmCameraObj.AddComponent<CinemachineInputAxisController>();
        cmInputAxisController.Controllers[0].Input.Gain = Prefs.horizontalSensitivity;
        cmInputAxisController.Controllers[1].Input.Gain = Prefs.verticalSensitivity * -1;
        cmInputAxisController.enabled = false;
        CmInputAxisController = cmInputAxisController;
        
        components = new CameraComponent[Config.components.Length];
        for (var i = 0; i < components.Length; i++)
        {
            var tName = Config.components[i];
            var type = Type.GetType(tName);
            if (type == null) throw new Exception("Type not found: " + tName);
            var script = (CameraComponent)Activator.CreateInstance(type);
            script.Init(this);
            components[i] = script;
        }
    }

    public void OnDelete()
    { isActive = false; foreach (var component in components) component.DeInit(); }
}

[Serializable]
public class CamZoom : CameraComponent
{
    public override void Init(Cam cam)
    { base.Init(cam); Core.Input.Get.Camera.Zoom.Enable(); Core.Coroutine.Start(Zoom); }

    private IEnumerator Zoom()
    {
        var orbit = Owner.CmOrbitFollow;
        var targetZoom = orbit.Radius;
        var velocity = 0f;
        while (Owner.isActive)
        {
            var zoomDelta = Core.Input.Get.Camera.Zoom.ReadValue<float>();
            if (zoomDelta != 0f)
            {
                targetZoom += zoomDelta * Owner.Prefs.zoomStep;
                var minZoom = Owner.Prefs.minZoom;
                var maxZoom = Owner.Prefs.maxZoom;
                targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
            }
            var zoomSpeed = Owner.Prefs.zoomSpeed;
            orbit.Radius = Mathf.SmoothDamp(orbit.Radius, targetZoom, ref velocity, zoomSpeed);
            yield return null;
        }
    }

    public override void DeInit()
    { base.DeInit(); Core.Coroutine.Stop(Zoom); Core.Input.Get.Camera.Zoom.Disable(); }
}

[Serializable]
public class CamMove : CameraComponent
{
    public override void Init(Cam cam)
    {
        base.Init(cam);
        Core.Input.Get.Camera.Move.Enable();
        Core.Input.Get.Camera.Drag.Enable();
        Core.Coroutine.Start(Move);
    }
    
    private IEnumerator Move()
    {
        var mult = 1f;
        var lastCursorPos = Mouse.current.position.ReadValue();
        while (Owner.isActive)
        {
            Vector2 inputVector;
            if (Core.Input.Get.Camera.Drag.WasPerformedThisFrame())
            {
                lastCursorPos = Mouse.current.position.ReadValue();
                Core.Input.Get.Camera.Move.Disable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            if (Core.Input.Get.Camera.Move.IsPressed())
            {
                Core.Input.Get.Camera.Drag.Disable();
                inputVector = Core.Input.Get.Camera.Move.ReadValue<Vector2>();
                mult = Owner.Prefs.moveMult;
            }
            else if (Core.Input.Get.Camera.Drag.IsPressed())
            {
                var mouseDelta = Mouse.current.delta.ReadValue().normalized;
                inputVector = new Vector3(-mouseDelta.x, -mouseDelta.y, 0);
                mult = Owner.Prefs.dragMult;
            }
            else
            {
                Core.Input.Get.Camera.Drag.Enable();
                Core.Input.Get.Camera.Move.Enable();
                inputVector = Vector2.zero;
            }

            if (Core.Input.Get.Camera.Drag.WasReleasedThisFrame())
            {
                Cursor.lockState = CursorLockMode.None;
                Mouse.current.WarpCursorPosition(lastCursorPos);
                Cursor.visible = true;
            }
            
            var h = inputVector.x;
            var v = inputVector.y;
            var forward = Owner.Target.forward;
            var right = Owner.Target.right;
            forward.y = 0;
            right.y = 0;
            var direction = forward * v + right * h;
            var currentPos = Owner.Target.localPosition;
            var newPos = currentPos + direction * GetSpeed(mult);
            Owner.Target.localPosition = Vector3.Lerp(currentPos, newPos, Time.deltaTime);
            yield return null;
        }
    }

    private float GetSpeed(float mult)
    {
        var baseSpeed = Owner.Prefs.baseSpeed;
        var zoomValue = Owner.CmOrbitFollow.Radius;
        var multiplier = zoomValue / baseSpeed;
        var speed = baseSpeed * multiplier;
        var minSpeed = Owner.Prefs.minSpeed;
        var maxSpeed = Owner.Prefs.maxSpeed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed) * mult;
        return speed;
    }

    public override void DeInit()
    {
        Core.Coroutine.Stop(Move);
        Core.Input.Get.Camera.Move.Disable();
        Core.Input.Get.Camera.Drag.Disable();
        base.DeInit();
    }
}

[Serializable]
public class CamRotate : CameraComponent
{
    public override void Init(Cam cam)
    {
        base.Init(cam);
        Core.Input.Get.Camera.Rotate.Enable();
        Core.Coroutine.Start(Rotate);
    }

    private IEnumerator Rotate()
    {
        while (Owner.isActive)
        {
            Owner.CmInputAxisController.enabled = Core.Input.Get.Camera.Rotate.IsPressed();
            var yAngle = Owner.CmCamera.transform.localEulerAngles.y;
            Owner.Target.localRotation = Quaternion.Euler(0f, yAngle, 0f);
            yield return null;
        }
    }

    public override void DeInit()
    {
        Core.Coroutine.Stop(Rotate);
        Core.Input.Get.Camera.Rotate.Disable();
        base.DeInit();
    }
}

[Serializable] public abstract class CameraComponent
{ public string componentName; protected Cam Owner; 
public virtual void Init(Cam cam) { Owner = cam; componentName = GetType().Name; } public virtual void DeInit() {} }
