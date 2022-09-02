using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using System.Linq;

namespace Character {
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        [Header("Attack")]
        public float TimeBetweenAttack = 0.4f;
        public float TimeBetweenNextMove = 0.35f;
        public float Damage = 20f;
        public Collider2D HitBox;
        public LayerMask EnemyLayers;
        public int? CountAttack { get; private set; }
        public bool IsAttacking { get; private set; }
        private float TimeSinceAttack = 0f;
        private int DamageFrameCount = 0;

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

            if(IsBlocking == false && Input.AttackDown && TimeSinceAttack > TimeBetweenAttack)
            {
                CountAttack++;
                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 3 || TimeSinceAttack > TimeBetweenAttack + TimeBetweenNextMove)
                {
                    CountAttack = 1;
                }
                
                //update animation
                AnimatorController.TriggerAttack(CountAttack);

                //Reset
                TimeSinceAttack = 0f;
                DamageFrameCount = 0;
            }

            //damage enemy
            if(IsAttacking && DamageFrameCount == 0)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = new List<Collider2D>();
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(EnemyLayers);
                HitBox.OverlapCollider(contactFilter, hitEnemies);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    hitEnemy.GetComponent<CharacterStatus>().TakeDamage(Damage, HitBox.gameObject);
                    DamageFrameCount++;
                }
            }
            else if(IsAttacking == false)
            {
                DamageFrameCount = 0;
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

            if(IsAttacking == false)
            {
                if(Input.Blocking && TimeSinceBlocked > 0.1f)
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
        }

        private void SetIsAttacking(int? countAttack)
        {
            IsAttacking = false;
            if(countAttack != null && countAttack > 0)
            {
                IsAttacking = AnimationState.IsName($"{AnimationName.Attack}{countAttack}");
            }
        }



    }
}
