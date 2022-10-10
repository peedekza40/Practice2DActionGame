using System;
using Core.Constants;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItem : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public Transform InventoryContainer;
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
        DragItemCanvasGroup = DragItemTransform.GetComponent<CanvasGroup>();
        DragItemTransform.GetComponent<LayoutElement>().ignoreLayout = true;
        DragItemTransform.Find(GameObjectName.SlotFrame).gameObject.SetActive(false);

        //disable masking
        Transform item = DragItemTransform.Find(GameObjectName.Item);
        Transform itemImage = item.Find(GameObjectName.ItemImage);
        Transform itemAmount = item.Find(GameObjectName.ItemAmount);
        DisableMasking(itemImage);
        DisableMasking(itemAmount);

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

    private void DisableMasking(Transform targetTransform)
    {
        MaskableGraphic maskableGraphic = targetTransform.GetComponent<MaskableGraphic>();
        maskableGraphic.maskable = false;
        maskableGraphic.RecalculateClipping();
    }
}
