using Character;
using Core.Constant;
using Core.Constants;
using UnityEngine;

namespace Character 
{
    public class PlayerHandler : MonoBehaviour
    {
        [Header("Assets")]
        public PlayerStatus Status;
        public PlayerCombat Combat;
        public Gold Gold;
        public Inventory Inventory;

        private void Awake() 
        {
            Inventory.UseItemAction += UseItem;    
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            CollectItem(other);
        }
        
        private void OnTriggerStay2D(Collider2D other) 
        {
            CollectItem(other);
        }

        private void CollectItem(Collider2D other)
        {
            ItemWorld itemWorld = other.GetComponent<ItemWorld>();
            if(itemWorld != null && itemWorld.IsCanCollect)
            {
                Inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        private void UseItem(ItemModel item)
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
}

