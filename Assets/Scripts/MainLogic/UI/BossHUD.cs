using Character.Status;
using Infrastructure.Entities;
using TMPro;
using UnityEngine;
using UI;

public class BossHUD : MonoBehaviour
{
    public FadeUI FadeUI;
    public TextMeshProUGUI NameText;
    public SliderBar HealthBar;
    
    public void ShowHUD(Component component, object data)
    {
        var enemyStatus = component.GetComponent<EnemyStatus>();
        var enemyConfig = (EnemyConfig)data;

        //set name
        NameText.text = enemyConfig.Name;

        //set health bar
        HealthBar.SetMaxValue(enemyStatus.BaseAttribute.MaxHP);
        HealthBar.SetCurrentValue(enemyStatus.CurrentHP);
        enemyStatus.HealthBar.gameObject.SetActive(false);
        enemyStatus.HealthBar = HealthBar;

        FadeUI.ShowUI();
    }
}
