using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Constants;
using System.Linq;
using UnityEngine.InputSystem;
using Infrastructure.Dependency;
using Infrastructure.InputSystem;

namespace Character {
    public class PlayerCombat : MonoBehaviour, IPlayerCombat
    {
        [Header("Attack")]
        public float TimeBetweenCombo = 0.45f;
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
        private PlayerInputControl PlayerInputControl;
        private Gold Gold;
        private AnimatorStateInfo AnimationState;
        private FrameInput Input;
        private List<EnemyStatus> AttackedEnemies = new List<EnemyStatus>();

        void Awake()
        {
            CountAttack = 0;
            PlayerController = GetComponent<IPlayerController>();
            AnimatorController = GetComponentInChildren<IAnimatorController>();
            Gold = GetComponent<Gold>();
        }

        // Start is called before the first frame update
        void Start()
        {
            PlayerInputControl = DependenciesContext.Dependencies.Get<PlayerInputControl>();
            PlayerInputControl.AttackInput.performed += StartAttack;
            PlayerInputControl.BlockInput.performed += StartBlock;
            PlayerInputControl.BlockInput.canceled += FinishBlock;
        }

        // Update is called once per frame
        void Update()
        {
            AnimationState = AnimatorController.Animator.GetCurrentAnimatorStateInfo(0);
            Input = PlayerController.Input;
            SetIsAttacking(CountAttack);

            Attacking();
            Blocking();
            CheckAttackedCharaterDeath();
        }

        public float GetReduceDamage()
        {
            return ReduceDamage;
        }

        public void SetIsParry(bool isParry)
        {
            IsParry = isParry;
        }

        private void StartAttack(InputAction.CallbackContext context)
        {
            if(IsBlocking == false 
                && ((CountAttack < 3 && TimeSinceAttack > TimeBetweenAttack)
                || (CountAttack == 3 && TimeSinceAttack > TimeBetweenAttack + TimeBetweenCombo)))
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
        }

        private void Attacking()
        {
            // Increase timer that controls attack combo
            TimeSinceAttack += Time.deltaTime;

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
                    var attackedEnemy = hitEnemy.GetComponent<EnemyStatus>();
                    attackedEnemy?.TakeDamage(Damage, HitBox.gameObject);
                    AttackedEnemies.Add(attackedEnemy);
                    DamageFrameCount++;
                }
            }
            else if(IsAttacking == false)
            {
                DamageFrameCount = 0;
            }
        }

        private void StartBlock(InputAction.CallbackContext context)
        {
            if(IsAttacking == false && TimeSinceBlocked > 0.1f)
            {   
                TimeSinceBlocked = 0f;
                TimeSincePressBlock = 0f;
                IsBlocking = true;
            }
        }

        private void FinishBlock(InputAction.CallbackContext context)
        {
            IsBlocking = false;
        }

        private void Blocking()
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

            AnimatorController.SetBlock(IsBlocking);
        }

        private void CheckAttackedCharaterDeath()
        {
            var deathAttackedEnemies = AttackedEnemies.Where(x => x.IsDeath).ToList();
            foreach(var deathAttackedEnemy in deathAttackedEnemies)
            {
                if(deathAttackedEnemy.GetIsCollectedGold() == false)
                {
                    Debug.Log($"Collect gold enemy : {deathAttackedEnemy.name}");
                    Gold.Collect(deathAttackedEnemy.Type);
                    deathAttackedEnemy.SetIsCollectedGold(true);
                    AttackedEnemies.Remove(deathAttackedEnemy);
                }

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
