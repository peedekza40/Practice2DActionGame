using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

namespace Character {
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        [Header("Attack")]
        public float TimeBetweenNextMove = 0.75f;
        public float AttackRange = 0.75f;
        public float Damage = 20f;
        public Transform AttackPoint;
        public LayerMask EnemyLayers;
        public int? CountAttack { get; private set; }
        public bool IsAttacking { get; private set; }
        private Collider2D HitBox;
        private float TimeSinceAttack = 0f;

        [Header("Block")]
        public float ReduceDamage = 5f;
        public bool IsBlocking { get; private set; }
        public bool PressBlockThisFrame { get; private set; }
        public bool IsParry { get; private set; }
        private float TimeSinceBlocked = 0f;
        private float TimeSincePressBlock = 0f;

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
            HitBox = GetComponentInChildren<Collider2D>();
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

        public float GetReduceDamage()
        {
            return ReduceDamage;
        }

        public void SetIsParry(bool isParry)
        {
            IsParry = isParry;
        }

        private void Attack()
        {
            // Increase timer that controls attack combo
            TimeSinceAttack += Time.deltaTime;

            if(IsBlocking == false && Input.AttackDown && TimeSinceAttack > 0.25f)
            {
                CountAttack++;

                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 3 || TimeSinceAttack > TimeBetweenNextMove)
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
                TimeSinceAttack = 0f;
            }
        }

        private void Block()
        {
            PressBlockThisFrame = false;
            IsParry = false;
            TimeSincePressBlock += Time.deltaTime;
            if(IsBlocking == false)
            {
                TimeSinceBlocked += Time.deltaTime;
            }

            if(TimeSincePressBlock <= 0.15)
            {
                PressBlockThisFrame = true;
            }

            if(Input.BlockDown && TimeSinceBlocked > 0.1f)
            {   
                TimeSinceBlocked = 0f;
                TimeSincePressBlock = 0f;
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
