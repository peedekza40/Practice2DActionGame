using System.Linq;
using Character.Animators;
using Character.Combat.States.Player;
using Constants;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.InputSystem;
using UI;
using UnityEngine;
using Zenject;

namespace Character.Status
{
    public class PlayerStatus : CharacterStatus, IDataPersistence
    {
        public SliderBar StaminaBar;
        public float CurrentStamina { get; private set; }

        public EquipmentConfig CurrentArmor { get; private set; }
        public EquipmentConfig CurrentBoot { get; private set; }
        public EquipmentConfig CurrentGlove { get; private set; }

        public PlayerHandler PlayerHandler { get; private set; }
        private BlockFlashAnimatorController BlockFlashAnimatorController;
        private StateMachine CombatStateMachine;

        #region Dependencies
        private PlayerInputControl playerInputControl;
        private IEquipmentConfigRepository equipmentConfigRepository;
        #endregion 

        [Inject]
        public void Init(
            PlayerInputControl playerInputControl,
            IEquipmentConfigRepository equipmentConfigRepository)
        {
            this.playerInputControl = playerInputControl;
            this.equipmentConfigRepository = equipmentConfigRepository;
        }

        protected override void Awake()
        {
            PlayerHandler = GetComponent<PlayerHandler>();
            BaseAttribute = PlayerHandler.Attribute;
            BlockFlashAnimatorController = GameObject.Find(GameObjectName.EffectBlock).GetComponent<BlockFlashAnimatorController>();
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat); 
            base.Awake();

            PlayerHandler.Attribute.Level = 1;
            StaminaBar.SetMaxValue(PlayerHandler.Attribute.MaxStamina);
            SetCurrentStamina(PlayerHandler.Attribute.MaxStamina);
            InvokeRepeating(nameof(RegenStamina), 0f, 1);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void TakeDamage(float damage, GameObject attackerHitBox)
        {
            //check attacker is in direction of player
            bool attackerIsInDirection = false;
            if(transform.localScale.x > 0)
            {
                attackerIsInDirection = attackerHitBox.transform.position.x > transform.position.x;
            }
            else if (transform.localScale.x < 0) 
            {
                attackerIsInDirection = attackerHitBox.transform.position.x < transform.position.x;
            }

            var reduceDamage = 0f;
            bool isBlocking = CombatStateMachine.IsCurrentState(typeof(BlockingState));
            if(isBlocking && attackerIsInDirection)
            {
                bool isParry = CombatStateMachine.IsCurrentState(typeof(BlockParryState));
                if(isParry)
                {
                    reduceDamage = damage;
                    BlockFlashAnimatorController.TriggerParryEffect();

                    //deflect attacker
                    var enemyAI = attackerHitBox.GetComponentInParent<EnemyAI>();
                    enemyAI.TriggerDeflected();
                }
                else
                {
                    reduceDamage = (PlayerHandler.Attribute.ReduceDamagePercent / 100) * damage;
                }

                damage -= reduceDamage;
            }
            else if (isBlocking && attackerIsInDirection == false) {
                CombatStateMachine.SetNextState(new BlockFinisherState());
            }

            //reduce damage from equipment
            var equipmentReduceDamagePercent = 0f;
            equipmentReduceDamagePercent += CurrentArmor?.MaxDefense ?? 0f;
            equipmentReduceDamagePercent += CurrentGlove?.MaxDefense ?? 0f;
            equipmentReduceDamagePercent += CurrentBoot?.MaxDefense ?? 0f;
            damage -= (equipmentReduceDamagePercent / 100) * damage;
            
            base.TakeDamage(damage, attackerHitBox);
        }

        public override void Die()
        {
            PlayerHandler.Combat.enabled = false;
            GetComponent<PlayerController>().enabled = false;
            base.Die();
        }

        public void SetLevel(int level)
        {
            PlayerHandler.Attribute.Level = level;
        }

        public void SetMaxStamina(float maxStamina)
        {
            PlayerHandler.Attribute.MaxStamina = maxStamina;
            StaminaBar.SetMaxValue(PlayerHandler.Attribute.MaxStamina);
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
            if(StaminaBar.TimeSinceSetValue >= PlayerHandler.Attribute.RegenStaminaSpeedTime){
                CurrentStamina += PlayerHandler.Attribute.RegenStamina;
                if(CurrentStamina >= PlayerHandler.Attribute.MaxStamina){
                    CurrentStamina = PlayerHandler.Attribute.MaxStamina;
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

            SetCurrentStamina(data.PlayerData.CurrentStamina);
            SetMaxStamina(data.PlayerData.MaxStamina);
            PlayerHandler.Attribute.RegenStamina = data.PlayerData.RegenStamina;
            PlayerHandler.Attribute.RegenStaminaSpeedTime = data.PlayerData.RegenStaminaSpeedTime;
            PlayerHandler.Attribute.StaminaUse = data.PlayerData.StaminaUse;

            PlayerHandler.Attribute.Level = data.PlayerData.Level;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.CurrentHP = CurrentHP;
            data.PlayerData.MaxHP = PlayerHandler.Attribute.MaxHP;

            data.PlayerData.CurrentStamina = CurrentStamina;
            data.PlayerData.MaxStamina = PlayerHandler.Attribute.MaxStamina;
            data.PlayerData.RegenStamina = PlayerHandler.Attribute.RegenStamina;
            data.PlayerData.RegenStaminaSpeedTime = PlayerHandler.Attribute.RegenStaminaSpeedTime;
            data.PlayerData.StaminaUse = PlayerHandler.Attribute.StaminaUse;

            data.PlayerData.Level = PlayerHandler.Attribute.Level;
        }
    }
}
