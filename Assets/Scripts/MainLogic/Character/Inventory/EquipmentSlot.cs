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
                UnityAction weaponTypeAction = () => { PlayerHandler.Combat.SetWeapon(item.Type); }; 
                UnityAction bootTypeAction = () => { PlayerHandler.Status.SetBoot(item.Type); }; 
                ActionByType(weaponTypeAction, bootTypeAction);    
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
                    bool isCorrectType = item.EquipmentType == Type;
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

            //set action
            UnityAction weaponTypeAction = () => { PlayerHandler.Combat.SetWeapon(item.Type); }; 
            UnityAction bootTypeAction = () => { PlayerHandler.Status.SetBoot(item.Type); }; 
            ActionByType(weaponTypeAction, bootTypeAction);

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

            //set action
            UnityAction weaponTypeAction = () => { PlayerHandler.Combat.SetWeapon(ItemType.None); }; 
            UnityAction bootTypeAction = () => { PlayerHandler.Status.SetBoot(ItemType.None); }; 
            ActionByType(weaponTypeAction, bootTypeAction);
        }

        private void ActionByType(UnityAction weaponTypeAction = null, UnityAction bootTypeAction = null)
        {
            switch (Type)
            {
                case EquipmentType.Weapon : 
                    if(weaponTypeAction != null)
                    {
                        weaponTypeAction();
                    }
                    break;
                case EquipmentType.Boot :
                    if(bootTypeAction != null)
                    {
                        bootTypeAction();
                    }
                    break;
            }
        }
    }
}