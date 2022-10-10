using UnityEngine;
using UnityEngine.Events;
using System;

namespace Character
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public float CurrentHP { get; protected set; }
        public HealthBar HealthBar;
        public UnityEvent OnDamaged;
        public UnityEvent<GameObject> OnDamagedPassHitBox;
        public UnityEvent<float> OnDamagedPassDamage;
        public UnityEvent OnDied;
        public bool IsDeath { get; private set; }

        private IAnimatorController AnimatorController;
        private Rigidbody2D Rigidbody;

        protected void BaseAwake()
        {
            AnimatorController = GetComponent<IAnimatorController>();
            Rigidbody = GetComponent<Rigidbody2D>();
            HealthBar.SetMaxHealth(MaxHP);
            SetCurrentHP(MaxHP);
        }

        protected void BaseUpdate()
        {
            if(CurrentHP <= 0)
            {
                IsDeath = true;
                Die();
            }
        }

        public void SetCurrentHP(float hp)
        {
            this.CurrentHP = hp;
            HealthBar.SetHealth(CurrentHP);
        }

        public void AddCurrentHP(float hp)
        {
            this.CurrentHP += hp;
            HealthBar.SetHealth(CurrentHP);
        }

        public virtual void TakeDamage(float damage, GameObject attackerHitBox)
        {
            //set hp
            SetCurrentHP(CurrentHP - damage);
            AnimatorController.TriggerAttacked();
            
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
