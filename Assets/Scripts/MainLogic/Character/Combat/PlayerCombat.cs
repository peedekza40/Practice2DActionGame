using System.Linq;
using Character.Combat.States;
using Character.Combat.States.Player;
using Character.Inventory;
using Constants;
using Core.Configs;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.InputSystem;
using UnityEngine;
using Zenject;

namespace Character.Combat
{
    public class PlayerCombat : MonoBehaviour, 
        IDataPersistence, 
        IAppSettingsPersistence
    {
        [Header("UI")]
        public SpriteRenderer WeaponSprite;

        [Header("Attack")]
        public float MaxDamage = 10f;
        public float AttackDuration = 0.4f;
        public float TimeBetweenCombo = 0.25f;
        public float StaminaUse = 20f;
        public Collider2D HitBox;
        public LayerMask EnemyLayers;

        [Header("Block")]

        [Range(0, 100)]
        public float ReduceDamagePercent = 5f;
        public float TimeBetweenBlock = 0.6f;
        public float ParryDurtation = 0.15f;
        public bool IsPressingBlock { get; private set; }

        public EquipmentConfig CurrentWeapon { get; private set; }
        private StateMachine CombatStateMachine;
        private PlayerHandler PlayerHandler;

        #region Dependencies
        public PlayerInputControl PlayerInputControl { get; private set; }
        private IEquipmentConfigRepository equipmentConfigRepository;
        #endregion 

        [Inject]
        public void Init(
            PlayerInputControl playerInputControl,
            IEquipmentConfigRepository equipmentConfigRepository)
        {
            PlayerInputControl = playerInputControl;
            this.equipmentConfigRepository = equipmentConfigRepository;
        }

        private void Awake() 
        {
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat); 
            PlayerHandler = GetComponent<PlayerHandler>();
        }

        private void Start() 
        {
            PlayerInputControl.AttackInput.performed += (context) => { SetMeleeState(); };
            PlayerInputControl.BlockInput.performed += (context) => { StartBlockState(); };
            PlayerInputControl.BlockInput.canceled += (context) => { FinishBlockState(); };
        }

        private void Update() 
        {
            if(IsPressingBlock)
            {
                StartBlockState();
            }
        }

        private void SetMeleeState()
        {
            if((CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) || CombatStateMachine.IsCurrentState(typeof(BlockFinisherState)))
               && CurrentWeapon != null
               && PlayerHandler.Status.CurrentStamina >= StaminaUse)
            {
                CombatStateMachine.SetNextState(new MeleeEntryState());
            }
        }

        private void StartBlockState()
        {
            IsPressingBlock = true;
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)))
            {
                CombatStateMachine.SetNextState(new BlockParryState());
            }
        }

        private void FinishBlockState()
        {
            IsPressingBlock = false;
            if(CombatStateMachine.IsCurrentState(typeof(BlockingState)))
            {
                CombatStateMachine.SetNextState(new BlockFinisherState());
            }
        }

        public void SetWeapon(ItemType itemType)
        {
            CurrentWeapon = equipmentConfigRepository.GetByItemType(itemType);
            WeaponSprite.sprite = Resources.Load<Sprite>(CurrentWeapon?.HaveWeaponSpritePath);
        }

        #region IDataPersistence
        public void LoadData(GameDataModel data)
        {
            MaxDamage = data.PlayerData.AttackMaxDamage;
            AttackDuration = data.PlayerData.AttackDuration;
            ReduceDamagePercent = data.PlayerData.ReduceDamagePercent;
            TimeBetweenBlock = data.PlayerData.TimeBetweenBlock;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.AttackMaxDamage = MaxDamage;
            data.PlayerData.AttackDuration = AttackDuration;
            data.PlayerData.ReduceDamagePercent = ReduceDamagePercent;
            data.PlayerData.TimeBetweenBlock = TimeBetweenBlock;
        }
        #endregion

        #region IAppSettingsPersistence
        public int SeqNo { get; private set; } = 1;
        
        public void SetConfig(AppSettingsModel config)
        {
            MaxDamage = config.Combat.Attacking.DefaultDamage;
            AttackDuration = config.Combat.Attacking.DefaultAttackDuration;
            ReduceDamagePercent = config.Combat.Blocking.DefaultReduceDamagePercent;
            TimeBetweenBlock = config.Combat.Blocking.DefaultTimeBetweenBlock;
        }
        #endregion
    }
}
