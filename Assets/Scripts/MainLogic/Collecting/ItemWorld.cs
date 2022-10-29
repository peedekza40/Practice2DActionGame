using System.Collections;
using Character.Inventory;
using UnityEngine;

namespace Collecting
{
    public class ItemWorld : MonoBehaviour
    {
        public SpriteRenderer DefaultSprite;
        public SpriteRenderer WeaponSprite;
        public float WaitCollectingTime = 2f;

        public bool IsCanCollect { get; private set; }
        private ItemModel Item;
        private Rigidbody2D Rb;

        public static ItemWorld SpawnItemWorld(Vector3 position, ItemModel item, ItemAssets itemAssets)
        {
            ItemWorld itemWorld = Instantiate(itemAssets.PrefabItemWorld, position, Quaternion.identity);
            itemWorld.SetItem(item);

            return  itemWorld;
        }

        private void Awake() 
        {
            Rb = GetComponent<Rigidbody2D>();
            StartCoroutine(WaitSetIsCanCollect());
        }

        public void SetItem(ItemModel item)
        {
            Item = item;

            if(item.IsWeapon)
            {
                WeaponSprite.sprite = item.Sprite;
                WeaponSprite.gameObject.SetActive(true);
            }
            else
            {
                DefaultSprite.sprite = item.Sprite;
                DefaultSprite.gameObject.SetActive(true);
            }
        }

        public ItemModel GetItem()
        {
            return Item;
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        private IEnumerator WaitSetIsCanCollect()
        {
            yield return new WaitForSeconds(WaitCollectingTime);
            IsCanCollect = true;
        }
    }
 
}
