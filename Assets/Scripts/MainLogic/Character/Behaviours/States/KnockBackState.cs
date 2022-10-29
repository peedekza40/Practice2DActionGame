using UnityEngine;

namespace Character.Behaviours.States
{
    public class KnockBackState : State
    {
        private float Range;
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
            Range = GetComponent<KnockBack>().KnockBackRange;
            Duration = GetComponent<KnockBack>().KnockBackDuration;
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
            Rb.velocity = (Vector2)(direction.normalized * Range);
        }
    }
}


