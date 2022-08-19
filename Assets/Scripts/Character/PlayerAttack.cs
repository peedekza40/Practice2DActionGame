using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

namespace Character {
    public class PlayerAttack : MonoBehaviour, IPlayerAttack
    {
        [Header("Attack")]
        public float TimeBetweenNextMove = 1f;
        public float AttackRange = 0.5f;
        public float Damage = 20f;
        public Transform AttackPoint;
        public LayerMask EnemyLayers;
        public int? CountAttack { get; private set; }
        public bool IsAttacking { get; private set; }
        private float timeSinceAttack = 0f;

        [Header("Block")]
        public float ReduceDamage = 5f;
        public bool IsBlocking { get; private set; }

        private IPlayerController PlayerController;
        private IAnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private FrameInput Input;

        void Awake()
        {
            CountAttack = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerController = GetComponent<IPlayerController>();
            AnimatorController = GetComponentInChildren<IAnimatorController>();
        }

        // Update is called once per frame
        void Update()
        {
            AnimationState = AnimatorController.Animator.GetCurrentAnimatorStateInfo(0);
            Input = PlayerController.Input;
            SetIsAttacking(CountAttack);
            Attack();
            Block();
        }

        private void Attack()
        {
            // Increase timer that controls attack combo
            timeSinceAttack += Time.deltaTime;

            if(IsBlocking == false && Input.AttackDown && timeSinceAttack > 0.25f)
            {
                CountAttack++;

                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 3 || timeSinceAttack > TimeBetweenNextMove)
                {
                    CountAttack = 1;
                }
                
                //update animation
                AnimatorController.TriggerAttack(CountAttack);

                //detect enemy in range of attack
                Collider2D[] hitEnemies =  Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    hitEnemy.GetComponent<CharacterStatus>().TakeDamage(Damage);
                }

                //Reset
                timeSinceAttack = 0f;
            }
        }

        private void Block()
        {
            if(Input.BlockDown)
            {   
                IsBlocking = true;
            }

            if(Input.BlockUp)
            {
                IsBlocking = false;
            }

            AnimatorController.SetBlock(IsBlocking);
        }

        private void SetIsAttacking(int? countAttack)
        {
            IsAttacking = false;
            if(countAttack != null && countAttack > 0)
            {
                IsAttacking = AnimationState.IsName($"{AnimationName.Attack}{countAttack}");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
        }

    }
}
