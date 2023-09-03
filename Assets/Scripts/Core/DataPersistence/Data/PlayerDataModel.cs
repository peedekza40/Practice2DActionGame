using System;
using System.Collections.Generic;
using Character.Inventory;
using UnityEngine;

[Serializable]
public class PlayerDataModel
{
    [Header("Player Status")]
    public float CurrentHP;
    public float MaxHP;

    public float CurrentStamina;
    public float MaxStamina;
    public float RegenStamina;
    public float RegenStaminaSpeedTime;

    public int Level;
    public int GoldAmount;
    
    public float MaxDamage;
    public float AttackDuration;//Attack Speed
    public float StaminaUse;

    public float ReduceDamagePercent;
    public float TimeBetweenBlock;//Block Speed

    public string LastCheckPointID;

    public List<ItemModel> Items;

    [Header("Player Sprite")]
    public Vector3? Position;

    public PlayerDataModel()
    {
        CurrentHP = 0;
        MaxHP = 0;

        CurrentStamina = 0;
        MaxStamina = 0;
        RegenStamina = 0;
        RegenStaminaSpeedTime = 0;

        Level = 1;
        GoldAmount = 0;

        MaxDamage = 0;
        AttackDuration = 0;
        StaminaUse = 0;
        
        ReduceDamagePercent = 0;
        TimeBetweenBlock = 0;
        Items = new List<ItemModel>();
    }
}
