namespace Character.Combat.States.Skeleton
{
    public class MeleeEntryState : State
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(fixedtime >= 1f)
            {
                State nextState = (State)new GroundEntryState();
                stateMachine.SetNextState(nextState);
            }
        }
    }  
}


