using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core.Constants;

namespace Character.Animators
{
    public class AnimatorController : MonoBehaviour
    {
        public Animator MainAnimator;

        private float DurationAttacked = 0.1f;
        private float CurrentTriggerAttacked = 0f;
        private float DurationDeath = 0.5f;
        private float CurrentTriggerDeath = 0f;
        private List<AnimationClip> Clips = new List<AnimationClip>();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Clips = MainAnimator.runtimeAnimatorController.animationClips.ToList();
        }

        protected virtual void Update()
        {
            CurrentTriggerAttacked += Time.deltaTime;
            if(CurrentTriggerAttacked >= DurationAttacked)
            {
                MainAnimator.SetBool(AnimationParameter.IsAttacked, false);
            }

            CurrentTriggerDeath += Time.deltaTime;
            if(CurrentTriggerDeath >= DurationDeath)
            {
                MainAnimator.SetBool(AnimationParameter.IsDeath, false);
            }
        }

        public virtual void SetIsAttacking(bool isAttacking)
        {
            MainAnimator.SetBool($"{AnimationName.IsAttacking}", isAttacking);
        }
        
        public virtual void TriggerAttack(int? countAttack){}

        public virtual void TriggerAttacked()
        {
            MainAnimator.SetBool(AnimationParameter.IsAttacked, true);
            CurrentTriggerAttacked = 0f;
        }

        public virtual void SetDeath()
        {
            MainAnimator.SetBool(AnimationParameter.IsDeath, true);
            CurrentTriggerDeath = 0;
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
