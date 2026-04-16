using Constant;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Module.UIElement
{
    public class DragModule : MonoBehaviour, IModule, IBeginDragHandler, IDragHandler
    {
        private RectTransform _ownerRect;
        private Canvas _canvas;

        private float _screenWidth;
        private float _screenHeight;
        private float _ownerWidth;
        private float _ownerHeight;

        public Enums.Modules ModuleKey => Enums.Modules.DragModule;
        public bool ModuleStatus { get; set; }

        public void Initialize(RectTransform ownerRect)
        {
            _ownerRect = ownerRect;
            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ModuleStatus) return;
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
            _ownerWidth = _ownerRect.rect.width;
            _ownerHeight = _ownerRect.rect.height;
            _ownerRect.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!ModuleStatus) return;
            var dragValue = eventData.delta / _canvas.scaleFactor;
            
            if (_ownerRect.anchoredPosition.x < 0)
            { if (dragValue.x < 0) dragValue.x = 0; }
            
            if (_ownerRect.anchoredPosition.y < 0)
            { if (dragValue.y < 0) dragValue.y = 0; }

            if (_ownerRect.anchoredPosition.x > _screenWidth - _ownerWidth)
            { if (dragValue.x > 0) dragValue.x = 0; }
            
            if (_ownerRect.anchoredPosition.y > _screenHeight - _ownerHeight)
            { if (dragValue.y > 0) dragValue.y = 0; }
            
            _ownerRect.anchoredPosition += dragValue;
        }
    }
}