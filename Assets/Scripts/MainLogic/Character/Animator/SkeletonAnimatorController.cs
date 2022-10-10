using UnityEngine;
using Core.Constants;

namespace Character
{
    public class SkeletonAnimatorController : AnimatorController
    {
        public Transform CanvasTransform;

        private Rigidbody2D Rb;

        void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            base.BaseStart();
        }
        
        void Update()
        {
            Animator.SetFloat(AnimationParameter.Speed, Mathf.Abs(Rb.velocity.x));
        }

        public override void TriggerAttack(int? countAttack)
        {
            Animator.SetTrigger($"{AnimationName.Attack}{countAttack}");
        }

        public override void FilpCharacter()
        {
            if(Rb.velocity.x > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(-1 * Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
            else if (Rb.velocity.x < -0.01f)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
        }

        public override void FilpCharacter(Vector2 direction)
        {
            if(direction.x > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(-1 * Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
            else if (direction.x < -0.01f)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CanvasTransform.localScale = new Vector3(Mathf.Abs(CanvasTransform.localScale.x), CanvasTransform.localScale.y, CanvasTransform.localScale.z);
            }
        }

    }
}
