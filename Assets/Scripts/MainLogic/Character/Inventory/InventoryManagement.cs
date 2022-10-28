using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Interfaces;
using Collecting;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

namespace Character.Inventory
{
    public class InventoryManagement : MonoBehaviour, IUIPersistence, IDataPersistence
    {
        [Header("UI")]
        public GameObject InventoryContainer;
        public Transform ItemContainerTransform;
        public ItemSlot SlotTemplate;
        public int SlotAmount = 16;

        [Header("Function")]
        public UnityAction<ItemModel> UseItemAction;
        
        private List<ItemSlot> Slots { get; set; } = new List<ItemSlot>();
        private List<ItemModel> Items { get; set; } = new List<ItemModel>();

        private IPlayerController PlayerController;

        #region Dependencies
        private PlayerInputControl playerInputControl;
        private ItemAssets itemAssets;
        private DiContainer diContainer;
        #endregion

        #region IUIPersistence
        public UINumber Number => UINumber.Inventory;
        public bool IsOpen { get; private set; }
        public MouseEvent MouseEvent { get; private set; }
        #endregion

        [Inject]
        public void Init(
            PlayerInputControl playerInputControl,
            ItemAssets itemAssets,
            DiContainer diContainer)
        {
            this.playerInputControl = playerInputControl;
            this.itemAssets = itemAssets;
            this.diContainer = diContainer;
        }

        private void Awake() 
        {
            PlayerController = GetComponent<IPlayerController>();
            MouseEvent = InventoryContainer.GetComponentInParent<MouseEvent>();

            IsOpen = false;
            DrawItemSlot();
        }

        private void Start() 
        {
            playerInputControl.ToggleInventoryInput.performed += ToggleInventory;
        }

        public ItemModel GetItem(System.Guid id)
        {
            return Items.FirstOrDefault(x => x.Id == id);
        }

        public bool IsFull(ItemModel item)
        {
            bool isFull = Items.Count() >= SlotAmount;
            ItemModel itemInventory = Items.FirstOrDefault(x => x.Type == item.Type);
            if(itemInventory != null && itemInventory.IsStackable)
            {
                isFull = false;
            }
            return isFull;
        }

        public void AddItem(ItemModel item)
        {
            if(item != null)
            {
                if(item.IsStackable)
                {
                    ItemModel inventoryItem = Items.FirstOrDefault(x => x.Type == item.Type);
                    if(inventoryItem != null)
                    {
                        inventoryItem.Amount += item.Amount;
                    }
                    else
                    {
                        Items.Add(item);
                    }
                }
                else
                {
                    Items.Add(item);
                }

                RefreshInventory();
            }
        }

        public void UseItem(ItemModel item)
        {
            if(item.IsCanUse)
            {
                Debug.Log("Use item : " + item.Type);
                UseItemAction?.Invoke(item);
                //remove item in inventory
                RemoveItem(item);
            }

        }

        public void DropItem(ItemModel item)
        {
            Debug.Log("Drop item : " + item.Type);

            //remove item in inventory
            ItemModel removeItem = RemoveItem(item);

            //control physic
            Vector2 dropDirection = new Vector2(1, Random.Range(0.2f, 1f));
            Vector2 dropPostion = new Vector2(0, 0.1f);
            if(transform.localScale.x < 0)
            {
                dropDirection.x *= -1;
            }

            ItemModel newItem = diContainer.Instantiate<ItemModel>();
            newItem.Setup(removeItem.Type, 1);
            ItemWorld dropItem = ItemWorld.SpawnItemWorld((Vector2)transform.position + dropPostion, newItem, itemAssets);
            dropItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * 2f, ForceMode2D.Impulse);

        }

        private ItemModel RemoveItem(ItemModel item)
        {
            ItemModel removeItem = Items.FirstOrDefault(x => x.Id == item.Id);
            if(removeItem.IsStackable)
            {
                removeItem.Amount--;
                if(removeItem.Amount == 0)
                {
                    Items.Remove(removeItem);
                }
            }
            else
            {
                Items.Remove(removeItem);
            }

            RefreshInventory();
            
            return removeItem;
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            IsOpen = !IsOpen;
            InventoryContainer.SetActive(IsOpen);
        }

        private void RefreshInventory()
        {
            List<ItemSlot> tempSlots = new List<ItemSlot>();
            foreach(var item in Items)
            {
                ItemSlot currentItemInSlot = Slots.FirstOrDefault(x => x.ItemId == item.Id);
                if(currentItemInSlot != null)
                {
                    currentItemInSlot.ClearItemGUI();
                    currentItemInSlot.SetItemGUI(item);
                    tempSlots.Add(currentItemInSlot);
                }
                else
                {
                    ItemSlot slot = Slots.FirstOrDefault(x => x.ItemId == System.Guid.Empty);
                    slot.SetItemGUI(item);
                    tempSlots.Add(slot);
                }
            }

            var clearSlots = Slots.Except(tempSlots);
            foreach(var clearSlot in clearSlots)
            {
                clearSlot.ClearItemGUI();
            }
        }

        private void ClearInventory()
        {
            foreach (var slot in Slots)
            {
                slot.ClearItemGUI();
            }
        }

        private void DrawItemSlot()
        {
            InventoryContainer.SetActive(true);

            int x = 0;
            int y = 0;
            float slotCellSize = 144.3f;
            Vector2 startAnchoredPosition = SlotTemplate.GetComponent<RectTransform>().anchoredPosition;

            for(int i = 0; i < SlotAmount; i++)
            {
                ItemSlot slot = Instantiate(SlotTemplate, ItemContainerTransform);
                slot.gameObject.SetActive(true);
                slot.SlotTransform.anchoredPosition = new Vector2((x * slotCellSize) + startAnchoredPosition.x, -(y * slotCellSize) + startAnchoredPosition.y);
                Slots.Add(slot);

                x++;
                if(x > 3)
                {
                    x = 0;
                    y++;
                }
            }

            InventoryContainer.SetActive(IsOpen);
        }

        public void LoadData(GameDataModel data)
        {
            Items = data.PlayerData.Items;
            foreach(var item in Items)
            {
                diContainer.Inject(item);
                item.Setup(item.Type, item.Amount);
            }
            ClearInventory();
            RefreshInventory();
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.Items = Items;
        }
    }
  
}
