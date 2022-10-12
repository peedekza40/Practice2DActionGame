using System;
using Core.Constants;
using Infrastructure.Dependency;
using UnityEngine;

[Serializable]
public class ItemModel
{
    public Guid ID;
    public ItemType Type;
    public int Amount;

    #region Dependencies
    private ItemAssets ItemAssets;
    #endregion

    public ItemModel(ItemType type, int amount = 1)
    {
        ID = Guid.NewGuid();
        Type = type;
        Amount = amount;

        ItemAssets = DependenciesContext.Dependencies.Get<ItemAssets>();
    }
    
    public Sprite GetSprite()
    {
        Sprite sprite = null;
        
        switch(Type)
        {
            case ItemType.HeathPotion : 
                sprite = ItemAssets.HealthPotionSprite;
                break;
            case ItemType.ManaPotion : 
                sprite = ItemAssets.ManaPotionSprite;
                break;
            case ItemType.Sword : 
                sprite = ItemAssets.SwordSprite;
                break;
        }

        if(sprite == null)
        {
            sprite = ItemAssets.DefaultSprite;
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

    public bool IsCanUse()
    {
        bool isCanUse = false;
        switch(Type)
        {
            case ItemType.HeathPotion : 
                isCanUse = true;
                break;
            case ItemType.ManaPotion : 
                isCanUse = true;
                break;
            case ItemType.Sword : 
                isCanUse = false;
                break;
        }

        return isCanUse;
    }
}
