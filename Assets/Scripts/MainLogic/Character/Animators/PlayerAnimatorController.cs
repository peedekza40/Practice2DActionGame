using UnityEngine;
using Core.Constants;
using Character.Interfaces;

namespace Character.Animators
{
    public class PlayerAnimatorController : AnimatorController
    {
        public SpriteRenderer RightHandFingerSprite;

        private IPlayerController PlayerController;
        private PlayerHandler PlayerHandler;

        private FrameInput Input;
        private bool IsFalling;
        
        // Start is called before the first frame update
        protected override void Start()
        {
            PlayerController = GetComponent<IPlayerController>();
            PlayerHandler = GetComponent<PlayerHandler>();
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

            if(PlayerHandler.Combat.CurrentWeapon != null)
            {
                MainAnimator.SetFloat("IsHasWeapon", 1);
                RightHandFingerSprite.enabled = true;
            }
            else
            {
                MainAnimator.SetFloat("IsHasWeapon", 0);
                RightHandFingerSprite.enabled = false;
            }
        }
        public override void TriggerAttack(int? countAttack)
        {
            MainAnimator.SetTrigger($"{AnimationParameter.Attack}{countAttack}");
        }
        public override void SetBlock(bool isBlocking)
        {
            MainAnimator.SetBool(AnimationParameter.IsBlocking, isBlocking);
        }
    }
}
