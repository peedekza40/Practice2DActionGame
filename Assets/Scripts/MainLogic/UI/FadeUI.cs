using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public CanvasGroup UIGroup;
    public bool FadeIn = false;
    public bool FadeOut = false;
    public float FadeSpeed = 1f;

    private Action OnFinished;

    public void ShowUI(Action onFinished = null)
    {
        FadeIn = true;
        OnFinished = onFinished;
    }

    public void HideUI(Action onFinished = null)
    {
        FadeOut = true;
        OnFinished = onFinished;
    }

    private void Update() 
    {
        if(FadeIn && UIGroup.alpha < 1)
        {
            UIGroup.alpha += Time.unscaledDeltaTime * FadeSpeed;
            if(UIGroup.alpha >= 1)
            {
                FadeIn = false;
                UIGroup.interactable = true;
                OnFinished?.Invoke();
            }
        }    

        if(FadeOut && UIGroup.alpha >= 0)
        {
            UIGroup.alpha -= Time.unscaledDeltaTime * FadeSpeed;
            if(UIGroup.alpha == 0)
            {
                FadeOut = false;
                UIGroup.interactable = false;
                OnFinished?.Invoke();
            }
        }

    }
}
