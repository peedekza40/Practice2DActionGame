using System.Collections;
using System.Collections.Generic;
using Core.Constants;
using Core.DataPersistence;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class DeathMenu : MonoBehaviour, IUIPersistence
{
    public GameObject MenuPanel;
    public Animator Animator;
    
    #region Dependencies
    [Inject]
    private readonly DataPersistenceManager dataPersistenceManager;

    [Inject] 
    private readonly GeneralFunction generalFunction;
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

    private void Start()
    {
        var restartButton = MenuPanel.transform.Find(GameObjectName.RestartButton).GetComponent<Button>();
        var returnToMenuButton = MenuPanel.transform.Find(GameObjectName.ReturnToMenuButton).GetComponent<Button>();
        var quitGameButton = MenuPanel.transform.Find(GameObjectName.QuitGameButton).GetComponent<Button>();


        //set onclick
        restartButton.onClick.AddListener(() => generalFunction.Restart());
        returnToMenuButton.onClick.AddListener(() => generalFunction.ReturnToMenu());
        quitGameButton.onClick.AddListener(() => generalFunction.QuitGame());
    }

    public void ShowDeathMenu()
    {
        MenuPanel.SetActive(true);
        Animator.SetTrigger(AnimationParameter.Start);
        IsOpen = true;
    }
}
