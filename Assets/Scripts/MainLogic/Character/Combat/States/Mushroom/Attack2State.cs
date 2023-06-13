using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Behaviours;
using Character.Behaviours.States;
using Character.Combat.States.Player;
using Character.Status;
using Constants;
using UnityEngine;

namespace Character.Combat.States.Mushroom
{
    public class Attack2State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 2;
            Duration = EnemyAI.AttackDuration;
            AnimatorController.TriggerAttack(AttackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                stateMachine.SetNextState(new Attack1State());
            }
        }

        protected override void Attack()
        {
            if(IsAttacking() && IsDamaged == false)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = DetectEnemy();

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    var randomDamage = Random.Range(MaxDamage * 0.9f, MaxDamage);
                    var combatStateMachine = hitEnemy.GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat);
                    var behaviorStateMachine = hitEnemy.GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Behaviour);
                    var knockBack = hitEnemy.GetComponent<KnockBack>();
                    var attackedEnemy = hitEnemy.GetComponent<PlayerStatus>();

                    if(attackedEnemy?.IsImmortal == false)
                    {
                        if(knockBack != null)
                        {
                            knockBack.Enabled = false;
                        }

                        var enemyIsBlocking = combatStateMachine?.IsCurrentState(typeof(BlockingState)) ?? false;
                        if(enemyIsBlocking == false)
                        {
                            behaviorStateMachine?.SetNextState(new DisabledMoveState(1.5f));
                        }
                        attackedEnemy?.TakeDamage(randomDamage, HitBoxesIsDetected.FirstOrDefault()?.gameObject);
                        IsDamaged = true;

                        if(knockBack != null)
                        {
                            knockBack.Enabled = true;
                        }
                    }
                }
            }
            else if(IsAttacking() == false)
            {
                IsDamaged = false;
            }
        }
    }
}
