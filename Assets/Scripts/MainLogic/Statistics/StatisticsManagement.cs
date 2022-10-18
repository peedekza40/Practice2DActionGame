using System.Collections.Generic;
using System.Linq;
using Character;
using Core.Configs;
using Core.Constant;
using Core.Constants;
using Core.Repositories;
using Infrastructure.Attributes;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using Infrastructure.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class StatisticsManagement : MonoBehaviour, IUIPersistence
{
    [Header("UI")]
    public Transform StatsContainerTransform;
    public Transform StatsListContainerTransform;
    public StatsRow StatsRowTemplate;
    public Transform InteractDisplayTransform;
    public Transform CurrentGoldTransform; 
    public Transform UseGoldTransform; 
    public Button SubmitButton;
    public Button ClearButton;

    private List<StatsRow> StatsRows { get; set; } = new List<StatsRow>();
    private PlayerHandler PlayerHandler;

    #region Caculate Stats
    private float DefaultTimeBetweenAttack { get; set; }
    private float MaxDecreasaeTimeBetweenAttack { get; set; }
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
    private const int UseGoldPerLevel = 150;
    #endregion

    #region Dependencies
    private IAppSettingsContext appSettingsContext;
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
        IAppSettingsContext appSettingsContext,
        IStatsConfigRepository statsConfigRepository,
        PlayerInputControl playerInputControl
    )
    {
        this.appSettingsContext = appSettingsContext;
        this.statsConfigRepository = statsConfigRepository;
        this.playerInputControl = playerInputControl;
    }


    private void Awake() 
    {
        MouseEvent = StatsContainerTransform.GetComponentInParent<MouseEvent>();
        PlayerHandler = FindObjectsOfType<PlayerHandler>().FirstOrDefault();

        DefaultTimeBetweenAttack = appSettingsContext.Configure.Combat.Attacking.DefaultTimeBetweenAttack;
        MaxDecreasaeTimeBetweenAttack = appSettingsContext.Configure.Combat.Attacking.MaxDecreasaeTimeBetweenAttack;
        DefaultTimeBetweenBlock = appSettingsContext.Configure.Combat.Blocking.DefaultTimeBetweenBlock;
        MaxDecreasaeTimeBetweenBlock = appSettingsContext.Configure.Combat.Blocking.MaxDecreasaeTimeBetweenBlock;

        if(PlayerHandler == null)
        {
            Debug.LogError("Can't find Player object on this scene.");
            this.enabled = false;
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
        if(playerHandler != null)
        {
            playerInputControl.InteractInput.performed += ToggleStatisticsUI;
            InteractDisplayTransform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerHandler playerStatus = other.GetComponent<PlayerHandler>();
        if(playerStatus != null)
        {
            playerInputControl.InteractInput.performed -= ToggleStatisticsUI;
            InteractDisplayTransform.gameObject.SetActive(false);
        }
    }

    private void ToggleStatisticsUI(InputAction.CallbackContext context)
    {
        TempLevel = PlayerHandler.Status.Level;
        SetUseGold(CalculateUseGold());
        SetCurrentGold(PlayerHandler.Gold.Amount);

        IsOpen = !IsOpen;
        StatsContainerTransform.gameObject.SetActive(IsOpen);
    }

    private void DrawStatsRow()
    {
        List<StatsConfig> statsConfigs = statsConfigRepository.Get();
        foreach(var statsConfig in statsConfigs)
        {
            StatsRow newStatsRow = Instantiate(StatsRowTemplate, StatsListContainerTransform);
            StatsActions.Add(statsConfig.Code, InitialStatsActionModel(statsConfig.Code));
            newStatsRow.gameObject.SetActive(true);
            newStatsRow.SetGUI(statsConfig, GetCurrentStatsValue(statsConfig.Code), IncreaseStatsPreview, DecreaseStatsPreview);
            StatsRows.Add(newStatsRow);

        }
    }

    private StatsActionModel InitialStatsActionModel(string statsCode)
    {
        UnityAction calculateIncreaseStatsAction = null;
        UnityAction calculateDecreaseStatsAction = null;
        UnityAction calculateGetCurrentStatsValueAction = null;
        UnityAction<float> setStatsAction = null;

        //add stats action
        UnityAction<StatsRow> addStatsAction = (statsRow) => { 
            ResultValueAction = null;
            ResultValueAction = statsRow.NextStatsValue ?? statsRow.CurrentStatsValue;
            calculateIncreaseStatsAction();
            statsRow.SetNextStatsValue(ResultValueAction);
            
            IncreaseTempLevel();
        };

        //minus stats action
        UnityAction<StatsRow> minusStatsAction = (statsRow) => { 
            ResultValueAction = null;
            ResultValueAction = statsRow.NextStatsValue;
            calculateDecreaseStatsAction();

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
                calculateIncreaseStatsAction = () => { ResultValueAction += 50f; };
                calculateDecreaseStatsAction = () => { ResultValueAction -= 50f; };
                calculateGetCurrentStatsValueAction = () => { ResultValueAction = PlayerHandler.Status.MaxHP; };
                setStatsAction = (newStatsValue) => { PlayerHandler.Status.SetMaxHealth(newStatsValue); };
                break;
            case StatsCode.Damage :
                calculateIncreaseStatsAction = () => { ResultValueAction += 5f; };
                calculateDecreaseStatsAction = () => { ResultValueAction -= 5f; };
                calculateGetCurrentStatsValueAction = () => { ResultValueAction = PlayerHandler.Combat.Damage; };
                setStatsAction = (newStatsValue) => { PlayerHandler.Combat.Damage = newStatsValue; };
                break;
            case StatsCode.AttackSpeed :
                calculateIncreaseStatsAction = () => { ResultValueAction += 10f; };
                calculateDecreaseStatsAction = () => { ResultValueAction -= 10f; };
                calculateGetCurrentStatsValueAction = () => { 
                    ResultValueAction = (DefaultTimeBetweenAttack - PlayerHandler.Combat.TimeBetweenAttack) / MaxDecreasaeTimeBetweenAttack * 100; 
                };
                setStatsAction = (newStatsValue) => { 
                    float newTimeBetweenAttack = DefaultTimeBetweenAttack - ((newStatsValue * MaxDecreasaeTimeBetweenAttack) / 100);
                    PlayerHandler.Combat.TimeBetweenAttack = newTimeBetweenAttack;
                };
                break;
            case StatsCode.BlockDefense :
                calculateIncreaseStatsAction = () => { ResultValueAction += 3f; };
                calculateDecreaseStatsAction = () => { ResultValueAction -= 3f; };
                calculateGetCurrentStatsValueAction = () => { 
                    ResultValueAction = PlayerHandler.Combat.ReduceDamagePercent; 
                };
                setStatsAction = (newStatsValue) => { 
                    PlayerHandler.Combat.ReduceDamagePercent = newStatsValue;
                };
                break;
            case StatsCode.BlockSpeed :
                calculateIncreaseStatsAction = () => { ResultValueAction += 10f; };
                calculateDecreaseStatsAction = () => { ResultValueAction -= 10f; };
                calculateGetCurrentStatsValueAction = () => { 
                    ResultValueAction = (DefaultTimeBetweenBlock - PlayerHandler.Combat.TimeBetweenBlock) / MaxDecreasaeTimeBetweenBlock * 100; 
                };
                setStatsAction = (newStatsValue) => { 
                    float newTimeBetweenBlock = DefaultTimeBetweenBlock - ((newStatsValue * MaxDecreasaeTimeBetweenBlock) / 100);
                    PlayerHandler.Combat.TimeBetweenBlock = newTimeBetweenBlock;
                };
                break;
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

        TempLevel = PlayerHandler.Status.Level;
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
}
