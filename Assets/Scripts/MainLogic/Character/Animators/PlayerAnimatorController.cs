using UnityEngine;
using Core.Constants;
using Character.Interfaces;
using UnityEngine.U2D.Animation;
using Infrastructure.Entities;

namespace Character.Animators
{
    public class PlayerAnimatorController : AnimatorController
    {
        public SpriteLibrary MainSpriteLibrary;
        public SpriteRenderer RightFingerSprite;

        private IPlayerController PlayerController;
        private PlayerHandler PlayerHandler;

        private FrameInput Input;
        private bool IsFalling;
        private const string HeroSpritePath = "Sprites/Characters/Hero";

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
        }

        public void SetAssetsAnimation(bool isHasWeapon, string bootSpritePath)
        {
            SpriteLibraryAsset bareBodySprite;
            SpriteLibraryAsset defaultArmorSprite;
            SpriteLibraryAsset defaultRightGloveSprite;
            SpriteLibraryAsset bootSprite = Resources.Load<SpriteLibraryAsset>(bootSpritePath) ?? Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/Boots/DefaultBoot");
            if(isHasWeapon)
            {
                RightFingerSprite.enabled = true;
                bareBodySprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/BareBody/HaveWeapon/BareBody");
                defaultArmorSprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/Armors/Default/HaveWeapon/DefaultArmor");
                defaultRightGloveSprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/RightGloves/Default/HaveWeapon/DefaultRightGlove");
            }
            else
            {
                RightFingerSprite.enabled = false;
                bareBodySprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/BareBody/NoWeapon/NwpBareBody");
                defaultArmorSprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/Armors/Default/NoWeapon/NwpDefaultArmor");
                defaultRightGloveSprite = Resources.Load<SpriteLibraryAsset>($"{HeroSpritePath}/RightGloves/Default/NoWeapon/NwpDefaultRightGlove");
            }

            MainSpriteLibrary.spriteLibraryAsset = bareBodySprite;
            MainSpriteLibrary.transform.Find(GameObjectName.Armor).GetComponent<SpriteLibrary>().spriteLibraryAsset = defaultArmorSprite;
            MainSpriteLibrary.transform.Find(GameObjectName.RightGlove).GetComponent<SpriteLibrary>().spriteLibraryAsset = defaultRightGloveSprite;
            MainSpriteLibrary.transform.Find(GameObjectName.Boot).GetComponent<SpriteLibrary>().spriteLibraryAsset = bootSprite;
        }

        public override void TriggerAttack(int? countAttack)
        {
            MainAnimator.SetTrigger($"{AnimationParameter.Attack}{countAttack}");
        }
        
        public override void SetBlock(bool isBlocking)
        {
            MainAnimator.SetBool(AnimationParameter.IsBlocking, isBlocking);
            if(isBlocking)
            {
                MainAnimator.SetTrigger(AnimationParameter.Block);
            }
        }
    }
}
