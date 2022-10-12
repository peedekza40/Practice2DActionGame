using System.Collections.Generic;
using System.Linq;
using Character;
using Core.Constant;
using Core.Constants;
using Core.Repositories;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using Infrastructure.InputSystem;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatisticsManagement : MonoBehaviour, IUIPersistence
{
    [Header("UI")]
    public Transform StatsContainerTransform;
    public Transform StatsListContainerTransform;
    public StatsRow StatsRowTemplate;
    public Transform InteractDisplayTransform;

    private List<StatsRow> StatsRows { get; set; } = new List<StatsRow>();
    private PlayerHandler PlayerHandler;
    private const int TimeBetweenAttackRatio = 10;

    #region Dependencies
    private IStatsConfigRepository statsConfigRepository;
    private PlayerInputControl PlayerInputControl;
    #endregion

    #region IUIPersistence
    public UINumber Number => UINumber.Statistic;
    public bool IsOpen { get; private set; }
    public MouseEvent MouseEvent { get; private set; }
    #endregion

    private void Awake() 
    {
        MouseEvent = StatsContainerTransform.GetComponentInParent<MouseEvent>();
        PlayerHandler = FindObjectsOfType<PlayerHandler>().FirstOrDefault();
        
        if(PlayerHandler == null)
        {
            Debug.LogError("Can't find Player object on this scene.");
            this.enabled = false;
        }
    }

    private void Start() 
    {
        statsConfigRepository = DependenciesContext.Dependencies.Get<IStatsConfigRepository>();
        PlayerInputControl = DependenciesContext.Dependencies.Get<PlayerInputControl>();
        DrawStatsRow();

        IsOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerHandler playerHandler = other.GetComponent<PlayerHandler>();
        if(playerHandler != null)
        {
            PlayerInputControl.InteractInput.performed += ToggleStatisticsUI;
            InteractDisplayTransform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerHandler playerStatus = other.GetComponent<PlayerHandler>();
        if(playerStatus != null)
        {
            PlayerInputControl.InteractInput.performed -= ToggleStatisticsUI;
            InteractDisplayTransform.gameObject.SetActive(false);
        }
    }

    private void ToggleStatisticsUI(InputAction.CallbackContext context)
    {
        IsOpen = !IsOpen;
        StatsContainerTransform.gameObject.SetActive(IsOpen);
    }

    private void DrawStatsRow()
    {
        List<StatsConfig> statsConfigs = statsConfigRepository.Get();
        foreach(var statsConfig in statsConfigs)
        {
            StatsRow newStatsRow = Instantiate(StatsRowTemplate, StatsListContainerTransform);
            newStatsRow.gameObject.SetActive(true);
            newStatsRow.SetGUI(statsConfig, GetCurrentStats(statsConfig.Code), UpStats);
            StatsRows.Add(newStatsRow);
        }
    }

    private void UpStats(string statsCode)
    {
        Debug.Log("Up : " + statsCode);
    }

    private float GetCurrentStats(string statsCode)
    {
        float result = 0f;
        switch(statsCode)
        {
            case StatsCode.MaxHealth :
                result = PlayerHandler.Status.MaxHP;
                break;
            case StatsCode.Damage :
                result = PlayerHandler.Combat.Damage;
                break;
            case StatsCode.AttackSpeed :
                result = PlayerHandler.Combat.TimeBetweenAttack * TimeBetweenAttackRatio;
                break;
            case StatsCode.BlockDefense :
                result = PlayerHandler.Combat.ReduceDamagePercent;
                break;
        }
        return result;
    }
}
