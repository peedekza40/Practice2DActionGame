using Character.Animators;

namespace Character.Combat.States.Player
{
    public class BlockFinisherState : State
    {
        private float Duration;
        protected PlayerHandler PlayerHandler;
        protected AnimatorController AnimatorController;


        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            PlayerHandler = GetComponent<PlayerHandler>();
            
            Duration = PlayerHandler.Attribute.TimeBetweenBlock;
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


