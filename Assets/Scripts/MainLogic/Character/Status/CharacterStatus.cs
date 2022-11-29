using UnityEngine;
using UnityEngine.Events;
using Character.Animators;
using UI;

namespace Character.Status
{
    public class CharacterStatus : MonoBehaviour
    {

        public float MaxHP = 100;
        public float CurrentHP { get; protected set; }
        public SliderBar HealthBar;
        public UnityEvent OnDamaged;
        public UnityEvent<float> OnDamagedPassDamage;
        public UnityEvent<GameObject> OnDamagedPassHitBox;
        public UnityEvent OnDied;
        public bool IsDeath { get; private set; }

        private AnimatorController AnimatorController;
        private Rigidbody2D Rigidbody;

        protected virtual void Awake()
        {
            AnimatorController = GetComponent<AnimatorController>();
            Rigidbody = GetComponent<Rigidbody2D>();
            
            HealthBar.SetMaxValue(MaxHP);
            SetCurrentHP(MaxHP);
        }

        protected virtual void Update()
        {
            if(CurrentHP <= 0)
            {
                IsDeath = true;
                Die();
            }
        }

        public void SetCurrentHP(float hp)
        {
            CurrentHP = hp;
            HealthBar.SetCurrentValue(CurrentHP);
        }

        public void SetMaxHealth(float maxHp)
        {
            MaxHP = maxHp;
            HealthBar.SetMaxValue(MaxHP);
        }

        public void AddCurrentHP(float hp)
        {
            CurrentHP += hp;
            HealthBar.SetCurrentValue(CurrentHP);
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
