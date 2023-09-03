using UnityEngine;
using Core.Constants;

namespace Character.Animators
{
    public class EnemyAnimatorController : AnimatorController
    {
        public Transform CanvasTransform;

        protected Rigidbody2D Rb;
        public bool IsDeflected { get; private set; }
        private float DurationDeflected = 0.8f;
        private float CurrentTriggerDeflected = 0f;
        protected AnimatorStateInfo AnimationState;

        protected override void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            base.Start();
        }
        
        protected override void Update()
        {
            base.Update();
            AnimationState = MainAnimator.GetCurrentAnimatorStateInfo(0);
            MainAnimator.SetFloat(AnimationParameter.Speed, Mathf.Abs(Rb.velocity.x));

            CurrentTriggerDeflected += Time.deltaTime;
            if(CurrentTriggerDeflected >= DurationDeflected)
            {
                MainAnimator.SetBool(AnimationParameter.IsDeflected, false);
                IsDeflected = false;
            }
        }

        public override void TriggerAttack(int? countAttack)
        {
            MainAnimator.SetTrigger($"{AnimationName.Attack}{countAttack}");
        }

        public virtual void TriggerDeflected()
        {
            MainAnimator.SetBool(AnimationParameter.IsDeflected, true);
            IsDeflected = true;
            CurrentTriggerDeflected = 0f;
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
