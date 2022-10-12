using Infrastructure.Extensions;
using UnityEngine;

public class ObjectDropItems : MonoBehaviour
{
    public WeightedRandomList<ItemModel> Items;
    public int DropAmount;
    
    public void DropItems()
    {
        int sumDropedAmount = 0;
        while(sumDropedAmount < DropAmount)
        {
            ItemModel dropItem = Items.GetRandom();
            if(dropItem.Amount > 0)
            {
                dropItem.Amount--;
                sumDropedAmount++;

                //control physic
                Vector2 dropDirection = RandomVectorDirection();
                Vector2 dropPostion = new Vector2(0, 0.5f);

                ItemWorld dropItemWorld = ItemWorld.SpawnItemWorld((Vector2)transform.position + dropPostion, new ItemModel(dropItem.Type, 1));
                dropItemWorld.GetComponent<Rigidbody2D>().AddForce(dropDirection * 2.5f, ForceMode2D.Impulse);
            }
        }
    }

    private Vector2 RandomVectorDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f));
    }
}