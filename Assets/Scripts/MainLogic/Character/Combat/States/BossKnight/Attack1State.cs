using Character.Combat.States;
using UnityEngine;

namespace Character.Combat.States.BossKnight
{
    public class Attack1State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 1;
            Duration = 1.2f;
            MaxDamage = MaxDamage * 0.2f;
            MinDamage = MaxDamage * 0.9f;
            AnimatorController.TriggerAttack(AttackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                if(Random.Range(0, 2) == 1)
                {
                    stateMachine.SetNextState(new Attack2State());
                }
                else
                {
                    stateMachine.SetNextState(new Attack3State());
                }
            }
        }
    }

}
