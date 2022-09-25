using Character;
using Constants;
using UnityEngine;

public class CloneCharacter : MonoBehaviour
{
    private GameObject TemplateCharacter;
    private EnemyStatus EnemyStatus;

    private void Awake() 
    {
        EnemyStatus = GetComponent<EnemyStatus>();
    }

    private void Start() 
    {
        TemplateCharacter = GetTemplate();
    }

    public void Clone()
    {
        Debug.Log("Clone character : " + gameObject.name);
        GameObject newCharacter = Instantiate(TemplateCharacter, new Vector3(5.13f, 5.13f, 0f), Quaternion.identity);
        newCharacter.SetActive(true);
    }

    private GameObject GetTemplate()
    {
        GameObject template = null;
        switch(EnemyStatus.Type)
        {
            case EnemyType.Skeleton : 
                template = CharacterTemplates.Instantce.PrefabSkeleton;
                break;
            default : 
                break;
        }

        return template;
    }
}
