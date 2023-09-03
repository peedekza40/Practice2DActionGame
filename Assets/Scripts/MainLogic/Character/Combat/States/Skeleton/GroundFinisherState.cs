namespace Character.Combat.States.Skeleton
{
    public class GroundFinisherState : EnemyMeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);

            //Attack
            AttackIndex = 2;
            Duration = EnemyStatus.Attribute.AttackDuration + EnemyStatus.Attribute.TimeBetweenCombo; // duration + time betweem combo
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

