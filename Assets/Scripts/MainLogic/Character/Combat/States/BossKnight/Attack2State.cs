using Character.Combat.States;
using UnityEngine;

namespace Character.Combat.States.BossKnight
{
    public class Attack2State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 2;
            Duration = 1.2f;
            MaxDamage = MaxDamage * 0.7f;
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
