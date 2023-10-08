using System.Collections.Generic;
using Core.Configs;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Core.DataPersistence.Data
{
    [System.Serializable]
    public class GameDataModel
    {
        [Header("Player Data")]
        public PlayerDataModel PlayerData;
        
        [Header("Scene")]
        public Vector3 CameraPosition;
        public string CurrentScene;
        public List<string> CheckedPointIDs = new List<string>();
        public List<string> OpenedChestIDs = new List<string>();

        public bool IsGameEnded = false;

        public GameDataModel(IAppSettingsContext appSettingsContext)
        {
            CameraPosition = Vector3.zero;

            PlayerData = new PlayerDataModel();
            PlayerData.CurrentHP = appSettingsContext.Config.Status.MaxHP;
            PlayerData.MaxHP = appSettingsContext.Config.Status.MaxHP;

            PlayerData.CurrentStamina = appSettingsContext.Config.Status.MaxStamina;
            PlayerData.MaxStamina = appSettingsContext.Config.Status.MaxStamina;
            PlayerData.RegenStamina = appSettingsContext.Config.Status.DefaultRegenStamina;
            PlayerData.RegenStaminaSpeedTime = appSettingsContext.Config.Status.DefaultRegenStaminaSpeedTime;
            
            PlayerData.MaxDamage = appSettingsContext.Config.Combat.Attacking.DefaultMaxDamage;
            PlayerData.AttackDuration = appSettingsContext.Config.Combat.Attacking.DefaultAttackDuration;
            PlayerData.StaminaUse = appSettingsContext.Config.Combat.Attacking.DefaultStaminaUse;

            PlayerData.ReduceDamagePercent = appSettingsContext.Config.Combat.Blocking.DefaultReduceDamagePercent;
            PlayerData.TimeBetweenBlock = appSettingsContext.Config.Combat.Blocking.DefaultTimeBetweenBlock;
        }
    }
}

