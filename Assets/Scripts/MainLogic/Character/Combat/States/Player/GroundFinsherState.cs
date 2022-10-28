using Infrastructure.InputSystem;

public class GroundFinsherState : MeleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //Attack
        AttackIndex = 3;
        Duration = PlayerCombat.AttackDuration + PlayerCombat.TimeBetweenCombo; // duration + time betweem combo
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
