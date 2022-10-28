using System.Linq;
using Constants;
using Core.Configs;
using Infrastructure.InputSystem;
using UnityEngine;
using Zenject;

namespace Character
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
        public bool IsPressingBlock { get; private set; }

        private StateMachine MeleeStateMachine;

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

            MeleeStateMachine = GetComponents<StateMachine>().FirstOrDefault(x => x.Id == StateId.Combat); 
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
            if(MeleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                MeleeStateMachine.SetNextState(new MeleeEntryState());
            }
        }

        private void StartBlockState()
        {
            IsPressingBlock = true;
            if(MeleeStateMachine.CurrentState.GetType() == typeof(IdleCombatState))
            {
                MeleeStateMachine.SetNextState(new BlockingState());
            }
        }

        private void FinishBlockState()
        {
            IsPressingBlock = false;
            if(MeleeStateMachine.CurrentState.GetType() == typeof(BlockingState))
            {
                MeleeStateMachine.SetNextState(new BlockFinsherState());
            }
        }
    }
}
