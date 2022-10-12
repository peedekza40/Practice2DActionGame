using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour, IDataPersistence
{
    [Header("UI")]
    public TextMeshProUGUI ValueText;
    [Range(0, 5)]
    public float LerpTime = 0f;

    private int Amount = 0;
    private int RunningAmount = 0;

    private void Start() 
    {
        ValueText.SetText(Amount.ToString(Formatter.Amount));    
    }

    private void Update() 
    {
        RunningAmount = (int)Mathf.Ceil(Mathf.Lerp(RunningAmount, Amount, LerpTime * Time.deltaTime));
        ValueText.SetText(RunningAmount.ToString(Formatter.Amount));
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

    public void LoadData(GameDataModel data)
    {
        Amount = data.PlayerData.GoldAmount;
    }

    public void SaveData(GameDataModel data)
    {
        data.PlayerData.GoldAmount = Amount;
    }
}
