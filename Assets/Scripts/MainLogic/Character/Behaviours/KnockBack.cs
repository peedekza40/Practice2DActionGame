using System.Collections;
using System.Linq;
using Character.Behaviours.States;
using Constants;
using UnityEngine;

namespace Character.Behaviours
{
    public class KnockBack : MonoBehaviour
    {
        public float KnockBackRange;
        public float KnockBackDuration = 1f;
        public Transform CenterTransform;

        private Rigidbody2D Rb;
        private StateMachine BehaviourStateMachine;

        private void Start() 
        {
            Rb = GetComponent<Rigidbody2D>();
            BehaviourStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Behaviour);
        }
        
        public void Action(GameObject attackerHitBox)
        {
            BehaviourStateMachine.SetNextState(new KnockBackState(attackerHitBox));
        }
        
        public void CalculateKnockBackRange(float damage)
        {
            KnockBackRange = damage * (3f/10f);
        }
    }

}
