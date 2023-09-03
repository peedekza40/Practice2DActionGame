using System.Collections;
using System.Collections.Generic;
using Core.Constants;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public ResolutionController ResolutionController;
    public QualityController QualityController;
    public Toggle FullscreenCheckbox;
    public FadeUI FadeUI;

    private bool CurrentApplyFullscreen { get; set; }

    private void Awake() 
    {
        FadeUI = GetComponent<FadeUI>();
    }

    private void OnEnable() 
    {
        CurrentApplyFullscreen = Screen.fullScreen;
        FullscreenCheckbox.isOn = CurrentApplyFullscreen;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void Reset()
    {
        ResolutionController.ResetResolution();
        QualityController.ResetQuality();
        Screen.fullScreen = CurrentApplyFullscreen;
    }

    public void Apply()
    {
        ResolutionController.SetCurrentApplyResolution();
        QualityController.SetCurrentApplyQualityLevel();
        CurrentApplyFullscreen = Screen.fullScreen;
    }

    public void ToggleOption(FadeUI menuFadeUI)
    {
        if(FadeUI.FadeIn == false && FadeUI.FadeOut == false){
            if(!gameObject.activeSelf)
            {
                menuFadeUI?.HideUI(() => { menuFadeUI.gameObject.SetActive(false); });
                gameObject.SetActive(true);
                FadeUI.ShowUI();
            }
            else
            {
                menuFadeUI.gameObject.SetActive(true);
                menuFadeUI?.ShowUI();
                FadeUI.HideUI(() => { gameObject.SetActive(false); });
            }
        }
    }
}
