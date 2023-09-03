using System.Collections;
using System.Linq;
using Character.Behaviours.States;
using Constants;
using UnityEngine;

namespace Character.Behaviours
{
    public class KnockBack : MonoBehaviour
    {
        public float KnockBackForce = 3f;

        [Range(0, 100)]
        public float ReduceForcePercent;

        public bool Enabled = true;

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
            if(Enabled)
            {
                BehaviourStateMachine.SetNextState(new KnockBackState(attackerHitBox));
            }
        }
        
        public void CalculateKnockBackForce(float damage)
        {
            KnockBackForce = damage * 0.3f * ((100 - ReduceForcePercent) / 100);
        }
    }

}
