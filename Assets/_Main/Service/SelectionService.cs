using System;
using Cmd;
using Cmd.Entity;
using Module.Entity;
using UnityEngine;
using UnityEngine.InputSystem;
using View.Entity;

namespace Service
{
    public class SelectionService : IDisposable
    {
        private readonly Camera _cam;
        private readonly Inputs _inputs;
        private readonly CommandProcessor _cmd;
        
        public SelectionService(CommandProcessor cmd, Camera cam, Inputs inputs)
        {
            _cmd = cmd;
            _cam = cam;
            _inputs = inputs;
        }

        public void ActivateSelections()
        {
            _inputs.Entity.Enable();
            _inputs.Entity.Select.Enable();
            _inputs.Entity.Select.performed += Select;
        }
        
        private void Select(InputAction.CallbackContext _)
        {
            if (Cursor.lockState == CursorLockMode.Locked) return;
            var mousePos = Mouse.current.position.ReadValue();
            var ray = _cam.ScreenPointToRay(mousePos);
            if (!Physics.Raycast(ray, out var hit)) return;
            var collider = hit.collider;
            if (collider == null) return;
            var module = collider.GetComponent<SelectionModule>();
            if (module == null) return;
            var status = module.ModuleStatus;
            if (!status) return;
            var target = module.Target;
            if (target == null) return;
                
            if (!target.IsSelected.CurrentValue) MarkSelected(target);
        }

        private bool MarkSelected(EntityVm entity)
        {
            var id = entity.ID;
            var command = new CmdCommandSelectEntity(id);
            var result = _cmd.Process(command);
            return result;
        }

        public bool MarkUnselected(string id)
        {
            var command = new CmdCommandDeselectEntity(id);
            var result = _cmd.Process(command);
            return result;
        }

        public void Dispose()
        {
            _inputs.Entity.Disable();
            _inputs.Entity.Select.Disable();
            _inputs.Entity.Select.performed -= Select;
        }
    }
}