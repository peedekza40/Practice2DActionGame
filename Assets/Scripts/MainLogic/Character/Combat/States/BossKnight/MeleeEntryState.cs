using Core.Constants;
using UnityEngine;

namespace Character.Combat.States.BossKnight
{
    public class MeleeEntryState : MovableCombatState
    {
        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            var enemyAI = GetComponent<EnemyAI>();

            if(enemyAI.DistanceFromTarget() >= 2)
            {
                switch (Random.Range(0, 3))
                {
                    case 1:
                        _stateMachine.SetNextState(new Attack4State());
                        break;
                    case 2:
                        _stateMachine.SetNextState(new AttackFromAirState());
                        break;
                }
            }
            else
            {
                _stateMachine.SetNextState(new Attack1State());
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(fixedtime >= 1.5f)
            {
                stateMachine.SetNextStateToMain();
            }
        }

    }  
}


