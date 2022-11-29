namespace Character.Combat.States.Player
{
    public class GroundFinisherState : MeleeBaseState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            PlayerHandler.Status.SetCurrentStamina(PlayerHandler.Status.CurrentStamina - StaminaUse);

            //Attack
            AttackIndex = 3;
            Duration = PlayerHandler.Combat.AttackDuration + PlayerHandler.Combat.TimeBetweenCombo; // duration + time betweem combo
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

