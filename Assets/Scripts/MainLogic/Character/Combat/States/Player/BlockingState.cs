using Character.Animators;
using Character.Interfaces;

namespace Character.Combat.States.Player
{
    public class BlockingState : State
    {
        protected float Duration;
        protected AnimatorController AnimatorController;
        protected PlayerHandler PlayerHandler;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            PlayerHandler = GetComponent<PlayerHandler>();
        }
    }
}


