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
                    var randNum = Random.Range(0, 10);
                    var playerIsAttack = PlayerCombat.CombatStateMachine.IsCurrentState(typeof(Player.MeleeBaseState));
                    if(playerIsAttack && randNum == 1)
                    {
                        stateMachine.SetNextState(new Attack2State());  
                    }
                }
            }

        }
    }  
}


