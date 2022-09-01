using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;
using Constants;

namespace Character 
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public float CurrentHP;
        public HealthBar HealthBar;
        public UnityEvent OnDamaged, OnDied;

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

        public virtual void TakeDamage(float damage)
        {
            //set hp
            CurrentHP -= damage;
            AnimatorController.TriggerAttacked();
            HealthBar.SetHealth(CurrentHP);
            
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
