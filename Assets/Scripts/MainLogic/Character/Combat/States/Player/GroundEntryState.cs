namespace Character.Combat.States.Player
{
    public class GroundEntryState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            PlayerHandler.Status.SetCurrentStamina(PlayerHandler.Status.CurrentStamina - StaminaUse);

            //Attack
            AttackIndex = 1;
            Duration = PlayerHandler.Attribute.AttackDuration;
            AnimatorController.TriggerAttack(AttackIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (fixedtime >= Duration)
            {
                if (ShouldCombo)
                {
                    stateMachine.SetNextState(new GroundComboState());
                }
                else
                {
                    stateMachine.SetNextStateToMain();
                }
            }
        }
    }
}


