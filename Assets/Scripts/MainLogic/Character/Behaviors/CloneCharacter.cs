using Character;
using Core.Constants;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CloneCharacter : MonoBehaviour
{
    private GameObject TemplateCharacter;
    private EnemyStatus EnemyStatus;
    private EnemyAI EnemyAI;

    #region Dependencies
    private CharacterTemplates characterTemplates;
    private IInstantiator instantiator;
    #endregion

    [Inject]
    public void Init(
        CharacterTemplates characterTemplates,
        IInstantiator instantiator)
    {
        this.characterTemplates = characterTemplates;
        this.instantiator = instantiator;
    }

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
        GameObject newCharacter = instantiator.InstantiatePrefab(TemplateCharacter, new Vector3(5.13f, 5.13f, 0f), Quaternion.identity, null);
        newCharacter.GetComponent<EnemyAI>().Target = EnemyAI.Target;
        newCharacter.SetActive(true);
    }

    private GameObject GetTemplate()
    {
        GameObject template = null;
        switch(EnemyStatus.Type)
        {
            case EnemyType.Skeleton : 
                template = characterTemplates.PrefabSkeleton;
                break;
            default : 
                break;
        }

        return template;
    }
}
