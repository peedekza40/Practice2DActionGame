using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character {
    public class PlayerAttack : MonoBehaviour, IPlayerAttack
    {
        public float TimeBetweenNextMove = 1f;
        public float AttackRange = 0.5f;
        public float Damage = 20f;
        public Transform AttackPoint;
        public LayerMask EnemyLayers;

        public int? CountAttack { get; private set; }
        private float timeSinceAttack = 0f;
        private IPlayerController playerController;
        private IAnimatorController animatorController;
        private FrameInput input;

        void Awake()
        {
            CountAttack = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            playerController = GetComponent<IPlayerController>();
            animatorController = GetComponentInChildren<IAnimatorController>();
        }

        // Update is called once per frame
        void Update()
        {
            input = playerController.Input;

            // Increase timer that controls attack combo
            timeSinceAttack += Time.deltaTime;

            if(input.AttackDown && timeSinceAttack > 0.25f)
            {
                CountAttack++;

                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 3 || timeSinceAttack > TimeBetweenNextMove)
                {
                    CountAttack = 1;
                }
                
                //update animation
                animatorController.TriggerAttack(CountAttack);

                //detect enemy in range of attack
                Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    hitEnemy.GetComponent<CharacterStatus>().TakeDamage(Damage);
                }

                //Reset time
                timeSinceAttack = 0f;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }

    }
}
