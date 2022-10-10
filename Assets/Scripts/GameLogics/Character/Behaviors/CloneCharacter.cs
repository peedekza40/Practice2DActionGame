using Character;
using Constants;
using UnityEngine;

public class CloneCharacter : MonoBehaviour
{
    private GameObject TemplateCharacter;
    private EnemyStatus EnemyStatus;
    private EnemyAI EnemyAI;

    private void Awake() 
    {
        EnemyStatus = GetComponent<EnemyStatus>();
        EnemyAI = GetComponent<EnemyAI>();
    }

    private void Start() 
    {
        TemplateCharacter = GetTemplate();
    }

    public void Clone()
    {
        Debug.Log("Clone character : " + gameObject.name);
        GameObject newCharacter = Instantiate(TemplateCharacter, new Vector3(5.13f, 5.13f, 0f), Quaternion.identity);
        newCharacter.GetComponent<EnemyAI>().Target = EnemyAI.Target;
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
