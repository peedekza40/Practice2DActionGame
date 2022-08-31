using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Constants;

namespace Character 
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public HealthBar HealthBar;

        private IAnimatorController CharacterAnimatorController;
        private BlockFlashAnimatorController BlockFlashAnimatorController;
        private IPlayerCombat PlayerCombat;
        private Rigidbody2D Rigidbody;
        private EnemyAI EnemyAi;
        private float CurrentHP;
        private bool IsDeath = false;

        // Start is called before the first frame update
        void Start()
        {
            CharacterAnimatorController = GetComponent<IAnimatorController>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            PlayerCombat = GetComponent<IPlayerCombat>();
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

                //player section
                if(gameObject.tag == "Player")
                {
                    var reduceDamage = 0f;
                    if(PlayerCombat.IsBlocking)
                    {
                        if(PlayerCombat.PressBlockThisFrame)
                        {
                            reduceDamage = damage;
                            BlockFlashAnimatorController.TriggerParry();
                        }
                        else
                        {
                            reduceDamage = PlayerCombat.GetReduceDamage();
                        }

                        damage -= reduceDamage;
                    }
                }

                CurrentHP -= damage;
                CharacterAnimatorController.TriggerAttacked();
                HealthBar.SetHealth(CurrentHP);
            }
        }

        public virtual void Die()
        {
            Rigidbody.simulated = false;
            CharacterAnimatorController.SetDeath();
        }
    }
}
