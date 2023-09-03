using System.Collections;
using System.Collections.Generic;
using Character;
using Character.Animators;
using Core.Constants;
using UnityEngine;

namespace Character.Animators
{
    public class BossKnightAnimatorController : EnemyAnimatorController
    {
        private EnemyAI EnemyAI;

        protected override void Start()
        {
            EnemyAI = GetComponent<EnemyAI>();
            base.Start();
        }
        
        protected override void Update()
        {
            base.Update();
            MainAnimator.SetBool(AnimationParameter.IsJumping, EnemyAI.IsJumping);
            MainAnimator.SetBool(AnimationParameter.IsGrounded, EnemyAI.IsGrounded);
        }

        public void TriggerAttackFromAir()
        {
            MainAnimator.SetTrigger(AnimationParameter.AttackFromAir);
        }

        public override void TriggerAttack(int? countAttack)
        {
            MainAnimator.SetTrigger($"{AnimationName.Attack}{countAttack}");
        }

    }
}
