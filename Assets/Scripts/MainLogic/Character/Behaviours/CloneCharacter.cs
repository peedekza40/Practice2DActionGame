using Core.Repositories;
using Infrastructure.Entity;
using UnityEngine;
using Zenject;

namespace Character.Behaviours
{
    public class CloneCharacter : MonoBehaviour
    {
        private GameObject TemplateCharacter;
        private EnemyStatus EnemyStatus;
        private EnemyAI EnemyAI;

        #region Dependencies
        private IEnemyConfigRepository enemyConfigRepository;
        private CharacterTemplates characterTemplates;
        private IInstantiator instantiator;
        #endregion

        [Inject]
        public void Init(
            IEnemyConfigRepository enemyConfigRepository,
            CharacterTemplates characterTemplates,
            IInstantiator instantiator)
        {
            this.enemyConfigRepository = enemyConfigRepository;
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
            EnemyConfig enemyConfig = enemyConfigRepository.GetByType(EnemyStatus.Type);
            GameObject template = Resources.Load<GameObject>(enemyConfig.PrefabPath);
            if(template == null)
            {
                template = characterTemplates.Default;
            }
            return template;
        }
    }

}
