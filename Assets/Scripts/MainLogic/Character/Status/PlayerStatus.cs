using Character.Animators;
using Character.Combat;
using Character.Combat.States.Player;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine;

namespace Character.Status
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        public int Level { get; private set; }

        private PlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;
        private StateMachine CombatStateMachine;

        void Awake()
        {
            PlayerCombat = GetComponent<PlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            CombatStateMachine = GetComponent<StateMachine>();

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
            bool isBlocking = CombatStateMachine.IsCurrentState(typeof(BlockingState));
            if(isBlocking)
            {
                bool isPressBlockThisFrame = CombatStateMachine.GetCurrentStateTime() <= 0.15;
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
