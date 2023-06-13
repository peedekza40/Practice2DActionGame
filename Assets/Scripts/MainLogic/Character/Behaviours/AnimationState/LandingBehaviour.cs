using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Behaviours.States;
using Constants;
using UnityEngine;

namespace Character.Behaviours.AnimationState 
{
    public class LandingBehaviour : StateMachineBehaviour
    {
        private StateMachine BehaviorStateMachine;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            BehaviorStateMachine = animator.GetComponentsInParent<StateMachine>()?.FirstOrDefault(x => x.Id == StateId.Behaviour);
            if(BehaviorStateMachine != null)
            {
                BehaviorStateMachine.SetNextState(new DisabledMoveState());
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(BehaviorStateMachine != null)
            {
                BehaviorStateMachine.SetNextStateToMain();
            }
        }
    }

}
