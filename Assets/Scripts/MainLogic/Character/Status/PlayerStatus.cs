using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine;

namespace Character
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        public int Level { get; private set; }

        private PlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;
        private StateMachine MeleeStateMachine;

        void Awake()
        {
            PlayerCombat = GetComponent<PlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            MeleeStateMachine = GetComponent<StateMachine>();

            Level = 1;
            base.BaseAwake();
        }

        void Update()
        {
            base.BaseUpdate();
        }

        public override void TakeDamage(float damage, GameObject attackerHitBox)
        {
            var reduceDamage = 0f;
            bool isBlocking = MeleeStateMachine.CurrentState.GetType() == typeof(BlockingState);
            if(isBlocking)
            {
                bool isPressBlockThisFrame = MeleeStateMachine.CurrentState.time <= 0.15;
                if(isPressBlockThisFrame)
                {
                    reduceDamage = damage;
                    BlockFlashAnimatorController.TriggerParryEffect();
                }
                else
                {
                    reduceDamage = (PlayerCombat.ReduceDamagePercent/100) * damage;
                }

                damage -= reduceDamage;
            }
            
            base.TakeDamage(damage, attackerHitBox);
        }

        public override void Die()
        {
            PlayerCombat.enabled = false;
            GetComponent<PlayerController>().enabled = false;
            base.Die();
        }

        public void SetLevel(int level)
        {
            Level = level;
        }

        public void LoadData(GameDataModel data)
        {
            SetCurrentHP(data.PlayerData.CurrentHP);
            SetMaxHealth(data.PlayerData.MaxHP);
            Level = data.PlayerData.Level;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.CurrentHP = CurrentHP;
            data.PlayerData.MaxHP = MaxHP;
            data.PlayerData.Level = Level;
        }
    }
}
