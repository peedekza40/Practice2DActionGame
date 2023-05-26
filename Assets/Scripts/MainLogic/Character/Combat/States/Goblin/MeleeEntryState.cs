using Core.Constants;
using UnityEngine;

namespace Character.Combat.States.Goblin
{
    public class MeleeEntryState : State
    {
        private PlayerCombat PlayerCombat;
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            
            PlayerCombat = GameObject.FindWithTag(TagName.Player)?.GetComponent<PlayerCombat>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if(fixedtime >= 1f)
            {
                stateMachine.SetNextState(new Attack1State());  
            }
            else 
            {
                if(PlayerCombat)
                {
                    var playerIsAttack = PlayerCombat.CombatStateMachine.IsCurrentState(typeof(Character.Combat.States.Player.GroundEntryState));
                    if(playerIsAttack)
                    {
                        stateMachine.SetNextState(new Attack2State());  
                    }
                }
            }

        }
    }  
}


