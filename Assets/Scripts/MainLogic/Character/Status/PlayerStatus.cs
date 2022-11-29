using System.Collections;
using Character.Animators;
using Character.Combat;
using Character.Combat.States.Player;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Core.Repositories;
using Infrastructure.Entities;
using UI;
using UnityEngine;
using Zenject;

namespace Character.Status
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        public float MaxStamina;
        public float CurrentStamina { get; private set; }
        public float RegenStaminaValue = 10f;
        public SliderBar StaminaBar;
        
        public int Level { get; private set; }

        public EquipmentConfig CurrentArmor { get; private set; }
        public EquipmentConfig CurrentBoot { get; private set; }
        public EquipmentConfig CurrentGlove { get; private set; }

        private PlayerCombat PlayerCombat;
        private BlockFlashAnimatorController BlockFlashAnimatorController;
        private StateMachine CombatStateMachine;

        #region Dependencies
        private IEquipmentConfigRepository equipmentConfigRepository;
        #endregion 

        [Inject]
        public void Init(
            IEquipmentConfigRepository equipmentConfigRepository)
        {
            this.equipmentConfigRepository = equipmentConfigRepository;
        }

        protected override void Awake()
        {
            PlayerCombat = GetComponent<PlayerCombat>();
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            CombatStateMachine = GetComponent<StateMachine>();
            base.Awake();

            Level = 1;
            StaminaBar.SetMaxValue(MaxStamina);
            SetCurrentStamina(MaxStamina);
            InvokeRepeating(nameof(RegenStamina), 0f, 1);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void TakeDamage(float damage, GameObject attackerHitBox)
        {
            var reduceDamage = 0f;
            bool isBlocking = CombatStateMachine.IsCurrentState(typeof(BlockingState));
            if(isBlocking)
            {
                bool isParry = CombatStateMachine.IsCurrentState(typeof(BlockParryState));
                if(isParry)
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

        public void SetCurrentStamina(float stamina)
        {
            CurrentStamina = stamina;
            if(CurrentStamina <= 0f)
            {
                CurrentStamina = 0f;
            }
            StaminaBar.SetCurrentValue(CurrentStamina);
        }

        public void RegenStamina()
        {
            if(StaminaBar.TimeSinceSetValue >= 1f){
                CurrentStamina += RegenStaminaValue;
                if(CurrentStamina >= MaxStamina){
                    CurrentStamina = MaxStamina;
                }
                StaminaBar.SetCurrentValue(CurrentStamina, false);  
            }
        }

        public void SetBoot(ItemType itemType)
        {
            CurrentBoot = equipmentConfigRepository.GetByItemType(itemType);
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
