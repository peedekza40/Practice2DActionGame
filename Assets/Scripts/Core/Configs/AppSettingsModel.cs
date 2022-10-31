using System;

namespace Core.Configs
{
    [Serializable]
    public class AppSettingsModel
    {
        public DatabaseModel Database;
        public CombatModel Combat;
    
        public AppSettingsModel()
        {
            Database = new DatabaseModel
            {
                Path = "/Database/PlatformRPG.db",
                Password = "Asdf+1234"
            };

            Combat = new CombatModel
            {
                Attacking = new AttackingModel
                {
                    DefaultDamage = 10f,
                    DefaultAttackDuration = 0.6f,
                    MaxDecreaseTimeBetweenAttack = 0.35f
                },
                Blocking = new BlockingModel
                {
                    DefaultReduceDamagePercent = 5f,
                    DefaultTimeBetweenBlock = 0.6f,
                    MaxDecreaseTimeBetweenBlock = 0.4f
                }
            };
        }
    
        public class DatabaseModel 
        {
            public string Path;
            public string Password;
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
            public float DefaultDamage;
            public float DefaultAttackDuration;
            public float MaxDecreaseTimeBetweenAttack;
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