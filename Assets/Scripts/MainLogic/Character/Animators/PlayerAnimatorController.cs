using UnityEngine;
using Core.Constants;
using Character.Interfaces;

namespace Character.Animators
{
    public class PlayerAnimatorController : AnimatorController
    {
        public Animator RightHandFingerAnimator;
        public IPlayerController PlayerController;

        private FrameInput Input;
        private bool IsFalling;
        
        // Start is called before the first frame update
        protected override void Start()
        {
            PlayerController = GetComponent<IPlayerController>();
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            Input = PlayerController.Input;

            if(Input.X != 0)
            {
                transform.localScale = new Vector3(Input.X, transform.localScale.y, transform.localScale.z);
            }

            IsFalling = PlayerController.Velocity.y < 0;

            MainAnimator.SetFloat(AnimationParameter.Speed, Mathf.Abs(PlayerController.RawMovement.x));
            MainAnimator.SetBool(AnimationParameter.IsJumping, PlayerController.JumpingThisFrame);
            MainAnimator.SetBool(AnimationParameter.IsFalling, IsFalling);
            MainAnimator.SetBool(AnimationParameter.IsGrounded, PlayerController.Grounded);
            RightHandFingerAnimator.SetFloat(AnimationParameter.Speed, Mathf.Abs(PlayerController.RawMovement.x));
            RightHandFingerAnimator.SetBool(AnimationParameter.IsJumping, PlayerController.JumpingThisFrame);
            RightHandFingerAnimator.SetBool(AnimationParameter.IsFalling, IsFalling);
            RightHandFingerAnimator.SetBool(AnimationParameter.IsGrounded, PlayerController.Grounded);
        }

        public override void SetIsAttacking(bool isAttacking)
        {
            base.SetIsAttacking(isAttacking);
            RightHandFingerAnimator.SetBool($"{AnimationName.IsAttacking}", isAttacking);
        }

        public override void TriggerAttack(int? countAttack)
        {
            MainAnimator.SetTrigger($"{AnimationParameter.Attack}{countAttack}");
            RightHandFingerAnimator.SetTrigger($"{AnimationParameter.Attack}{countAttack}");
        }

        public override void TriggerAttacked()
        {
            base.TriggerAttacked();
            RightHandFingerAnimator.SetTrigger(AnimationParameter.Attacked);
        }

        public override void SetDeath()
        {
            base.SetDeath();
            RightHandFingerAnimator.SetBool(AnimationParameter.IsDeath, true);
        }

        public override void SetBlock(bool isBlocking)
        {
            MainAnimator.SetBool(AnimationParameter.IsBlocking, isBlocking);
            RightHandFingerAnimator.SetBool(AnimationParameter.IsBlocking, isBlocking);
        }
    }
}
