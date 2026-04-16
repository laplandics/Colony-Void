using System.Collections;
using Inspector;
using Unity.Cinemachine;
using UnityEngine;

namespace Tools.Cam
{
    public class CameraController : MonoBehaviour
    {
        [ReadOnly] public float moveSpeed;
        [ReadOnly] public float rotateSpeed;
        [ReadOnly] public float zoomSpeed;
        [ReadOnly] public Vector2Int zoomConstrains;
        
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinemachineOrbitalFollow orbitalFollow;
        [SerializeField] private CinemachineInputAxisController inputAxis;
        [Space]
        [SerializeField] private GridCursor gridCursor;
        
        private Coroutine _moveCoroutine;
        private Inputs _inputs;
        
        public GridCursor GridCursor => gridCursor;

        public void Init(Inputs inputs, Camera cam)
        {
            _inputs = inputs;
            _inputs.Camera.Move.Enable();
            _inputs.Camera.Rotate.Enable();
            _inputs.Camera.Zoom.Enable();
            _moveCoroutine = StartCoroutine(Move());
            
            gridCursor.Initialize(cam);
        }
        
        private IEnumerator Move()
        {
            gridCursor.Enable();
            while (true)
            {
                transform.position += GetDirection() * (moveSpeed * Time.deltaTime);
                ApplyRotation();
                ApplyZoom();
                yield return null;
            }
        }

        private Vector3 GetDirection()
        {
            var dir = _inputs.Camera.Move.ReadValue<Vector2>().normalized;
            var v = dir.y;
            var h = dir.x;
            var forward = cameraTransform.forward;
            var right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            var moveDirection = forward * v + right * h;
            return moveDirection;
        }

        private float _rotationBuffer;
        private bool _disableRotationInputAfterButtonReleased;
        private bool _isRotating;
        
        private void ApplyRotation()
        {
            if (_inputs.Camera.Rotate.WasPressedThisFrame())
            {
                if (_isRotating)
                {
                    inputAxis.enabled = false;
                    _isRotating = false;
                    
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    
                    _rotationBuffer = 0.18f;
                    _disableRotationInputAfterButtonReleased = false;
                    inputAxis.enabled = true;
                    _isRotating = true;
                    gridCursor.Disable();
                }
            }
            
            if (_rotationBuffer > 0f && _inputs.Camera.Rotate.IsPressed())
            {
                _rotationBuffer -= Time.deltaTime;
            }
            else if (_rotationBuffer <= 0f && _inputs.Camera.Rotate.IsPressed())
            {
                _disableRotationInputAfterButtonReleased = true;
            }
            else if (_disableRotationInputAfterButtonReleased && !_inputs.Camera.Rotate.IsPressed())
            {
                inputAxis.enabled = false;
                _isRotating = false;
            }

            if (!_isRotating && _inputs.Camera.Rotate.WasReleasedThisFrame())
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gridCursor.Enable();
            }
            
            inputAxis.Controllers[0].Input.Gain = rotateSpeed;
            inputAxis.Controllers[1].Input.Gain = -rotateSpeed;
        }

        private void ApplyZoom()
        {
            var zoomDelta = _inputs.Camera.Zoom.ReadValue<float>();
            var zoomValue = zoomDelta * (zoomSpeed * Time.deltaTime);
            orbitalFollow.Radius = Mathf.Clamp(orbitalFollow.Radius += zoomValue, zoomConstrains.x, zoomConstrains.y);
        }
        
        private void OnDestroy()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
            _inputs.Camera.Move.Disable();
            _inputs.Camera.Rotate.Disable();
            _inputs.Camera.Zoom.Disable();
            
            _inputs = null;
        }
    }
}