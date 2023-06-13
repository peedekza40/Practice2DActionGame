using System.Collections.Generic;
using System.Linq;
using Character.Animators;
using Character.Status;
using Collecting;
using Core.Constants;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Combat.States.Player
{
    public class MeleeBaseState : State
    {
        public float Duration;
        protected bool ShouldCombo;
        protected int AttackIndex;
        protected PlayerHandler PlayerHandler;
        protected AnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        private Gold Gold;
        private List<EnemyStatus> AttackedEnemies = new List<EnemyStatus>();


        #region Calculate attack
        protected float MaxDamage;
        protected float WeaponDamage;
        protected LayerMask EnemyLayers;
        protected Collider2D HitBox;
        protected bool IsDamaged;
        protected float StaminaUse;
        #endregion

        #region Dependencies
        protected PlayerInputControl playerInputControl;
        #endregion 

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            Gold = GetComponent<Gold>();
            PlayerHandler = GetComponent<PlayerHandler>();
            MaxDamage = PlayerHandler.Combat.MaxDamage;
            WeaponDamage = PlayerHandler.Combat.CurrentWeapon?.MaxDamage ?? 0f;
            EnemyLayers = PlayerHandler.Combat.EnemyLayers;
            HitBox = PlayerHandler.Combat.HitBox;
            StaminaUse = PlayerHandler.Combat.StaminaUse;
            playerInputControl = PlayerHandler.Combat.PlayerInputControl;

            playerInputControl.AttackInput.performed += SetShouldCombo;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            AnimationState = AnimatorController.MainAnimator.GetCurrentAnimatorStateInfo(0);

            Attack();
            CollectGoldFromDeathEnemy();
        }

        public override void OnExit()
        {
            base.OnExit();
            playerInputControl.AttackInput.performed -= SetShouldCombo;
        }

        private void SetShouldCombo(InputAction.CallbackContext context)
        {
            ShouldCombo = true && PlayerHandler.Status.CurrentStamina >= StaminaUse;
        }

        protected void Attack()
        {
            if(IsAttacking() && IsDamaged == false)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D();
                filter.SetLayerMask(EnemyLayers);
                HitBox.OverlapCollider(filter, hitEnemies);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    var maxDamage = MaxDamage + WeaponDamage;
                    var randomDamage = Random.Range(maxDamage * 0.9f, maxDamage);
                    var attackedEnemy = hitEnemy.GetComponent<EnemyStatus>();
                    if(attackedEnemy?.IsImmortal == false)
                    {
                        attackedEnemy?.TakeDamage(randomDamage, HitBox.gameObject);
                        AttackedEnemies.Add(attackedEnemy);
                        IsDamaged = true;
                    }
                }
            }
            else if(IsAttacking() == false)
            {
                IsDamaged = false;
            }

        }

        private bool IsAttacking()
        {
            return AnimationState.IsName($"{AnimationName.Attack}{AttackIndex}");
        }

        private void CollectGoldFromDeathEnemy()
        {
            var deathEnemies = AttackedEnemies.Where(x => x.IsDeath).ToList();
            foreach(var deathEnemy in deathEnemies)
            {
                if(deathEnemy.GetIsCollectedGold() == false)
                {
                    Debug.Log($"Collect gold enemy : {deathEnemy.name}");
                    Gold.Collect(deathEnemy.Type);
                    deathEnemy.SetIsCollectedGold(true);
                    AttackedEnemies.Remove(deathEnemy);
                }

            }
        }
    }
  
}
