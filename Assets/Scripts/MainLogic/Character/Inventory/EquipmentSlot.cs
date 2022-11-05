using System;
using Core.Constants;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Character.Inventory
{
    public class EquipmentSlot : ItemSlot, IEndDragHandler
    {
        public CanvasGroup ItemCanvasGroup;
        public EquipmentType Type;

        private Sprite IconSprite;
        private PlayerHandler PlayerHandler;

        protected override void Awake()
        {
            base.Awake();
            PlayerHandler = GetComponentInParent<PlayerHandler>();
            IconSprite = ItemImage.sprite;
        }

        private void Start() 
        {
            ItemModel item = Inventory.GetItem(ItemInstanceId);
            if(item != null)
            {
                ActionByType(() => { PlayerHandler.Combat.SetWeapon(item.Type); });    
            }
        }

        protected override void Update() 
        {
            base.Update();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
            if(slot != null && slot?.ItemInstanceId != null)
            {                
                if(ItemInstanceId == Guid.Empty)
                {
                    ItemModel item = Inventory.GetItem(slot.ItemInstanceId);

                    //check typ equipment
                    bool isCorrectType = false;
                    ActionByType(() => { isCorrectType = item.IsWeapon; });

                    if(isCorrectType)
                    {
                        //set this item slot
                        ClearItem();
                        SetItem(item);

                        //clear source item slot
                        slot.ClearItem();
                    }

                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(ItemInstanceId == Guid.Empty)
            {
                ClearItem();  
            }
        }

        public override void SetItem(ItemModel item)
        {
            base.SetItem(item);

            //clear on click
            ItemMouseEvent.OnLeftClick.RemoveAllListeners();
            ItemMouseEvent.OnRightClick.RemoveAllListeners();

            //set weapon
            ActionByType(() => { PlayerHandler.Combat.SetWeapon(item.Type); });

            //set opacity
            ItemCanvasGroup.alpha = 1f;
        }

        public override void ClearItem()
        {
            ItemInstanceId = Guid.Empty;

            //set image item
            ItemImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            ItemImage.sprite = IconSprite;

            //set amount
            ItemAmountText?.SetText("0");
            ItemAmountText?.gameObject.SetActive(false);

            //set onclick
            ItemMouseEvent.OnLeftClick.RemoveAllListeners();
            ItemMouseEvent.OnRightClick.RemoveAllListeners();

            //set weapon to null
            ActionByType(() => { PlayerHandler.Combat.SetWeapon(ItemType.None); });
        }

        private void ActionByType(UnityAction weaponTypeAction)
        {
            switch (Type)
            {
                case EquipmentType.Weapon : 
                    weaponTypeAction();
                    break;
            }
        }
    }
}