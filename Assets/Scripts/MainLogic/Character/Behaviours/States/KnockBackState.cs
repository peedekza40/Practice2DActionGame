using UnityEngine;

namespace Character.Behaviours.States
{
    public class KnockBackState : State
    {
        private float Force;
        private float Duration;
        private Transform CenterTransform;
        private Rigidbody2D Rb;
        private GameObject AttackerHitBox;

        public KnockBackState(GameObject attackerHitBox)
        {
            AttackerHitBox = attackerHitBox;
        }

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            Rb = GetComponent<Rigidbody2D>();
            Force = GetComponent<KnockBack>().KnockBackForce;
            CenterTransform = GetComponent<KnockBack>().CenterTransform;

            KnockBack();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if(fixedtime >= Duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }

        private void KnockBack()
        {

            var direction = (Vector3)CenterTransform.position - AttackerHitBox.transform.position;
            if(direction.x < 0)
            {
                direction.x = -1;
            }
            else if(direction.x > 0)
            {
                direction.x = 1;
            }
            
            var velocity = (Vector2)(direction.normalized * Force);
            velocity.y = 0;
            Rb.velocity = velocity;

            var distance = Force - 1.8f; //estimate distance relate with force
            Duration = (distance / (Rb.velocity.magnitude * 0.7f));
            Duration = float.IsNaN(Duration) ? 0 : Duration;
        }
    }
}


