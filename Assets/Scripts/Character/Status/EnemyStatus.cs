using System.Collections;
using UnityEngine;

namespace Character
{
    public class EnemyStatus : CharacterStatus
    {
        private EnemyAI EnemyAi;

        void Start()
        {
            EnemyAi = GetComponent<EnemyAI>();
            base.BaseStart();
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

        private IEnumerator WaitForDestroy()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
            Debug.Log("Destroy : " + gameObject.name);
        }
    }
}
