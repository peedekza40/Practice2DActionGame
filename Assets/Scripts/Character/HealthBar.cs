using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    
    public void SetMaxHealth(float maxHealth)
    {
        Slider.maxValue = maxHealth;
    }

    public void SetHealth(float health)
    {
        Slider.value = health;
    }
}
