using System.Collections;
using UnityEngine;

namespace Character
{
    public class EnemyStatus : CharacterStatus
    {
        public EnemyType Type;

        private EnemyAI EnemyAi;
        private bool IsCollectedGold;

        void Awake()
        {
            EnemyAi = GetComponent<EnemyAI>();
            base.BaseAwake();
        }

        void Update()
        {
            base.BaseUpdate();
        }

        public override void Die()
        {
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
