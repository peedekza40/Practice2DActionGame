using System.Collections;
using Core.Constants;
using UnityEngine;

namespace Character.Status
{
    public class EnemyStatus : CharacterStatus
    {
        public EnemyAttribute Attribute;
        public EnemyId Type;

        private EnemyAI EnemyAi;
        private bool IsCollectedGold;

        protected override void Awake()
        {
            BaseAttribute = Attribute;
            EnemyAi = GetComponent<EnemyAI>();
            base.Awake();
        }

        public override void Die()
        {
            EnemyAi.ResetStateMachine();
            EnemyAi.enabled = false;
            base.Die();
            StartCoroutine(WaitForDestroy());
        }

        public void SetIsCollectedGold(bool isCollectedGold)
        {
            IsCollectedGold = isCollectedGold;
        }

        public bool GetIsCollectedGold()
        {
            return IsCollectedGold;
        }

        private IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
            Debug.Log("Destroy : " + gameObject.name);
        }


    }
}
