using Core.Constants;
using UnityEngine;

namespace Character.Combat.States.Mushroom
{
    public class MeleeEntryState : State
    {
        public override void OnUpdate()
        {
            base.OnUpdate();

            if(fixedtime >= 1f)
            {
                if (Random.Range(0, 2) == 1)
                {
                    stateMachine.SetNextState(new Attack1State());
                }
                else
                {
                    stateMachine.SetNextState(new Attack2State());
                }
            }

        }
    }  
}


