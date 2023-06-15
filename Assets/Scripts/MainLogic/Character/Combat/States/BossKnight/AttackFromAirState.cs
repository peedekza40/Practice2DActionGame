using Character.Animators;
using Character.Combat.States;
using Core.Constants;
using UnityEngine;

namespace Character.Combat.States.BossKnight
{
    public class AttackFromAirState : EnemyMeleeBaseState
    {
        private bool IsTriggered = false;
        private float TimeSinceGrounded = 0f;
        private float DurationOnGrounded = 1.5f;

        private float AngleJump = 30f;
        private float BeyondTarget = 18f;
        private float DistantTrigger = 0.5f;
        private float DropSpeed = 10f;

        private BossKnightAnimatorController BossKnightAnimatorController;

        public override void OnEnter(StateMachine _stateMachine)
        {
            base.OnEnter(_stateMachine);
            Duration = 0.5f;
            BossKnightAnimatorController = GetComponent<BossKnightAnimatorController>();

            if(EnemyAI.IsGrounded)
            {
                Rb.velocity = JumpToTarget(AngleJump, BeyondTarget);
                EnemyAI.SetIsJumping(true);
            }
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var triggerAttack = Mathf.Abs(EnemyAI.Target.position.x - Rb.transform.position.x) <= DistantTrigger;
            if(EnemyAI.IsGrounded == false && IsTriggered == false && triggerAttack)
            {
                IsTriggered = true;
                Rb.velocity = Vector2.down * DropSpeed;
                EnemyAI.gameObject.layer = LayerMask.NameToLayer(LayerName.EnemyNoCollisionPlayer);
                BossKnightAnimatorController.TriggerAttackFromAir();
            }

            if(EnemyAI.IsGrounded && IsTriggered)
            {
                EnemyAI.gameObject.layer = LayerMask.NameToLayer(LayerName.Enemy);
                    
                TimeSinceGrounded += Time.deltaTime;
                if(TimeSinceGrounded >= DurationOnGrounded)
                {
                    stateMachine.SetNextStateToMain();
                }
            }
            else if(EnemyAI.IsGrounded && fixedtime > Duration)
            {
                stateMachine.SetNextStateToMain();
            }
        }

        private Vector2 JumpToTarget(float angle, float beyondTarget)
        {
            var direction = EnemyAI.DirectionToTarget() ;

            var height = direction.y; // get height difference
            direction.y = 0; // retain only the horizontal direction

            var distance = direction.magnitude + beyondTarget; // get horizontal distance
            var radius = angle * Mathf.Deg2Rad;  // convert angle to radians

            direction.y = distance * Mathf.Tan(radius);  // set dir to the elevation angle
            distance += height / Mathf.Tan(radius);  // correct for small height differences

            // calculate the velocity magnitude
            var velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radius));
            return velocity * direction.normalized;
        }

        protected override bool IsAttacking()
        {
            return base.IsAttacking() || IsTriggered;
        }
    }

}
