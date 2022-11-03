using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core.Constants;

namespace Character.Animators
{
    public class AnimatorController : MonoBehaviour
    {
        public Animator MainAnimator;

        private List<AnimationClip> Clips = new List<AnimationClip>();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Clips = MainAnimator.runtimeAnimatorController.animationClips.ToList();
        }

        public virtual void SetIsAttacking(bool isAttacking)
        {
            MainAnimator.SetBool($"{AnimationName.IsAttacking}", isAttacking);
        }
        
        public virtual void TriggerAttack(int? countAttack){}

        public virtual void TriggerAttacked()
        {
            MainAnimator.SetTrigger(AnimationParameter.Attacked);
        }

        public virtual void SetDeath()
        {
            MainAnimator.SetBool(AnimationParameter.IsDeath, true);
        }

        public virtual void SetBlock(bool isBlocking){}

        public virtual void FilpCharacter(){}

        public virtual void FilpCharacter(Vector2 direction){}

        public virtual float GetAnimationLength(string animName)
        {
            return Clips.FirstOrDefault(x => x.name == animName)?.length ?? 0;
        }
    }
}
