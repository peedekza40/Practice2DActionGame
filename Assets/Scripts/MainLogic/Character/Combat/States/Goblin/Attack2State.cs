using Character.Combat.States;

namespace Character.Combat.States.Goblin
{
    public class Attack2State : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 2;
            Duration = EnemyAI.AttackDuration;
            EnemyStatus.IsImmortal = true;
            AnimatorController.TriggerAttack(AttackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                EnemyStatus.IsImmortal = false;
                stateMachine.SetNextState(new Attack1State());
            }
        }
    }
}
