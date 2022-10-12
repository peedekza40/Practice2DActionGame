using Core.Constants;
using Infrastructure.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsRow : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI StatsName;
    public Image MainIcon;
    public Image SubIcon;
    public Sprite DefaultIcon;
    public Transform CurrentStatsTransform;
    public Button UpStatsButton;

    public string StatsCode { get; private set; }

    public void SetGUI(StatsConfig statsConfig, float currentStats, UnityAction<string> upStatsAction)
    {
        StatsCode = statsConfig.Code;
        //set current stats value
        CurrentStatsTransform.Find(GameObjectName.Value).GetComponent<TextMeshProUGUI>().SetText(currentStats.ToString(Formatter.Amount));

        //set name
        StatsName.SetText(statsConfig.Name);

        //set icon
        Sprite mainIcon = Resources.Load<Sprite>(statsConfig.MainIconPath) ?? DefaultIcon;
        Sprite subIcon = Resources.Load<Sprite>(statsConfig.SubIconPath) ?? DefaultIcon;
        MainIcon.sprite = mainIcon;
        SubIcon.sprite = subIcon;

        //set onclick
        UpStatsButton.onClick.AddListener(() => upStatsAction(statsConfig.Code));
    }
}
