using System.Collections;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public float WaitCollectingTime = 2f;

    public bool IsCanCollect { get; private set; }
    private Item Item;
    private Rigidbody2D Rb;

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        ItemWorld itemWorld = Instantiate(ItemAssets.Instance.PrefabItemWorld, position, Quaternion.identity);
        itemWorld.SetItem(item);

        return  itemWorld;
    }

    private void Awake() 
    {
        Rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitSetIsCanCollect());
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

    private IEnumerator WaitSetIsCanCollect()
    {
        yield return new WaitForSeconds(WaitCollectingTime);
        IsCanCollect = true;
    }
}
