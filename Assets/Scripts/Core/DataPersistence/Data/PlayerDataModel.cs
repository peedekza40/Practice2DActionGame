using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataModel
{
    [Header("Player Status")]
    public float CurrentHP;
    public float MaxHP;
    public int Level;
    public int GoldAmount;
    public float AttackDamage;
    public float TimeBetweenAttack;//Attack Speed
    public float ReduceDamagePercent;
    public float TimeBetweenBlock;//Block Speed

    public List<ItemModel> Items;

    [Header("Player Sprite")]
    public Vector3 Position;
    public Vector3 Scale;

    public PlayerDataModel()
    {
        CurrentHP = 0;
        MaxHP = 0;
        Level = 1;
        GoldAmount = 0;
        AttackDamage = 0;
        TimeBetweenAttack = 0;
        ReduceDamagePercent = 0;
        TimeBetweenBlock = 0;
        Position = Vector3.zero;
        Items = new List<ItemModel>();
    }
}
