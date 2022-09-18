using TMPro;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [Header("Display")]
    public TextMeshProUGUI ValueText;
    [Range(0, 5)]
    public float LerpTime = 0f;

    private int Value = 0;
    private int RunningValue = 0;

    private void Start() 
    {
        ValueText.SetText(Value.ToString(Formatter.Number));    
    }

    private void Update() 
    {
        RunningValue = (int)Mathf.Ceil(Mathf.Lerp(RunningValue, Value, LerpTime * Time.deltaTime));
        ValueText.SetText(RunningValue.ToString(Formatter.Number));
    }

    public void Collect(EnemyType attackedEnemyType)
    {
        switch(attackedEnemyType)
        {
            case EnemyType.Skeleton :
                Value += Random.Range(50, 70);;
                break; 
            default :
                break;
        }
    }
}
