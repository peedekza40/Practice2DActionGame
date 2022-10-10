using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Constants;

namespace Character 
{
    public class AnimatorController : MonoBehaviour, IAnimatorController
    {
        public Animator Animator { get; set; }

        private List<AnimationClip> Clips = new List<AnimationClip>();

        // Start is called before the first frame update
        protected void BaseStart()
        {
            Animator = GetComponentInChildren<Animator>();
            Clips = Animator.runtimeAnimatorController.animationClips.ToList();
        }
        
        public virtual void TriggerAttack(int? countAttack){}

        public virtual void TriggerAttacked()
        {
            Animator.SetTrigger(AnimationParameter.Attacked);
        }

        public virtual void SetDeath()
        {
            Animator.SetBool(AnimationParameter.IsDeath, true);
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
