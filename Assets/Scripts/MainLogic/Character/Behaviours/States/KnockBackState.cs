using UnityEngine;

namespace Character.Behaviours.States
{
    public class KnockBackState : State
    {
        private float Range;
        private float Duration;
        private float ReduceDurationPercent;
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
            Range = GetComponent<KnockBack>().KnockBackRange;
            ReduceDurationPercent = GetComponent<KnockBack>().ReduceDurationPercent;
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

            var velocity = (Vector2)(direction.normalized * Range);
            Rb.velocity = velocity;
            Duration = (Range / Mathf.Abs(velocity.x)) * (100 - ReduceDurationPercent) / 100;
            Duration = float.IsNaN(Duration) ? 0 : Duration;
        }
    }
}


