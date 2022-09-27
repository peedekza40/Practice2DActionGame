using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Item Assets in the scene. Destroy the newest one.");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        ItemWorld.SpawnItemWorld(new Vector3(2, -2.8f, 0), new Item(Constants.ItemType.HeathPotion, 1));
        ItemWorld.SpawnItemWorld(new Vector3(4, -2.8f, 0), new Item(Constants.ItemType.ManaPotion, 1));
    }

    public GameObject PrefabSkeleton;

    public ItemWorld PrefabItemWorld;

    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    public Sprite SwordSprite;
    public Sprite DefaultSprite;
}
