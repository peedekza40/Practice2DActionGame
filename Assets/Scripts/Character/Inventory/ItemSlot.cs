using System;
using Constants;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public RectTransform SlotTransform { get; private set; }
    public Transform ItemTransform { get; private set; }
    public Image ItemImage { get; private set; }
    public TextMeshProUGUI ItemAmountText { get; private set; }
    public MouseEvent ItemMouseEvent { get; private set; }

    public Guid ItemID { get; private set; }
    private Inventory Inventory;

    private void Awake() 
    {
        SlotTransform = GetComponent<RectTransform>();
        ItemTransform = SlotTransform.Find(GameObjectName.Item);
        ItemImage = ItemTransform.Find(GameObjectName.ItemImage).GetComponent<Image>();
        ItemAmountText = ItemTransform.Find(GameObjectName.ItemAmount).GetComponent<TextMeshProUGUI>();
        ItemMouseEvent = SlotTransform.GetComponent<MouseEvent>();
        Inventory = GetComponentInParent<Inventory>();
    }

    private void Update() 
    {
        GetComponent<DragDropItem>().enabled = ItemID != Guid.Empty;
    }

    private void OnDestroy() 
    {
        ClearItemGUI();
    }

    public void SetItemGUI(Item item)
    {
        ItemID = item.ID;
        ItemTransform.gameObject.SetActive(true);

        //set image item
        ItemImage.sprite = item.GetSprite();
        ItemImage.gameObject.SetActive(true);

        //set amount
        if(item.IsStackable())
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
        ItemID = Guid.Empty;
        ItemTransform.gameObject.SetActive(false);

        //set image item
        ItemImage.sprite = null;
        ItemImage.gameObject.SetActive(false);

        //set amount
        ItemAmountText.SetText("0");
        ItemAmountText.gameObject.SetActive(false);

        //set onclick
        ItemMouseEvent.OnLeftClick.RemoveAllListeners();
        ItemMouseEvent.OnRightClick.RemoveAllListeners();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop Item");

        ItemSlot slot = eventData.pointerDrag.GetComponent<ItemSlot>();
        if(slot != null && slot?.ItemID != null)
        {
            if(ItemID == Guid.Empty)
            {
                //set this item slot
                ClearItemGUI();
                SetItemGUI(Inventory.GetItem(slot.ItemID));

                //clear source item slot
                slot.ClearItemGUI();
            }
        }
    }

}
