using Character.Animators;
using Character.Interfaces;

namespace Character.Combat.States.Player
{
    public class BlockingState : State
    {
        protected float Duration;
        protected IAnimatorController AnimatorController;
        protected PlayerCombat PlayerCombat;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            AnimatorController = GetComponent<AnimatorController>();
            PlayerCombat = GetComponent<PlayerCombat>();
        }
    }
}


