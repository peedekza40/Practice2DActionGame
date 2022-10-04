using System;
using Constants;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public Transform InstantiateContainer;
    public CanvasGroup ItemCanvasGroup;

    private Canvas Canvas;
    private RectTransform SlotTransform;
    private RectTransform DragItemTransform;
    private CanvasGroup DragItemCanvasGroup;

    private void Awake() 
    {
        Canvas = GetComponentInParent<Canvas>();
        SlotTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag Item");
        
        DragItemTransform = Instantiate(SlotTransform, InstantiateContainer);
        DragItemTransform.Find(GameObjectName.SlotFrame).gameObject.SetActive(false);            
        DragItemCanvasGroup = DragItemTransform.GetComponent<CanvasGroup>();
        DragItemTransform.GetComponent<LayoutElement>().ignoreLayout = true;

        //set rect transform
        DragItemTransform.anchoredPosition = SlotTransform.anchoredPosition;
        DragItemTransform.anchorMax = new Vector2(0, 1);
        DragItemTransform.anchorMin = new Vector2(0, 1);
        DragItemTransform.sizeDelta = SlotTransform.sizeDelta;
        
        ItemCanvasGroup.alpha = 0.6f;
        DragItemCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragItemTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag Item");

        ItemCanvasGroup.alpha = 1f;
        DragItemCanvasGroup.blocksRaycasts = true;

        Destroy(DragItemTransform.gameObject);
    }
}
