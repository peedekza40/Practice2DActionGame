using System;
using Core.Constants;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character.Inventory
{
    public class DragDropItem : MonoBehaviour,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
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
            DragItemTransform = Instantiate(SlotTransform, Canvas.transform);
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
            DragItemTransform.anchorMax = new Vector2(0, 1);
            DragItemTransform.anchorMin = new Vector2(0, 1);
            DragItemTransform.sizeDelta = SlotTransform.sizeDelta;
            
            ItemCanvasGroup.alpha = 0.6f;
            DragItemCanvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)Canvas.transform,
                eventData.position,
                Canvas.worldCamera,
                out position);
            DragItemTransform.position = Canvas.transform.TransformPoint(position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ItemCanvasGroup.alpha = 1f;
            DragItemCanvasGroup.blocksRaycasts = true;

            Destroy(DragItemTransform.gameObject);
        }

        private void DisableMasking(Transform targetTransform)
        {
            if(targetTransform != null)
            {
                MaskableGraphic maskableGraphic = targetTransform.GetComponent<MaskableGraphic>();
                maskableGraphic.maskable = false;
                maskableGraphic.RecalculateClipping();
            }

        }
    }

}
