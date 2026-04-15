using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class PointerEvent : UnityEvent<PointerEventData.InputButton> { }

public class AdvancedButton : Button
{
    [Header("Mouse Clicks")]
    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onMiddleClick;

    public PointerEvent onAnyClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                onLeftClick?.Invoke();
                break;

            case PointerEventData.InputButton.Right:
                onRightClick?.Invoke();
                break;

            case PointerEventData.InputButton.Middle:
                onMiddleClick?.Invoke();
                break;
        }

        onAnyClick?.Invoke(eventData.button);
    }
}