namespace Character.Combat.States.Player
{
    public class BlockFinisherState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            Duration = PlayerHandler.Combat.TimeBetweenBlock;
            AnimatorController.SetBlock(false);
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


