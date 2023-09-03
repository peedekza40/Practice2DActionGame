using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class QualityController : MonoBehaviour
{
    public TMP_Dropdown QualityDropdown;

    private int CurrentApplyQualityLevel;

    private void OnEnable() 
    {
        SetCurrentApplyQualityLevel();

        var options = new List<string>();
        foreach(var qualityLevel in QualitySettings.names)
        {
            options.Add(qualityLevel);
        }

        QualityDropdown.ClearOptions();
        QualityDropdown.AddOptions(options);
        QualityDropdown.value = CurrentApplyQualityLevel;
    }

    public void SetQualityLevel(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public void ResetQuality()
    {
        QualitySettings.SetQualityLevel(CurrentApplyQualityLevel);
    }

    public void SetCurrentApplyQualityLevel()
    {
        CurrentApplyQualityLevel = QualitySettings.GetQualityLevel();
    }
}
