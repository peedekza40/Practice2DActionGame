using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

public class MouseEvent : MonoBehaviour, 
    IPointerEnterHandler, 
    IPointerExitHandler,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public bool IsHover { get; private set; }

    public UnityEvent OnLeftClick;
    public UnityEvent OnLeftDown;
    public UnityEvent OnLeftUp;

    public UnityEvent OnRightClick;
    public UnityEvent OnRightDown;
    public UnityEvent OnRightUp;

    public UnityEvent OnMiddleClick;
    public UnityEvent OnMiddleDown;
    public UnityEvent OnMiddleUp;


    public void OnPointerEnter(PointerEventData eventData)
    {
        IsHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsHover = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InvokeByInputButton(OnLeftClick, OnRightClick, OnMiddleClick, eventData.button);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        InvokeByInputButton(OnLeftDown, OnRightDown, OnMiddleDown, eventData.button);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InvokeByInputButton(OnLeftUp, OnRightUp, OnMiddleUp, eventData.button);
    }

    private void InvokeByInputButton(UnityEvent onLeft, UnityEvent onRight, UnityEvent onMiddle, InputButton button)
    {
        switch(button)
        {
            case InputButton.Left : 
                onLeft?.Invoke();
                break;
            case InputButton.Right :
                onRight?.Invoke();
                break;
            case InputButton.Middle :
                onMiddle?.Invoke();
                break;
        }
    }
}
