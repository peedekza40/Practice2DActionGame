using System.Collections.Generic;
using Character;
using Core.Constants;
using Core.Repositories;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using Infrastructure.InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatisticsManagement : MonoBehaviour, IUIPersistence
{
    public RectTransform StatsContainer;
    public RectTransform StatsListContainer;
    public RectTransform StatsRowTemplate;
    public RectTransform InteractDisplay;

    private IStatsConfigRepository statsConfigRepository;
    private PlayerInputControl PlayerInputControl;

    #region IUIPersistence
    public UINumber Number => UINumber.Statistic;
    public bool IsOpen { get; private set; }
    public MouseEvent MouseEvent { get; private set; }
    #endregion

    private void Awake() 
    {
        MouseEvent = StatsContainer.GetComponentInParent<MouseEvent>();
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
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        if(playerStatus != null)
        {
            PlayerInputControl.InteractInput.performed += ToggleStatisticsUI;
            InteractDisplay.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        if(playerStatus != null)
        {
            PlayerInputControl.InteractInput.performed -= ToggleStatisticsUI;
            InteractDisplay.gameObject.SetActive(false);
        }
    }

    private void ToggleStatisticsUI(InputAction.CallbackContext context)
    {
        IsOpen = !IsOpen;
        StatsContainer.gameObject.SetActive(IsOpen);
    }

    private void DrawStatsRow()
    {
        List<StatsConfig> statsConfigs = statsConfigRepository.Get();
        foreach(var statsConfig in statsConfigs)
        {
            RectTransform newStatsRow = Instantiate(StatsRowTemplate, StatsListContainer);
            newStatsRow.gameObject.SetActive(true);

            //set name
            newStatsRow.Find(GameObjectName.StatNameText).GetComponent<TextMeshProUGUI>().SetText(statsConfig.Name);

            //set icon
            Sprite mainIcon = Resources.Load<Sprite>(statsConfig.MainIconPath);
            Sprite subIcon = Resources.Load<Sprite>(statsConfig.SubIconPath);
            Transform icon = newStatsRow.Find(GameObjectName.StatsIcon);
            icon.Find(GameObjectName.MainStatsIcon).GetComponent<Image>().sprite = mainIcon;
            icon.Find(GameObjectName.SubStatsIcon).GetComponent<Image>().sprite = subIcon;
        }
    }
}
