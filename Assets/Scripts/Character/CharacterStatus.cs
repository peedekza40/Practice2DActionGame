using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace Character 
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public HealthBar HealthBar;

        private IAnimatorController AnimatorController;
        private Rigidbody2D Rigidbody;
        private EnemyAI EnemyAi;
        private float CurrentHP;
        private bool IsDeath = false;

        // Start is called before the first frame update
        void Start()
        {
            AnimatorController = GetComponent<IAnimatorController>();
            Rigidbody = GetComponent<Rigidbody2D>();
            EnemyAi = GetComponent<EnemyAI>();

            CurrentHP = MaxHP;
            HealthBar.SetMaxHealth(MaxHP);
        }

        // Update is called once per frame
        void Update()
        {
            if(CurrentHP <= 0)
            {
                if(EnemyAi != null)
                {
                    EnemyAi.enabled = false;
                }
                
                IsDeath = true;
                Die();
            }
        }

        public void TakeDamage(float damage)
        {
            if(IsDeath == false){
                CurrentHP -= damage;
                AnimatorController.TriggerAttacked();
                HealthBar.SetHealth(CurrentHP);
            }
        }

        public virtual void Die()
        {
            AnimatorController.SetDeath();
        }
    }
}
