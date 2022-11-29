namespace Character.Combat.States.Player
{
    public class GroundComboState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            PlayerHandler.Status.SetCurrentStamina(PlayerHandler.Status.CurrentStamina - StaminaUse);

            //Attack
            AttackIndex = 2;
            Duration = PlayerHandler.Combat.AttackDuration;
            AnimatorController.TriggerAttack(AttackIndex);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                if (ShouldCombo)
                {
                    stateMachine.SetNextState(new GroundFinisherState());
                }
                else
                {
                    stateMachine.SetNextStateToMain();
                }
            }
        }
    }
}

