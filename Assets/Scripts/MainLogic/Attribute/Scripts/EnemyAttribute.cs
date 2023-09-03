using UnityEngine;

[CreateAssetMenu]
public class EnemyAttribute : CharacterAttribute
{
    [Header("Combat - Attack")]
    public float AttackDuration = 1.5f;
    public float TimeBetweenCombo = 0.25f;
    public float MaxDamage = 20f;
    public float MinDamage = 10f;
    public float BufferTimeAttack = 0.5f;
}