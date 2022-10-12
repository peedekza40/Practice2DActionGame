using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataModel
{
    [Header("Player Status")]
    public float CurrentHP;
    public float MaxHP;
    public int GoldAmount;
    public float ReduceDamagePercent;
    public float AttackDamage;
    public float TimeBetweenAttack;//Attack Speed

    [Header("Player Sprite")]
    public Vector3 Position;
    public Vector3 Scale;

    public PlayerDataModel()
    {
        CurrentHP = 0;
        MaxHP = 0;
        GoldAmount = 0;
        ReduceDamagePercent = 0;
        AttackDamage = 0;
        TimeBetweenAttack = 0;
        Position = Vector3.zero;
    }
}
