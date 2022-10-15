using System.Collections;
using UnityEngine;
using Zenject;

public class ItemWorld : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
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
        SpriteRenderer.sprite = item.Sprite;
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
