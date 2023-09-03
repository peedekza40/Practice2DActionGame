using UnityEngine;

[CreateAssetMenu]
public class PlayerAttribute : CharacterAttribute
{
    public float MaxStamina = 100f;
    public float RegenStamina = 15f;
    public float RegenStaminaSpeedTime = 1f;
    public int Level;

    [Header("Combat - Attack")]
    public float MaxDamage = 10f;
    public float AttackDuration = 0.4f;    
    public float TimeBetweenCombo = 0.25f;
    public float StaminaUse = 20f;

    [Header("Combat - Block")]
    [Range(0, 100)]
    public float ReduceDamagePercent = 5f;
    public float TimeBetweenBlock = 0.25f; 
    public float ParryDurtation = 0.15f;
}