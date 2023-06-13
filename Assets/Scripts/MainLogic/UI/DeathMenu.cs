using System.Collections;
using System.Collections.Generic;
using Core.Constants;
using Core.DataPersistence;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DeathMenu : MonoBehaviour, IUIPersistence
{
    public GameObject MenuPanel;
    public Animator Animator;
    
    #region Dependencies
    [Inject]
    private DataPersistenceManager dataPersistenceManager;
    #endregion

    #region IUIPersistence
    public UINumber Number => UINumber.DeathMenu;
    public bool IsOpen { get; private set; }
    public MouseEvent MouseEvent { get; private set; }
    #endregion

    private void Awake() 
    {
        MouseEvent = GetComponent<MouseEvent>();
    }

    public void ShowDeathMenu()
    {
        MenuPanel.SetActive(true);
        Animator.SetTrigger(AnimationParameter.Start);
        IsOpen = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
