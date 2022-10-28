public class BlockingState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        AnimatorController.SetBlock(true);
    }
}
