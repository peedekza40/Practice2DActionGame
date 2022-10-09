using System.Collections.Generic;
using System.Linq;
using Character;
using Constants;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, IUIPersistence
{
    [Header("UI")]
    public GameObject InventoryContainer;
    public Transform ItemContainer;
    public ItemSlot SlotTemplate;
    public int SlotAmount = 16;

    [Header("Function")]
    public UnityAction<Item> UseItemAction;
    
    private List<ItemSlot> Slots { get; set; } = new List<ItemSlot>();
    private List<Item> Items { get; set; } = new List<Item>();
    public UINumber Number => UINumber.Inventory;
    public bool IsOpen { get; private set; }
    private bool IsDrewSlot { get; set; }

    private IPlayerController PlayerController;

    private void Awake() 
    {
        PlayerController = GetComponent<IPlayerController>();
        DrawItemSlot();
    }

    private void Start() 
    {
        IsOpen = false;
        PlayerInputControl.Instance.ToggleInventoryInput.performed += ToggleInventory;

        AddItem(new Item(ItemType.HeathPotion, 10));
        AddItem(new Item(ItemType.ManaPotion, 1));
        AddItem(new Item(ItemType.Sword, 1));
    }

    public Item GetItem(System.Guid id)
    {
        return Items.FirstOrDefault(x => x.ID == id);
    }

    public void AddItem(Item item)
    {
        if(item != null)
        {
            if(item.IsStackable())
            {
                Item inventoryItem = Items.FirstOrDefault(x => x.Type == item.Type);
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

    public void UseItem(Item item)
    {
        if(item.IsCanUse())
        {
            Debug.Log("Use item : " + item.Type);
            UseItemAction?.Invoke(item);
            //remove item in inventory
            RemoveItem(item);
        }

    }

    public void DropItem(Item item)
    {
        Debug.Log("Drop item : " + item.Type);

        //remove item in inventory
        Item removeItem = RemoveItem(item);

        //control physic
        Vector2 dropDirection = new Vector2(1, Random.Range(0.2f, 1f));
        Vector2 dropPostion = new Vector2(0, 0.1f);
        if(transform.localScale.x < 0)
        {
            dropDirection.x *= -1;
        }

        ItemWorld dropItem = ItemWorld.SpawnItemWorld((Vector2)transform.position + dropPostion, new Item(removeItem.Type, 1));
        dropItem.GetComponent<Rigidbody2D>().AddForce(dropDirection * 2f, ForceMode2D.Impulse);

    }

    private Item RemoveItem(Item item)
    {
        Item removeItem = Items.FirstOrDefault(x => x.Type == item.Type);
        if(removeItem.IsStackable())
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
        var cleanSlots = Slots.Where(slot => !Items.Any(item => item.ID == slot.ItemID));
        var haveItemSlots = Slots.Except(cleanSlots);

        foreach(var cleanSlot in cleanSlots)
        {
            cleanSlot.ClearItemGUI();
            Item item = Items.FirstOrDefault(item => !haveItemSlots.Any(slot => slot.ItemID == item.ID));
            if(item != null)
            {
                cleanSlot.SetItemGUI(item);
            }
        }

        foreach(var haveItemSlot in haveItemSlots)
        {
            Item item = Items.FirstOrDefault(item => item.ID == haveItemSlot.ItemID);
            haveItemSlot.ClearItemGUI();
            haveItemSlot.SetItemGUI(item);
            
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
            ItemSlot slot = Instantiate(SlotTemplate, ItemContainer);
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

        IsDrewSlot = true;
        InventoryContainer.SetActive(IsOpen);
    }
}
