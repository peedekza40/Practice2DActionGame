using System.Linq;
using Character.Combat.States;
using Character.Combat.States.Player;
using Constants;
using Core.Configs;
using Infrastructure.InputSystem;
using UnityEngine;
using Zenject;

namespace Character.Combat
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Attack")]
        public float MaxDamage = 10f;
        public float AttackDuration = 0.4f;
        public float TimeBetweenCombo = 0.25f;
        public Collider2D HitBox;
        public LayerMask EnemyLayers;

        [Header("Block")]

        [Range(0, 100)]
        public float ReduceDamagePercent = 5f;
        public float TimeBetweenBlock = 0.6f;
        public float ParryDurtation = 0.15f;
        public bool IsPressingBlock { get; private set; }

        private StateMachine CombatStateMachine;

        #region Dependencies
        private IAppSettingsContext appSettingsContext;
        public PlayerInputControl PlayerInputControl { get; private set; }
        #endregion 

        [Inject]
        public void Init(
            IAppSettingsContext appSettingsContext,
            PlayerInputControl playerInputControl)
        {
            this.appSettingsContext = appSettingsContext;
            PlayerInputControl = playerInputControl;
        }

        private void Awake() 
        {
            MaxDamage = appSettingsContext.Configure.Combat.Attacking.DefaultDamage;
            AttackDuration = appSettingsContext.Configure.Combat.Attacking.DefaultAttackDuration;
            ReduceDamagePercent = appSettingsContext.Configure.Combat.Blocking.DefaultReduceDamagePercent;
            TimeBetweenBlock = appSettingsContext.Configure.Combat.Blocking.DefaultTimeBetweenBlock;

            CombatStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat); 
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
            if(CombatStateMachine.IsCurrentState(typeof(IdleCombatState)) || CombatStateMachine.IsCurrentState(typeof(BlockFinisherState)))
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
    }
}
