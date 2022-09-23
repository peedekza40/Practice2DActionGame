using Constants;
using UnityEngine;

public class Item
{
    public ItemType Type;
    public int Amount;

    public Item(ItemType type, int amount = 1)
    {
        Type = type;
        Amount = amount;
    }

    public Item()
    {
    }

    public Sprite GetSprite()
    {
        Sprite sprite = null;
        
        switch(Type)
        {
            case ItemType.HeathPotion : 
                sprite = ItemAssets.Instantce.HealthPotionSprite;
                break;
            case ItemType.ManaPotion : 
                sprite = ItemAssets.Instantce.ManaPotionSprite;
                break;
            case ItemType.Sword : 
                sprite = ItemAssets.Instantce.SwordSprite;
                break;
        }

        if(sprite == null)
        {
            sprite = ItemAssets.Instantce.DefaultSprite;
        }

        return sprite;
    }
}
