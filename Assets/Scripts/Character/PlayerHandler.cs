using System.Collections;
using System.Collections.Generic;
using Character;
using Constants;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Assets")]
    public PlayerStatus Status;
    public Gold Gold;
    public Inventory Inventory;

    private void Awake() 
    {
        Inventory.UseItemAction += UseItem;    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        ItemWorld itemWorld = other.GetComponent<ItemWorld>();
        if(itemWorld != null)
        {
            Inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch(item.Type)
        {
            case ItemType.HeathPotion : 
                Status.AddCurrentHP(20);
                break;
            case ItemType.ManaPotion : 
                break;
        }
    }

}
