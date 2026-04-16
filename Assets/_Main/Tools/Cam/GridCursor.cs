using UnityEngine;
using UnityEngine.InputSystem;
using Grid = Utils.Grid;

namespace Tools.Cam
{
    public class GridCursor : MonoBehaviour
    {
        private Camera _camera;
        private Grid _grid;
        private bool _isActive;
        
        public void Initialize(Camera cam)
        {
            _camera = cam;
            _grid = new Grid(Grid.GridSize.Grid1X1);
        }
        
        public void Enable()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }
        
        private void Update()
        {
            if (!_isActive) return;
            var plane = new Plane(Vector3.up, Vector3.zero);
            var mousePos = Mouse.current.position.ReadValue();
            var ray = _camera.ScreenPointToRay(mousePos);
            var hit = plane.Raycast(ray, out var distance);
            if (!hit) return;
            var point = ray.GetPoint(distance);
            var cellIndex = _grid.GetCellIndex(point);
            var cellCenter = _grid.GetCellCenter(cellIndex);
            transform.position = cellCenter;
        }
        
        public void Disable()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }
    }
}