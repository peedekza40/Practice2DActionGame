using UnityEngine;

public class CharacterTemplates : MonoBehaviour
{
    public static CharacterTemplates Instantce { get; private set; }

    private void Awake()
    {
        if(Instantce != null)
        {
            Debug.LogError("Found more than one Character Templates in the scene. Destroy the newest one.");
            Destroy(this.gameObject);
            return;
        }

        Instantce = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject PrefabSkeleton;
}
