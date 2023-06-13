using Character.Behaviours.States;
using Character.Combat.States;
using UnityEngine;

namespace Character.Combat.States.BossKnight
{
    public class Attack4State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 4;
            Duration = 1.5f;
            MaxDamage = MaxDamage * 0.4f;
            MinDamage = MaxDamage * 0.9f;

            //charge
            var direction = EnemyAI.DirectionToTarget();
            Rb.AddForce(direction * EnemyAI.Speed * 1.5f);

            AnimatorController.TriggerAttack(AttackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }

}
