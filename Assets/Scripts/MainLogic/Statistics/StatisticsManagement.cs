using System.Collections.Generic;
using System.Linq;
using Character;
using Core.Configs;
using Core.Constant;
using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Statistics
{
    public class StatisticsManagement : MonoBehaviour, 
        IUIPersistence, 
        IAppSettingsPersistence
    {
        [Header("UI")]
        public Transform StatsContainerTransform;
        public Transform StatsListContainerTransform;
        public StatsRow StatsRowTemplate;
        public Transform CurrentGoldTransform; 
        public Transform UseGoldTransform; 
        public Button SubmitButton;
        public Button ClearButton;

        private List<StatsRow> StatsRows { get; set; } = new List<StatsRow>();
        private List<StatsConfig> StatsConfigs { get; set; } = new List<StatsConfig>();
        private PlayerHandler PlayerHandler;
        private CheckPointController CheckPointController;

        #region Caculate Stats
        private float DefaultRegenStaminaSpeedTime { get; set; }
        private float MaxDecreaseRegenStaminaSpeedTime { get; set; }
        private float DefaultAttackDuration { get; set; }
        private float MaxDecreaseAttackDuration { get; set; }
        private float DefaultTimeBetweenBlock { get; set; }
        private float MaxDecreasaeTimeBetweenBlock { get; set; }
        private Dictionary<string, StatsActionModel> StatsActions = new Dictionary<string, StatsActionModel>();
        private float? ResultValueAction { get; set; }
        #endregion

        #region Caculate Gold
        private int CurrentGoldAmount { get; set; }
        private int UseGoldAmount { get; set; }
        private int UsingGoldAmount = 0;
        private int TempLevel { get; set; }
        private const int UseGoldPerLevel = 50;
        #endregion

        #region Dependencies
        private IStatsConfigRepository statsConfigRepository;
        private PlayerInputControl playerInputControl;
        #endregion

        #region IUIPersistence
        public UINumber Number => UINumber.Statistic;
        public bool IsOpen { get; private set; }
        public MouseEvent MouseEvent { get; private set; }
        #endregion

        [Inject]
        public void Init(
            IStatsConfigRepository statsConfigRepository,
            PlayerInputControl playerInputControl)
        {
            this.statsConfigRepository = statsConfigRepository;
            this.playerInputControl = playerInputControl;
        }


        private void Awake() 
        {
            MouseEvent = StatsContainerTransform.GetComponentInParent<MouseEvent>();
            PlayerHandler = FindObjectsOfType<PlayerHandler>().FirstOrDefault();
            CheckPointController = GetComponent<CheckPointController>();

            if(PlayerHandler == null)
            {
                Debug.LogError("Can't find Player object on this scene.");
                enabled = false;
            }
        }

        private void Start() 
        {
            DrawStatsRow();
            ClearButton.onClick.AddListener(() => ClearNextStats());
            SubmitButton.onClick.AddListener(() => SubmitNextStats());

            IsOpen = false;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            PlayerHandler playerHandler = other.GetComponent<PlayerHandler>();
            if(playerHandler != null && CheckPointController.IsActivated)
            {
                PlayerHandler.InteractAction = ToggleStatisticsUI;
            }
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            PlayerHandler playerHandler = other.GetComponent<PlayerHandler>();
            if(playerHandler != null && CheckPointController.IsActivated)
            {
                PlayerHandler.InteractAction = null;
            }
        }

        public void ToggleStatisticsUI()
        {
            var inventoryIsOpen = playerInputControl.UIPersistences.FirstOrDefault(x => x.Number == UINumber.Inventory).IsOpen;
            if(inventoryIsOpen == false)
            {
                TempLevel = PlayerHandler.Attribute.Level;
                SetUseGold(CalculateUseGold());
                SetCurrentGold(PlayerHandler.Gold.Amount);

                foreach(StatsRow statsRow in StatsRows)
                {
                    statsRow.SetCurrentStatsValue(GetCurrentStatsValue(statsRow.StatsCode));
                }

                var fadeUi = StatsContainerTransform.GetComponent<FadeUI>();
                if(fadeUi.FadeIn == false && fadeUi.FadeOut == false)
                {
                    IsOpen = !IsOpen;
                    if(IsOpen)
                    {
                        StatsContainerTransform.gameObject.SetActive(true);
                        fadeUi.ShowUI();
                    }
                    else {
                        fadeUi.HideUI(() => { StatsContainerTransform.gameObject.SetActive(false); });
                    }
                }
            }
        }

        private void DrawStatsRow()
        {
            StatsConfigs = statsConfigRepository.Get();
            foreach(var statsConfig in StatsConfigs)
            {
                StatsRow newStatsRow = Instantiate(StatsRowTemplate, StatsListContainerTransform);
                StatsActions.Add(statsConfig.Code, InitialStatsActionModel(statsConfig.Code));
                newStatsRow.gameObject.SetActive(true);
                newStatsRow.SetGUI(statsConfig, IncreaseStatsPreview, DecreaseStatsPreview);
                StatsRows.Add(newStatsRow);

            }
        }

        private StatsActionModel InitialStatsActionModel(string statsCode)
        {
            UnityAction<StatsRow> calculateIncreaseStatsAction = (statsRow) => 
            {
                ResultValueAction += StatsConfigs.FirstOrDefault(x => x.Code == statsRow.StatsCode)?.IncreaseValue;
            };

            UnityAction<StatsRow> calculateDecreaseStatsAction = (statsRow) => 
            {
                ResultValueAction -= StatsConfigs.FirstOrDefault(x => x.Code == statsRow.StatsCode)?.IncreaseValue;
            };

            UnityAction calculateGetCurrentStatsValueAction = null;
            UnityAction<float> setStatsAction = null;

            //add stats action
            UnityAction<StatsRow> addStatsAction = (statsRow) => { 
                ResultValueAction = null;
                ResultValueAction = statsRow.NextStatsValue ?? statsRow.CurrentStatsValue;
                calculateIncreaseStatsAction(statsRow);
                statsRow.SetNextStatsValue(ResultValueAction);
                
                IncreaseTempLevel();
            };

            //minus stats action
            UnityAction<StatsRow> minusStatsAction = (statsRow) => { 
                ResultValueAction = null;
                ResultValueAction = statsRow.NextStatsValue;
                calculateDecreaseStatsAction(statsRow);

                if(ResultValueAction <= statsRow.CurrentStatsValue)
                {
                    ResultValueAction = null;
                }

                statsRow.SetNextStatsValue(ResultValueAction);

                DecreaseTempLevel();
            };
            
            //get current stats value action
            UnityAction getCurrentStatsValue = () => { 
                ResultValueAction = 0f;
                calculateGetCurrentStatsValueAction();
            };

            //up stats action
            UnityAction<StatsRow> upStatsAction = (statsRow) => { 
                if(statsRow.NextStatsValue != null)
                {
                    setStatsAction(statsRow.NextStatsValue ?? 0);
                    statsRow.SetNextStatsValue(null);
                }
            };

            switch(statsCode)
            {
                case StatsCode.MaxHealth :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = PlayerHandler.Attribute.MaxHP; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        PlayerHandler.Status.SetMaxHealth(newStatsValue); 
                        PlayerHandler.Status.SetCurrentHP(newStatsValue);
                    };
                    break;
                }
                case StatsCode.Damage :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = PlayerHandler.Attribute.MaxDamage; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        PlayerHandler.Attribute.MaxDamage = newStatsValue; 
                    };
                    break;
                }
                case StatsCode.AttackSpeed :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = (DefaultAttackDuration - PlayerHandler.Attribute.AttackDuration) / MaxDecreaseAttackDuration * 100; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        float newAttackDuration = DefaultAttackDuration - ((newStatsValue * MaxDecreaseAttackDuration) / 100);
                        PlayerHandler.Attribute.AttackDuration = newAttackDuration;
                    };
                    break;
                }
                case StatsCode.BlockDefense :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = PlayerHandler.Attribute.ReduceDamagePercent; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        PlayerHandler.Attribute.ReduceDamagePercent = newStatsValue;
                    };
                    break;
                }
                case StatsCode.BlockSpeed :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = (DefaultTimeBetweenBlock - PlayerHandler.Attribute.TimeBetweenBlock) / MaxDecreasaeTimeBetweenBlock * 100; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        float newTimeBetweenBlock = DefaultTimeBetweenBlock - ((newStatsValue * MaxDecreasaeTimeBetweenBlock) / 100);
                        PlayerHandler.Attribute.TimeBetweenBlock = newTimeBetweenBlock;
                    };
                    break;
                }
                case StatsCode.MaxStamina :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = PlayerHandler.Attribute.MaxStamina; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        PlayerHandler.Status.SetMaxStamina(newStatsValue); 
                        PlayerHandler.Status.SetCurrentStamina(newStatsValue); 
                    };
                    break;
                }
                case StatsCode.RegenStaminaSpeed :
                {
                    calculateGetCurrentStatsValueAction = () => 
                    { 
                        ResultValueAction = (DefaultRegenStaminaSpeedTime - PlayerHandler.Attribute.RegenStaminaSpeedTime) / MaxDecreaseRegenStaminaSpeedTime * 100; 
                    };
                    setStatsAction = (newStatsValue) => 
                    { 
                        float newRegenStaminaSpeedTime = DefaultRegenStaminaSpeedTime - ((newStatsValue * MaxDecreaseRegenStaminaSpeedTime) / 100);
                        PlayerHandler.Attribute.RegenStaminaSpeedTime = newRegenStaminaSpeedTime;
                    };
                    break;
                }
            }

            return new StatsActionModel(addStatsAction, minusStatsAction, getCurrentStatsValue, upStatsAction);
        }

        private void IncreaseStatsPreview(StatsRow statsRow)
        {
            if(IsCanUpStats())
            {
                StatsActions[statsRow.StatsCode].IncreaseStats(statsRow);
            }
        }

        private void DecreaseStatsPreview(StatsRow statsRow)
        {
            StatsActions[statsRow.StatsCode].DecreaseStats(statsRow);
        }

        private float GetCurrentStatsValue(string statsCode)
        {
            StatsActions[statsCode].GetCurrentStatsValue();
            return ResultValueAction ?? 0;
        }

        private void ClearNextStats()
        {
            foreach(StatsRow statsRow in StatsRows)
            {
                statsRow.SetNextStatsValue(null);
            }

            TempLevel = PlayerHandler.Attribute.Level;
            SetUseGold(CalculateUseGold());
            SetUsingGold(0);
        }

        private void SubmitNextStats()
        {
            foreach(StatsRow statsRow in StatsRows)
            {
                StatsActions[statsRow.StatsCode].UpStats(statsRow);
                statsRow.SetCurrentStatsValue(GetCurrentStatsValue(statsRow.StatsCode));
            }
            
            //set new level & gold
            PlayerHandler.Status.SetLevel(TempLevel);
            PlayerHandler.Gold.SetGoldAmount(CurrentGoldAmount - UsingGoldAmount);

            //reset
            SetUsingGold(0);
            SetCurrentGold(PlayerHandler.Gold.Amount);
        }

        private int CalculateUseGold()
        {
            return TempLevel * UseGoldPerLevel;
        }

        private void SetUseGold(int useGoldAmount)
        {
            UseGoldAmount = useGoldAmount;
            UseGoldTransform.Find(GameObjectName.Amount).GetComponent<TextMeshProUGUI>().SetText(UseGoldAmount.ToString(Formatter.Amount));
        }

        private void SetUsingGold(int usingGoldAmount)
        {
            UsingGoldAmount = usingGoldAmount;
            TextMeshProUGUI usingGoldAmountText = CurrentGoldTransform.Find(GameObjectName.UsingAmount).GetComponent<TextMeshProUGUI>();
            if(UsingGoldAmount > 0)
            {
                usingGoldAmountText.gameObject.SetActive(true);
                usingGoldAmountText.SetText("-" + UsingGoldAmount.ToString(Formatter.Amount));
            }
            else
            {
                usingGoldAmountText.gameObject.SetActive(false);
                usingGoldAmountText.SetText("0");
            }
        }

        private void SetCurrentGold(int currentGoldAmount)
        {
            CurrentGoldAmount = currentGoldAmount;
            CurrentGoldTransform.Find(GameObjectName.Amount).GetComponent<TextMeshProUGUI>().SetText(CurrentGoldAmount.ToString(Formatter.Amount));
        }

        private void IncreaseTempLevel()
        {
            SetUsingGold(UsingGoldAmount + UseGoldAmount);
            TempLevel++;
            SetUseGold(CalculateUseGold());
        }

        private void DecreaseTempLevel()
        {
            TempLevel--;
            SetUseGold(CalculateUseGold());
            SetUsingGold(UsingGoldAmount - UseGoldAmount);
        }

        private bool IsCanUpStats()
        {
            return (CurrentGoldAmount - UsingGoldAmount) >= UseGoldAmount;
        }

        #region IAppSettingsPersistence
        public int SeqNo { get; private set; } = 2;
        
        public void SetConfig(AppSettingsModel config)
        {
            DefaultRegenStaminaSpeedTime = config.Status.DefaultRegenStaminaSpeedTime;
            MaxDecreaseRegenStaminaSpeedTime = config.Status.MaxDecreaseRegenStaminaSpeedTime;

            DefaultAttackDuration = config.Combat.Attacking.DefaultAttackDuration;
            MaxDecreaseAttackDuration = config.Combat.Attacking.MaxDecreaseAttackDuration;

            DefaultTimeBetweenBlock = config.Combat.Blocking.DefaultTimeBetweenBlock;
            MaxDecreasaeTimeBetweenBlock = config.Combat.Blocking.MaxDecreaseTimeBetweenBlock;
        }
        #endregion
    }

}
