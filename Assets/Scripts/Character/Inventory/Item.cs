using System;
using Constants;
using UnityEngine;

public class Item
{
    public Guid ID;
    public ItemType Type;
    public int Amount;

    public Item(ItemType type, int amount = 1)
    {
        ID = Guid.NewGuid();
        Type = type;
        Amount = amount;
    }
    
    public Sprite GetSprite()
    {
        Sprite sprite = null;
        
        switch(Type)
        {
            case ItemType.HeathPotion : 
                sprite = ItemAssets.Instance.HealthPotionSprite;
                break;
            case ItemType.ManaPotion : 
                sprite = ItemAssets.Instance.ManaPotionSprite;
                break;
            case ItemType.Sword : 
                sprite = ItemAssets.Instance.SwordSprite;
                break;
        }

        if(sprite == null)
        {
            sprite = ItemAssets.Instance.DefaultSprite;
        }

        return sprite;
    }

    public bool IsStackable()
    {
        bool isStackable = false;
        switch(Type)
        {
            case ItemType.HeathPotion : 
                isStackable = true;
                break;
            case ItemType.ManaPotion : 
                isStackable = true;
                break;
            case ItemType.Sword : 
                isStackable = false;
                break;
        }

        return isStackable;
    }
}
