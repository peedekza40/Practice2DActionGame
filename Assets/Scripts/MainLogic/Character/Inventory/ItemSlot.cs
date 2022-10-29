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
        public RectTransform SlotTransform { get; private set; }
        public Transform ItemTransform { get; private set; }
        public Image ItemImage { get; private set; }
        public Image WeaponImage { get; private set; }
        public TextMeshProUGUI ItemAmountText { get; private set; }
        public MouseEvent ItemMouseEvent { get; private set; }

        public Guid ItemId { get; private set; }
        private InventoryManagement Inventory;

        private void Awake() 
        {
            SlotTransform = GetComponent<RectTransform>();
            ItemTransform = SlotTransform.Find(GameObjectName.Item);
            ItemImage = ItemTransform.Find(GameObjectName.ItemImage).GetComponent<Image>();
            WeaponImage = ItemTransform.Find(GameObjectName.WeaponImage).GetComponent<Image>();
            ItemAmountText = ItemTransform.Find(GameObjectName.ItemAmount).GetComponent<TextMeshProUGUI>();
            ItemMouseEvent = SlotTransform.GetComponent<MouseEvent>();
            Inventory = GetComponentInParent<InventoryManagement>();
        }

        private void Update() 
        {
            GetComponent<DragDropItem>().enabled = ItemId != Guid.Empty;
        }

        private void OnDestroy() 
        {
            ClearItemGUI();
        }

        public void SetItemGUI(ItemModel item)
        {
            ItemId = item.Id;
            ItemTransform.gameObject.SetActive(true);

            //set image item
            if(item.IsWeapon)
            {
                WeaponImage.sprite = item.Sprite;
                WeaponImage.gameObject.SetActive(true);
                ItemImage.gameObject.SetActive(false);
            }
            else
            {
                ItemImage.sprite = item.Sprite;
                ItemImage.gameObject.SetActive(true);
                WeaponImage.gameObject.SetActive(false);
            }


            //set amount
            if(item.IsStackable)
            {
                ItemAmountText.SetText(item.Amount.ToString(Formatter.Amount));
                ItemAmountText.gameObject.SetActive(true);
            }
            else
            {
                ItemAmountText.gameObject.SetActive(false);
            }

            //set onclick
            ItemMouseEvent.OnLeftClick.AddListener(() => { Inventory.UseItem(item); });
            ItemMouseEvent.OnRightClick.AddListener(() => { Inventory.DropItem(item); });
        }

        public void ClearItemGUI()
        {
            ItemId = Guid.Empty;
            ItemTransform.gameObject.SetActive(false);

            //set image item
            ItemImage.sprite = null;
            WeaponImage.sprite = null;
            ItemImage.gameObject.SetActive(false);
            WeaponImage.gameObject.SetActive(false);

            //set amount
            ItemAmountText.SetText("0");
            ItemAmountText.gameObject.SetActive(false);

            //set onclick
            ItemMouseEvent.OnLeftClick.RemoveAllListeners();
            ItemMouseEvent.OnRightClick.RemoveAllListeners();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
            if(slot != null && slot?.ItemId != null)
            {
                if(ItemId == Guid.Empty)
                {
                    //set this item slot
                    ClearItemGUI();
                    SetItemGUI(Inventory.GetItem(slot.ItemId));

                    //clear source item slot
                    slot.ClearItemGUI();
                }
            }
        }

    }
}

