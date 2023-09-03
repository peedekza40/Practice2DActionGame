using Character.Combat.States;

namespace Character.Combat.States.Goblin
{
    public class Attack1State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 1;
            Duration = EnemyStatus.Attribute.AttackDuration;
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
