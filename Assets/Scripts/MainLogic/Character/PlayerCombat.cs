using System.Collections.Generic;
using UnityEngine;
using Core.Constants;
using System.Linq;
using UnityEngine.InputSystem;
using Infrastructure.InputSystem;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Zenject;
using Core.Configs;

namespace Character
{
    public class PlayerCombat : MonoBehaviour, IPlayerCombat, IDataPersistence
    {
        [Header("Attack")]
        public float TimeBetweenCombo = 0.45f;
        public float TimeBetweenAttack = 0.4f;
        public float TimeBetweenNextMove = 0.35f;
        public float Damage = 10f;
        public Collider2D HitBox;
        public LayerMask EnemyLayers;
        public int? CountAttack { get; private set; }
        public bool IsAttacking { get; private set; }
        private float TimeSinceAttack = 0f;
        private int DamageFrameCount = 0;

        [Header("Block")]
        [Range(0, 100)]
        public float ReduceDamagePercent = 5f;
        public float TimeBetweenBlock = 0.6f;
        public bool IsPressingBlock { get; private set; }
        public bool IsBlocking { get; private set; }
        public bool PressBlockThisFrame { get; private set; }
        public bool IsParry { get; private set; }
        private float TimeSinceBlocked = 0f;
        private float TimeSincePressBlock = 0f;

        private IPlayerController PlayerController;
        private IAnimatorController AnimatorController;
        private Gold Gold;
        private AnimatorStateInfo AnimationState;
        private FrameInput Input;
        private List<EnemyStatus> AttackedEnemies = new List<EnemyStatus>();

        #region Dependencies
        private IAppSettingsContext appSettingsContext;
        private PlayerInputControl playerInputControl;
        #endregion

        [Inject]
        public void Init(
            IAppSettingsContext appSettingsContext,
            PlayerInputControl playerInputControl)
        {
            this.appSettingsContext = appSettingsContext;
            this.playerInputControl = playerInputControl;
        }

        void Awake()
        {
            Damage = appSettingsContext.Configure.Combat.Attacking.DefaultDamage;
            TimeBetweenAttack = appSettingsContext.Configure.Combat.Attacking.DefaultTimeBetweenAttack;
            ReduceDamagePercent = appSettingsContext.Configure.Combat.Blocking.DefaultReduceDamagePercent;
            TimeBetweenBlock = appSettingsContext.Configure.Combat.Blocking.DefaultTimeBetweenBlock;

            CountAttack = 0;
            PlayerController = GetComponent<IPlayerController>();
            AnimatorController = GetComponentInChildren<IAnimatorController>();
            Gold = GetComponent<Gold>();
        }

        // Start is called before the first frame update
        void Start()
        {
            playerInputControl.AttackInput.performed += (context) => { StartAttack(); };
            playerInputControl.BlockInput.performed += (context) => { StartBlock(); };
            playerInputControl.BlockInput.canceled += (context) => { FinishBlock(); };
        }

        // Update is called once per frame
        void Update()
        {
            AnimationState = AnimatorController.Animator.GetCurrentAnimatorStateInfo(0);
            Input = PlayerController.Input;
            SetIsAttacking(CountAttack);

            Attacking();
            if(IsPressingBlock)
            {
                StartBlock();
            }
            Blocking();
            CheckAttackedCharaterDeath();
        }

        #region  Attacking
        private void StartAttack()
        {
            if(IsBlocking == false 
                && ((CountAttack < 3 && TimeSinceAttack > TimeBetweenAttack)
                    || (CountAttack == 3 && TimeSinceAttack > TimeBetweenAttack + TimeBetweenCombo)))
            {
                CountAttack++;
                // Loop back to one after third attack or Reset Attack combo if time since last attack is too large
                if(CountAttack > 3 || TimeSinceAttack > TimeBetweenAttack + TimeBetweenNextMove)
                {
                    CountAttack = 1;
                }
                
                //update animation
                AnimatorController.TriggerAttack(CountAttack);

                //Reset
                TimeSinceAttack = 0f;
                DamageFrameCount = 0;
            }
        }

        private void Attacking()
        {
            // Increase timer that controls attack combo
            TimeSinceAttack += Time.deltaTime;

            //damage enemy
            if(IsAttacking && DamageFrameCount == 0)
            {
                //detect enemy in range of attack
                List<Collider2D> hitEnemies = new List<Collider2D>();
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(EnemyLayers);
                HitBox.OverlapCollider(contactFilter, hitEnemies);

                //damage them
                foreach (var hitEnemy in hitEnemies)
                {
                    var attackedEnemy = hitEnemy.GetComponent<EnemyStatus>();
                    attackedEnemy?.TakeDamage(Damage, HitBox.gameObject);
                    AttackedEnemies.Add(attackedEnemy);
                    DamageFrameCount++;
                }
            }
            else if(IsAttacking == false)
            {
                DamageFrameCount = 0;
            }
        }
        
        private void CheckAttackedCharaterDeath()
        {
            var deathAttackedEnemies = AttackedEnemies.Where(x => x.IsDeath).ToList();
            foreach(var deathAttackedEnemy in deathAttackedEnemies)
            {
                if(deathAttackedEnemy.GetIsCollectedGold() == false)
                {
                    Debug.Log($"Collect gold enemy : {deathAttackedEnemy.name}");
                    Gold.Collect(deathAttackedEnemy.Type);
                    deathAttackedEnemy.SetIsCollectedGold(true);
                    AttackedEnemies.Remove(deathAttackedEnemy);
                }

            }
        }

        private void SetIsAttacking(int? countAttack)
        {
            IsAttacking = false;
            if(countAttack != null && countAttack > 0)
            {
                IsAttacking = AnimationState.IsName($"{AnimationName.Attack}{countAttack}");
            }
        }
        #endregion
        
        #region  Blocking
        public float GetReduceDamagePercent()
        {
            return ReduceDamagePercent;
        }

        public void SetIsParry(bool isParry)
        {
            IsParry = isParry;
        }

        private void StartBlock()
        {
            if(IsAttacking == false && TimeSinceBlocked > TimeBetweenBlock)
            {   
                TimeSinceBlocked = 0f;
                TimeSincePressBlock = 0f;
                IsBlocking = true;
            }
            IsPressingBlock = true;
        }

        private void Blocking()
        {
            PressBlockThisFrame = false;
            IsParry = false;
            TimeSincePressBlock += Time.deltaTime;
            if(IsBlocking == false)
            {
                TimeSinceBlocked += Time.deltaTime;
            }

            if(TimeSincePressBlock <= 0.15)
            {
                PressBlockThisFrame = true;
            }

            AnimatorController.SetBlock(IsBlocking);
        }

        private void FinishBlock()
        {
            IsBlocking = false;
            IsPressingBlock = false;
        }

        #endregion

        public void LoadData(GameDataModel data)
        {
            Damage = data.PlayerData.AttackDamage;
            TimeBetweenAttack = data.PlayerData.TimeBetweenAttack;
            ReduceDamagePercent = data.PlayerData.ReduceDamagePercent;
            TimeBetweenBlock = data.PlayerData.TimeBetweenBlock;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.AttackDamage = Damage;
            data.PlayerData.TimeBetweenAttack = TimeBetweenAttack;
            data.PlayerData.ReduceDamagePercent = ReduceDamagePercent;
            data.PlayerData.TimeBetweenBlock = TimeBetweenBlock;
        }
    }
}
