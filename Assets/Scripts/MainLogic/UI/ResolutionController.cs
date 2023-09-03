using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    public TMP_Dropdown ResolutionDropdown;

    private Resolution CurrentApplyResolution;
    private Resolution[] Resolutions;
    private List<Resolution> FilterResolutions = new List<Resolution>();

    private double CurrentRefreshRate;
    private int CurrentResolutionIndex = 0;

    private void OnEnable() 
    {
        SetCurrentApplyResolution();
        Resolutions = Screen.resolutions;
        FilterResolutions = new List<Resolution>();  

        ResolutionDropdown.ClearOptions();
        CurrentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        foreach(var resolution in Resolutions)
        {
            if(resolution.refreshRateRatio.value == CurrentRefreshRate)
            {
                FilterResolutions.Add(resolution);
            }
        }

        var options = new List<string>();
        foreach(var filterResolution in FilterResolutions)
        {
            var resolutionOption = $"{filterResolution.width} x {filterResolution.height} {filterResolution.refreshRateRatio.value} Hz"; 
            options.Add(resolutionOption);
            if(filterResolution.width == Screen.width && filterResolution.height == Screen.height)
            {
                CurrentResolutionIndex = FilterResolutions.IndexOf(filterResolution);
            }
        }

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = FilterResolutions[resolutionIndex]; 
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ResetResolution()
    {
        Screen.SetResolution(CurrentApplyResolution.width, CurrentApplyResolution.height, Screen.fullScreen);
    }

    public void SetCurrentApplyResolution()
    {
        CurrentApplyResolution = new Resolution
        {
            width = Screen.width,
            height = Screen.height
        };
    }
}
