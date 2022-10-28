using UnityEngine;
using Core.Constants;
using Character.Interfaces;

namespace Character.Animators
{
    public class PlayerAnimatorController : AnimatorController
    {
        public IPlayerController PlayerController;

        private FrameInput Input;
        private bool IsFalling;
        
        // Start is called before the first frame update
        void Start()
        {
            PlayerController = GetComponent<IPlayerController>();
            base.BaseStart();
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

            Animator.SetFloat(AnimationParameter.Speed, Mathf.Abs(PlayerController.RawMovement.x));
            Animator.SetBool(AnimationParameter.IsJumping, PlayerController.JumpingThisFrame);
            Animator.SetBool(AnimationParameter.IsFalling, IsFalling);
            Animator.SetBool(AnimationParameter.IsGrounded, PlayerController.Grounded);
        }

        public override void TriggerAttack(int? countAttack)
        {
            Animator.SetTrigger($"{AnimationParameter.Attack}{countAttack}");
        }

        public override void SetBlock(bool isBlocking)
        {
            Animator.SetBool(AnimationParameter.IsBlocking, isBlocking);
        }
    }
}
