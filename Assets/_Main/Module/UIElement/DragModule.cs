using UnityEngine;
using UnityEngine.EventSystems;

namespace Module.UIElement
{
    public class DragModule : MonoBehaviour, IModule, IBeginDragHandler, IDragHandler
    {
        public RectTransform OwnerRect { get; set; }
        
        private Canvas _canvas;
        private bool _isDragging;
        
        private float _screenWidth;
        private float _screenHeight;
        private float _ownerWidth;
        private float _ownerHeight;
        
        public void Activate()
        {
            _canvas = GetComponentInParent<Canvas>();
            _isDragging = true;
        }

        public void Deactivate()
        {
            _isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
            _ownerWidth = OwnerRect.rect.width;
            _ownerHeight = OwnerRect.rect.height;
            OwnerRect.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDragging) return;
            var dragValue = eventData.delta / _canvas.scaleFactor;
            
            if (OwnerRect.anchoredPosition.x < 0)
            { if (dragValue.x < 0) dragValue.x = 0; }
            
            if (OwnerRect.anchoredPosition.y < 0)
            { if (dragValue.y < 0) dragValue.y = 0; }

            if (OwnerRect.anchoredPosition.x > _screenWidth - _ownerWidth)
            { if (dragValue.x > 0) dragValue.x = 0; }
            
            if (OwnerRect.anchoredPosition.y > _screenHeight - _ownerHeight)
            { if (dragValue.y > 0) dragValue.y = 0; }
            
            OwnerRect.anchoredPosition += dragValue;
        }
    }
}