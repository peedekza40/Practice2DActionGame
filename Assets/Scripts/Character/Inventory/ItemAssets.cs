using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instantce { get; private set; }

    private void Awake()
    {
        Instantce = this;
        DontDestroyOnLoad(this.gameObject);

        ItemWorld.SpawnItemWorld(new Vector3(2, -2.8f, 0), new Item(Constants.ItemType.HeathPotion, 1));
        ItemWorld.SpawnItemWorld(new Vector3(4, -2.8f, 0), new Item(Constants.ItemType.ManaPotion, 1));
    }

    public ItemWorld PrefabItemWorld;

    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    public Sprite SwordSprite;
    public Sprite DefaultSprite;
}
