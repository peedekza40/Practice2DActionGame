using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Animators;
using Character.Status;
using Core.Constants;
using UnityEngine;

namespace Character.Combat.States
{
    public class EnemyMeleeBaseState : State
    {
        public float Duration;
        protected int AttackIndex;
        protected EnemyAI EnemyAI;
        protected EnemyStatus EnemyStatus;
        protected AnimatorController AnimatorController;
        private AnimatorStateInfo AnimationState;
        public Rigidbody2D Rb;
        public List<Collider2D> DetectedEnemies;

        #region Calculate attack
        protected float MaxDamage;
        protected LayerMask EnemyLayers;
        protected Collider2D HitBox;
        protected bool IsDamaged;
        #endregion


        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            EnemyAI = GetComponent<EnemyAI>();
            EnemyStatus = GetComponent<EnemyStatus>();
            Rb = GetComponent<Rigidbody2D>();
            DetectedEnemies = EnemyAI.DetectedEnemies;
            MaxDamage = EnemyAI.MaxDamage;
            EnemyLayers = EnemyAI.EnemyLayers;
            HitBox = EnemyAI.HitBox;

            //look direction
            var target = DetectedEnemies.FirstOrDefault(x => x.tag == TagName.Player);
            if(target != null)
            {
                Vector2 direction = ((Vector2)target.transform.position - Rb.position).normalized;
                AnimatorController.FilpCharacter(direction);
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            AnimationState = AnimatorController.MainAnimator.GetCurrentAnimatorStateInfo(0);

            Attack();
            AnimatorController.SetIsAttacking(IsAttacking());
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
                    var randomDamage = Random.Range(MaxDamage * 0.9f, MaxDamage);
                    var attackedEnemy = hitEnemy.GetComponent<PlayerStatus>();
                    if(attackedEnemy?.IsImmortal == false)
                    {
                        attackedEnemy?.TakeDamage(randomDamage, HitBox.gameObject);
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
    }
}
