using UnityEngine;
using UnityEngine.Events;
using System;

namespace Character
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public float CurrentHP;
        public HealthBar HealthBar;
        public UnityEvent OnDamaged;
        public UnityEvent<GameObject> OnDamagedPassHitBox;
        public UnityEvent<float> OnDamagedPassDamage;
        public UnityEvent OnDied;

        private IAnimatorController AnimatorController;
        private Rigidbody2D Rigidbody;
        private bool IsDeath = false;

        protected void BaseStart()
        {
            AnimatorController = GetComponent<IAnimatorController>();
            Rigidbody = GetComponent<Rigidbody2D>();

            CurrentHP = MaxHP;
            HealthBar.SetMaxHealth(MaxHP);
        }

        protected void BaseUpdate()
        {
            if(CurrentHP <= 0)
            {
                IsDeath = true;
                Die();
            }
        }

        public virtual void TakeDamage(float damage, GameObject attackerHitBox)
        {
            //set hp
            CurrentHP -= damage;
            AnimatorController.TriggerAttacked();
            HealthBar.SetHealth(CurrentHP);
            
            OnDamagedPassDamage?.Invoke(damage);
            OnDamagedPassHitBox?.Invoke(attackerHitBox);
            OnDamaged?.Invoke();
        }

        public virtual void Die()
        {
            this.enabled = false;
            Rigidbody.simulated = false;
            AnimatorController.SetDeath();

            OnDied?.Invoke();
        }
    }
}
