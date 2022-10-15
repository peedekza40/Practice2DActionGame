using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine;

namespace Character
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        public int Level { get; private set; }

        private IPlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;

        void Awake()
        {
            PlayerCombat = GetComponent<IPlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
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
            if(PlayerCombat.IsBlocking)
            {
                if(PlayerCombat.PressBlockThisFrame)
                {
                    reduceDamage = damage;
                    BlockFlashAnimatorController.TriggerParry();
                }
                else
                {
                    reduceDamage = (PlayerCombat.GetReduceDamagePercent()/100) * damage;
                }

                damage -= reduceDamage;
            }
            
            base.TakeDamage(damage, attackerHitBox);
        }

        public override void Die()
        {
            GetComponent<PlayerCombat>().enabled = false;
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
