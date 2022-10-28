using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.Extensions;
using Infrastructure.InputSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Collecting
{
    public class Gold : MonoBehaviour, IDataPersistence
    {
        [Header("UI")]
        public Transform GoldLabelTransform;
        public TextMeshProUGUI ValueText;

        public int Amount { get; private set; } = 0;
        private int RunningAmount = 0;
        private float CurrentVelocity;

        #region Dependencies
        [Inject]
        private PlayerInputControl playerInputControl;
        #endregion

        private void Start() 
        {
            ValueText.SetText(Amount.ToString(Formatter.Amount));    
        }

        private void Update() 
        {
            float tempRunnigAmount = Mathf.SmoothDamp(RunningAmount, Amount, ref CurrentVelocity, 80 * Time.deltaTime);
            if(RunningAmount <= Amount)
            {
                RunningAmount = Mathf.CeilToInt(tempRunnigAmount);
            }
            else
            {
                RunningAmount = Mathf.FloorToInt(tempRunnigAmount);
            }

            ValueText.SetText(RunningAmount.ToString(Formatter.Amount));

            //hide when statistics ui is open
            bool statisticIsOpen = playerInputControl.UIPersistences.GetByUiNumber(UINumber.Statistic).IsOpen;
            GoldLabelTransform.gameObject.SetActive(!statisticIsOpen);
        }

        public void Collect(EnemyType attackedEnemyType)
        {
            switch(attackedEnemyType)
            {
                case EnemyType.Skeleton :
                    Amount += Random.Range(50, 70);;
                    break; 
                default :
                    break;
            }
        }

        public void SetGoldAmount(int amount)
        {
            Amount = amount;
        }

        public void LoadData(GameDataModel data)
        {
            Amount = data.PlayerData.GoldAmount;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.GoldAmount = Amount;
        }
    }

}
