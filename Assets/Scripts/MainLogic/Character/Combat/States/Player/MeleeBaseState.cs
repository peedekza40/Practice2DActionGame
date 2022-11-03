using System.Collections.Generic;
using System.Linq;
using Character.Animators;
using Character.Interfaces;
using Collecting;
using Core.Constants;
using Infrastructure.InputSystem;
using UnityEngine;

namespace Character.Combat.States.Player
{
    public class MeleeBaseState : State
    {
        public float Duration;
        protected bool ShouldCombo;
        protected int AttackIndex;
        protected PlayerCombat PlayerCombat;
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
        #endregion

        #region Dependencies
        protected PlayerInputControl playerInputControl;
        #endregion 

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            Gold = GetComponent<Gold>();
            PlayerCombat = GetComponent<PlayerCombat>();
            MaxDamage = PlayerCombat.MaxDamage;
            WeaponDamage = PlayerCombat.CurrentWeapon?.MaxDamage ?? 0f;
            EnemyLayers = PlayerCombat.EnemyLayers;
            HitBox = PlayerCombat.HitBox;
            playerInputControl = PlayerCombat.PlayerInputControl;

            playerInputControl.AttackInput.performed += (context) => { SetShouldCombo(); };
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            AnimationState = AnimatorController.MainAnimator.GetCurrentAnimatorStateInfo(0);

            Attack();
            CollectGoldFromDeathEnemy();
        }

        private void SetShouldCombo()
        {
            ShouldCombo = true;
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
                    attackedEnemy?.TakeDamage(randomDamage, HitBox.gameObject);
                    AttackedEnemies.Add(attackedEnemy);
                    IsDamaged = true;
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
