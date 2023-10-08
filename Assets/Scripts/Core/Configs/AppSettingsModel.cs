using System;

namespace Core.Configs
{
    [Serializable]
    public class AppSettingsModel
    {
        public DatabaseModel Database;
        public StatusModel Status;
        public CombatModel Combat;

        public string StartSceneName;

        public AppSettingsModel()
        {
            Database = new DatabaseModel
            {
                Path = "/Database/PlatformRPG.db",
                Password = "Asdf+1234"
            };

            Status = new StatusModel
            {
                MaxHP = 100f,
                MaxStamina = 100f,
                DefaultRegenStamina = 15f,
                DefaultRegenStaminaSpeedTime = 1f,
                MaxDecreaseRegenStaminaSpeedTime = 0.5f
            };


            Combat = new CombatModel
            {
                Attacking = new AttackingModel
                {
                    DefaultMaxDamage = 10f,
                    DefaultAttackDuration = 0.6f,
                    MaxDecreaseAttackDuration = 0.35f,
                    DefaultStaminaUse = 20f
                },
                Blocking = new BlockingModel
                {
                    DefaultReduceDamagePercent = 5f,
                    DefaultTimeBetweenBlock = 0.6f,
                    MaxDecreaseTimeBetweenBlock = 0.4f
                }
            };

            StartSceneName = "Level 1";
        }
    
        public class DatabaseModel 
        {
            public string Path;
            public string Password;
        }

        [Serializable]
        public class StatusModel
        {
            public float MaxHP;
            public float MaxStamina;
            public float DefaultRegenStamina;
            public float DefaultRegenStaminaSpeedTime;
            public float MaxDecreaseRegenStaminaSpeedTime;
        }

        [Serializable]
        public class CombatModel 
        {
            public AttackingModel Attacking;
            public BlockingModel Blocking;
        }

        [Serializable]
        public class AttackingModel 
        {
            public float DefaultMaxDamage;
            public float DefaultAttackDuration;
            public float MaxDecreaseAttackDuration;
            public float DefaultStaminaUse;
        }
        
        [Serializable]
        public class BlockingModel 
        {
            public float DefaultReduceDamagePercent;
            public float DefaultTimeBetweenBlock;
            public float MaxDecreaseTimeBetweenBlock;
        }
    
    }
}