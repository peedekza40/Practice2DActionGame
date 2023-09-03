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
    public class PlayerCombat : MonoBehaviour, IDataPersistence
    {
        [Header("UI")]
        public SpriteRenderer WeaponSprite;
        

        [Header("Attack")]
        public Collider2D HitBox;
        public LayerMask EnemyLayers;
        
        public bool IsPressingBlock { get; private set; }

        public EquipmentConfig CurrentWeapon { get; private set; }
        public StateMachine CombatStateMachine { get; private set; }
        private PlayerHandler PlayerHandler;

        #region Dependencies
        public PlayerInputControl playerInputControl { get; private set; }
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

        private void Awake() 
        {
            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat); 
            PlayerHandler = GetComponent<PlayerHandler>();
        }

        private void Start() 
        {
        }

        private void Update() 
        {
            if(IsPressingBlock)
            {
                StartBlockState();
            }
        }

        public void SetMeleeState()
        {
            if((CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) || CombatStateMachine.IsCurrentState(typeof(BlockFinisherState)))
               && CurrentWeapon != null
               && PlayerHandler.Status.CurrentStamina >= PlayerHandler.Attribute.StaminaUse)
            {
                CombatStateMachine.SetNextState(new MeleeEntryState());
            }
        }

        public void StartBlockState()
        {
            IsPressingBlock = true;
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)))
            {
                CombatStateMachine.SetNextState(new BlockParryState());
            }
        }

        public void FinishBlockState()
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
            PlayerHandler.Attribute.MaxDamage = data.PlayerData.MaxDamage;
            PlayerHandler.Attribute.AttackDuration = data.PlayerData.AttackDuration;
            PlayerHandler.Attribute.ReduceDamagePercent = data.PlayerData.ReduceDamagePercent;
            PlayerHandler.Attribute.TimeBetweenBlock = data.PlayerData.TimeBetweenBlock;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.MaxDamage = PlayerHandler.Attribute.MaxDamage;
            data.PlayerData.AttackDuration = PlayerHandler.Attribute.AttackDuration;
            data.PlayerData.ReduceDamagePercent = PlayerHandler.Attribute.ReduceDamagePercent;
            data.PlayerData.TimeBetweenBlock = PlayerHandler.Attribute.TimeBetweenBlock;
        }
        #endregion
    }
}
