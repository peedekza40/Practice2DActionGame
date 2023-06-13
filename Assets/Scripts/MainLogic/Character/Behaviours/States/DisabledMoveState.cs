using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Behaviours.States
{
    public class DisabledMoveState : State
    {
        public float? Duration;
        public DisabledMoveState(float duration)
        {
            this.Duration = duration;
        }

        public DisabledMoveState()
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(Duration != null && fixedtime >= Duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }
    }
}
