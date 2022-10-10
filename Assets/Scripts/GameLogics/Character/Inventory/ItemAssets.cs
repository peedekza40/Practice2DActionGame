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
    }

    public ItemWorld PrefabItemWorld;

    public Sprite HealthPotionSprite;
    public Sprite ManaPotionSprite;
    public Sprite SwordSprite;
    public Sprite DefaultSprite;
}
