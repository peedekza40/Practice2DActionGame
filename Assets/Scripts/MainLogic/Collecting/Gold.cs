using Core.Constants;
using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
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
}
