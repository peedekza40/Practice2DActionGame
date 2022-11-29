using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderBar : MonoBehaviour
    {
        public Slider MainSlider;
        public Slider BackSlider;
        
        public float TimeSinceSetValue { get; private set; } = 0f;
        private float CurrentVelocity = 0;

        private void Update() 
        {
            //back silder smooth
            if(TimeSinceSetValue >= 1)
            {
                float runningValue = Mathf.SmoothDamp(BackSlider.value, MainSlider.value, ref CurrentVelocity, 80 * Time.deltaTime);        
                BackSlider.value = runningValue;
            }

            TimeSinceSetValue += Time.deltaTime;
        }
        
        public void SetMaxValue(float maxValue)
        {
            MainSlider.maxValue = maxValue;
            BackSlider.maxValue = maxValue;
        }

        public void SetCurrentValue(float value, bool resetTime = true)
        {
            MainSlider.value = value;
            if(resetTime)
            {
                TimeSinceSetValue = 0;
            }

        }
    }

}
