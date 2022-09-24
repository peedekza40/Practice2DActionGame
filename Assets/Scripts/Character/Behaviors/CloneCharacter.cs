using UnityEngine;

public class CloneCharacter : MonoBehaviour
{
    private GameObject TemplateCharacter;

    private void Start() 
    {
        TemplateCharacter = ItemAssets.Instantce.PrefabSkeleton;
    }

    public void Clone()
    {
        Debug.Log("Clone character : " + gameObject.name);
        GameObject newCharacter = Instantiate(TemplateCharacter, new Vector3(5.13f, 5.13f, 0f), Quaternion.identity);
        newCharacter.SetActive(true);
    }
}
