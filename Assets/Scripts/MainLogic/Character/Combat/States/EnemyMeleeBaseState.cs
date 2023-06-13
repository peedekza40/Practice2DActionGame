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
        protected AnimatorStateInfo AnimationState;
        public Rigidbody2D Rb;
        public List<Collider2D> DetectedEnemies;

        #region Calculate attack
        protected float MaxDamage;
        protected float MinDamage;
        protected LayerMask EnemyLayers;
        protected List<Collider2D> HitBoxes = new List<Collider2D>();
        protected List<Collider2D> HitBoxesIsDetected = new List<Collider2D>();
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
            MinDamage = EnemyAI.MinDamage;
            EnemyLayers = EnemyAI.EnemyLayers;
            HitBoxes = EnemyAI.HitBoxes;

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

        protected virtual void Attack()
        {
            if(IsAttacking() && IsDamaged == false)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = DetectEnemy();

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    var randomDamage = Random.Range(MinDamage, MaxDamage);
                    var attackedEnemy = hitEnemy.GetComponent<PlayerStatus>();
                    if(attackedEnemy?.IsImmortal == false)
                    {
                        attackedEnemy?.TakeDamage(randomDamage, HitBoxesIsDetected.FirstOrDefault()?.gameObject);
                        IsDamaged = true;
                    }
                }
            }
            else if(IsAttacking() == false)
            {
                IsDamaged = false;
            }
        }

        protected List<Collider2D> DetectEnemy()
        {
            //detect enemy in range of attack
            List<Collider2D> hitEnemies = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(EnemyLayers);
            foreach(var hitBox in HitBoxes)
            {
                var tempHitEnemies = new List<Collider2D>();
                hitBox.OverlapCollider(filter, tempHitEnemies);

                if(tempHitEnemies.Any())
                {
                    HitBoxesIsDetected.Add(hitBox);
                }

                hitEnemies.AddRange(tempHitEnemies);
            }
            return hitEnemies;
        }

        protected virtual bool IsAttacking()
        {
            return AnimationState.IsName($"{AnimationName.Attack}{AttackIndex}");
        }
    }
}
