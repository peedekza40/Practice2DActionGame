using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("UI")]
    public GameObject InvetoryContainer;
    public Transform ItemContainer;
    public Transform SlotTemplate;
    
    private List<Item> Items { get; set; } = new List<Item>();
    private bool IsOpen { get; set; }

    private IPlayerController PlayerController;

    private void Awake() 
    {
        PlayerController = GetComponent<IPlayerController>();
    }

    private void Start() 
    {
        IsOpen = false;
        AddItem(new Item(ItemType.HeathPotion, 10000));
        AddItem(new Item(ItemType.Sword, 1));
        AddItem(new Item(ItemType.ManaPotion, 1));

        PlayerInputControl.Instance.ToggleInventoryInput.performed += ToggleInventory;
    }

    private void Update() 
    {
        Cursor.visible = IsOpen;
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

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsOpen = !IsOpen;
        InvetoryContainer.SetActive(IsOpen);
    }

    private void RefreshInventory()
    {
        //destroy complex items
        foreach(Transform child in ItemContainer)
        {
            if(child != SlotTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        int x = 0;
        int y = 0;
        float slotCellSize = 87f;
        Vector2 startAnchoredPosition = SlotTemplate.GetComponent<RectTransform>().anchoredPosition;
        foreach(var item in Items)
        {
            RectTransform slotRectTransform = Instantiate(SlotTemplate, ItemContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.SetActive(true);
            slotRectTransform.anchoredPosition = new Vector2((x * slotCellSize) + startAnchoredPosition.x, -(y * slotCellSize) + startAnchoredPosition.y);

            //set image item
            Image image = slotRectTransform.Find(GameObjectName.ItemImage).GetComponent<Image>();
            image.sprite = item.GetSprite();

            //set amount
            TextMeshProUGUI amountText = slotRectTransform.Find(GameObjectName.ItemAmount).GetComponent<TextMeshProUGUI>();
            if(item.IsStackable())
            {
                amountText.gameObject.SetActive(true);
                amountText.SetText(item.Amount.ToString(Formatter.Amount));
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }

            x++;
            if(x > 3)
            {
                x = 0;
                y++;
            }
        }
    }

}
