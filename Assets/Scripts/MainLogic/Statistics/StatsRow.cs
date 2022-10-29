using Core.Constants;
using Infrastructure.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Statistics
{
    public class StatsRow : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI StatsName;
        public Image MainIcon;
        public Image SubIcon;
        public Sprite DefaultIcon;
        public Transform CurrentStatsTransform;
        public Transform NextStatsTransform;
        public Transform ArrowTransform;
        public Button AddStatsButton;
        public Button MinusStatsButton;

        public string StatsCode { get; private set; }
        public float CurrentStatsValue { get; private set; }
        public float? NextStatsValue { get; private set; }

        private void Update() 
        {
            if(NextStatsValue != null)
            {
                ArrowTransform.gameObject.SetActive(true);
                NextStatsTransform.gameObject.SetActive(true);
                MinusStatsButton.gameObject.SetActive(true);
            }
            else
            {
                ArrowTransform.gameObject.SetActive(false);
                NextStatsTransform.gameObject.SetActive(false);
                MinusStatsButton.gameObject.SetActive(false);
            }
        }

        public void SetGUI(StatsConfig statsConfig, float currentStatsValue, UnityAction<StatsRow> addStatsAction, UnityAction<StatsRow> minusStatsAction)
        {
            StatsCode = statsConfig.Code;
            //set current stats value
            SetCurrentStatsValue(currentStatsValue);

            //set name
            StatsName.SetText(statsConfig.Name);

            //set icon
            Sprite mainIcon = Resources.Load<Sprite>(statsConfig.MainIconPath) ?? DefaultIcon;
            Sprite subIcon = Resources.Load<Sprite>(statsConfig.SubIconPath) ?? DefaultIcon;
            MainIcon.sprite = mainIcon;
            SubIcon.sprite = subIcon;

            //set onclick
            AddStatsButton.onClick.AddListener(() => addStatsAction(this));
            MinusStatsButton.onClick.AddListener(() => minusStatsAction(this));
        }

        public void SetCurrentStatsValue(float currentStatsValue)
        {
            CurrentStatsValue = currentStatsValue;
            CurrentStatsTransform.Find(GameObjectName.Value).GetComponent<TextMeshProUGUI>().SetText(CurrentStatsValue.ToString(Formatter.Amount));
        }

        public void SetNextStatsValue(float? nextStatsValue)
        {
            NextStatsValue = nextStatsValue;
            NextStatsTransform.Find(GameObjectName.Value).GetComponent<TextMeshProUGUI>().SetText(NextStatsValue?.ToString(Formatter.Amount));
        }
    }

}
