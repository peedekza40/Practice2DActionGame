using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class HealthBar : MonoBehaviour
    {
        public Slider HpSlider;
        public Slider SmoothSlider;
        
        private float TimeSinceSetHealth = 0f;
        private float CurrentVelocity = 0;

        private void Update() 
        {
            if(TimeSinceSetHealth >= 1)
            {
                float runningHp = Mathf.SmoothDamp(SmoothSlider.value, HpSlider.value, ref CurrentVelocity, 80 * Time.deltaTime);        
                SmoothSlider.value = runningHp;
            }

            TimeSinceSetHealth += Time.deltaTime;
        }
        
        public void SetMaxHealth(float maxHealth)
        {
            HpSlider.maxValue = maxHealth;
            SmoothSlider.maxValue = maxHealth;
        }

        public void SetHealth(float health)
        {
            HpSlider.value = health;
            TimeSinceSetHealth = 0;
        }
    }

}
