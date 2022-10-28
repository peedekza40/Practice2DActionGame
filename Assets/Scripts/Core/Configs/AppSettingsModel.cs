using System;

namespace Core.Configs
{
    [Serializable]
    public class AppSettingsModel
    {
        public DatabaseStruct Database;
        public CombatStruct Combat;
    
        public AppSettingsModel()
        {
            Database = new DatabaseStruct
            {
                Path = "/Database/PlatformRPG.db",
                Password = "Asdf+1234"
            };

            Combat = new CombatStruct
            {
                Attacking = new AttackingStruct
                {
                    DefaultDamage = 10f,
                    DefaultAttackDuration = 0.6f,
                    MaxDecreasaeTimeBetweenAttack = 0.35f
                },
                Blocking = new BlockingStruct
                {
                    DefaultReduceDamagePercent = 5f,
                    DefaultTimeBetweenBlock = 0.6f,
                    MaxDecreasaeTimeBetweenBlock = 0.4f
                }
            };
        }
    
    }

    public class DatabaseStruct 
    {
        public string Path;
        public string Password;
    }

    public class CombatStruct 
    {
        public AttackingStruct Attacking;
        public BlockingStruct Blocking;
    }

    public class AttackingStruct 
    {
        public float DefaultDamage;
        public float DefaultAttackDuration;
        public float MaxDecreasaeTimeBetweenAttack;
    }
    
    public class BlockingStruct 
    {
        public float DefaultReduceDamagePercent;
        public float DefaultTimeBetweenBlock;
        public float MaxDecreasaeTimeBetweenBlock;
    }
}