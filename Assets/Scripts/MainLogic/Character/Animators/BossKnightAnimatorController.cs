using System.Collections;
using System.Collections.Generic;
using Character;
using Character.Animators;
using Core.Constants;
using UnityEngine;

namespace Character.Animators
{
    public class BossKnightAnimatorController : AnimatorController
    {
        public Transform CanvasTransform;

        private Rigidbody2D Rb;
        private EnemyAI EnemyAI;

        protected override void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            EnemyAI = GetComponent<EnemyAI>();
            base.Start();
        }
        
        protected override void Update()
        {
            base.Update();
            MainAnimator.SetFloat(AnimationParameter.Speed, Mathf.Abs(Rb.velocity.x));
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

        public override void FilpCharacter()
        {
            if(Rb.velocity.x > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
            else if (Rb.velocity.x < -0.01f)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(-1 * Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
        }

        public override void FilpCharacter(Vector2 direction)
        {
            if(direction.x > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
            else if (direction.x < -0.01f)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(-1 * Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
        }

    }
}
