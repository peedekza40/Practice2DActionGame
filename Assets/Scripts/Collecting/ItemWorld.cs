using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        ItemWorld itemWorld = Instantiate(ItemAssets.Instance.PrefabItemWorld, position, Quaternion.identity);
        itemWorld.SetItem(item);

        return  itemWorld;
    }

    private Item Item;
    private SpriteRenderer SpriteRenderer;

    private void Awake() 
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();    
    }

    public void SetItem(Item item)
    {
        Item = item;
        SpriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return Item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
