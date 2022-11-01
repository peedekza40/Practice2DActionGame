using System;
using Core.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character.Inventory
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        [Header("UI")]
        public RectTransform SlotTransform;
        public Transform ItemTransform;
        public Image ItemImage;
        public TextMeshProUGUI ItemAmountText;

        public Guid ItemInstanceId { get; private set; }
        private InventoryManagement Inventory;
        public MouseEvent ItemMouseEvent { get; private set; }

        private void Awake() 
        {
            ItemMouseEvent = SlotTransform.GetComponent<MouseEvent>();
            Inventory = GetComponentInParent<InventoryManagement>();
        }

        private void Update() 
        {
            GetComponent<DragDropItem>().enabled = ItemInstanceId != Guid.Empty;
        }

        private void OnDestroy() 
        {
            ClearItemGUI();
        }

        public void SetItemGUI(ItemModel item)
        {
            ItemInstanceId = item.InstanceId;
            ItemTransform.gameObject.SetActive(true);

            //set image item
            ItemImage.transform.rotation = item.IsWeapon ? Quaternion.Euler(0, 0, 45) : Quaternion.Euler(0, 0, 0);
            ItemImage.sprite = item.Sprite;
            ItemImage.gameObject.SetActive(true);


            //set amount
            if(item.IsStackable)
            {
                ItemAmountText?.SetText(item.Amount.ToString(Formatter.Amount));
                ItemAmountText?.gameObject.SetActive(true);
            }
            else
            {
                ItemAmountText?.gameObject.SetActive(false);
            }

            //set onclick
            ItemMouseEvent.OnLeftClick.AddListener(() => { Inventory.UseItem(item); });
            ItemMouseEvent.OnRightClick.AddListener(() => { Inventory.DropItem(item); });
        }

        public void ClearItemGUI()
        {
            ItemInstanceId = Guid.Empty;
            ItemTransform.gameObject.SetActive(false);

            //set image item
            ItemImage.sprite = null;
            ItemImage.gameObject.SetActive(false);

            //set amount
            ItemAmountText?.SetText("0");
            ItemAmountText?.gameObject.SetActive(false);

            //set onclick
            ItemMouseEvent.OnLeftClick.RemoveAllListeners();
            ItemMouseEvent.OnRightClick.RemoveAllListeners();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
            if(slot != null && slot?.ItemInstanceId != null)
            {
                if(ItemInstanceId == Guid.Empty)
                {
                    //set this item slot
                    ClearItemGUI();
                    SetItemGUI(Inventory.GetItem(slot.ItemInstanceId));

                    //clear source item slot
                    slot.ClearItemGUI();
                }
            }
        }

    }
}

