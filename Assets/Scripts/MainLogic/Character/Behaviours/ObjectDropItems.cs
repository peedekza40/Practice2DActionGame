using Character.Inventory;
using Collecting;
using Infrastructure.Extensions;
using UnityEngine;
using Zenject;

namespace Character.Behaviours
{
    public class ObjectDropItems : MonoBehaviour
    {
        public WeightedRandomList<ItemModel> Items;
        public int DropAmount;
        
        #region Dependencies
        private ItemAssets itemAssets;
        private DiContainer diContainer;
        #endregion

        [Inject]
        public void Init(
            ItemAssets itemAssets,
            DiContainer diContainer)
        {
            this.itemAssets = itemAssets;
            this.diContainer = diContainer;
        }

        public void DropItems()
        {
            int sumDropedAmount = 0;
            int randomsDropAmount = Random.Range(0, DropAmount);
            while(sumDropedAmount < randomsDropAmount)
            {
                ItemModel dropItem = Items.GetRandom();
                if(dropItem.Amount > 0)
                {
                    dropItem.Amount--;
                    sumDropedAmount++;

                    //control physic
                    Vector2 dropDirection = RandomVectorDirection();
                    Vector2 dropPostion = new Vector2(0, 0.5f);


                    ItemModel newItem = diContainer.Instantiate<ItemModel>();
                    newItem.Setup(dropItem.Type, 1);
                    ItemWorld dropItemWorld = ItemWorld.SpawnItemWorld((Vector2)transform.position + dropPostion, newItem, itemAssets);
                    dropItemWorld.GetComponent<Rigidbody2D>().AddForce(dropDirection * 2.5f, ForceMode2D.Impulse);
                }
            }
        }

        private Vector2 RandomVectorDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
        }
    }
}

