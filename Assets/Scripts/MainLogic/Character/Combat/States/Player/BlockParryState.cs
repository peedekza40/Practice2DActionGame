using UnityEngine;

namespace Character.Combat.States.Player
{
    public class BlockParryState : BlockingState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            Duration = PlayerHandler.Attribute.ParryDurtation;
            AnimatorController.SetBlock(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (fixedtime >= Duration)
            {
                stateMachine.SetNextState(new BlockingState());
            }
        }
    }
}


