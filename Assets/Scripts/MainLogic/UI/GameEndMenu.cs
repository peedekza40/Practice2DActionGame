using System.Collections;
using System.Collections.Generic;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameEndMenu : MonoBehaviour, IUIPersistence, IDataPersistence
{
    public GameObject MenuPanel;
    public Animator Animator;
    
    #region Dependencies
    [Inject]
    private DataPersistenceManager dataPersistenceManager;

    [Inject] 
    private readonly GeneralFunction generalFunction;
    #endregion

    #region IUIPersistence
    public UINumber Number => UINumber.GameEndMenu;
    public bool IsOpen { get; private set; }
    public MouseEvent MouseEvent { get; private set; }
    #endregion

    private void Awake() 
    {
        MouseEvent = GetComponent<MouseEvent>();
    }
    
    private void Start()
    {
        var returnToMenuButton = MenuPanel.transform.Find(GameObjectName.ReturnToMenuButton).GetComponent<Button>();
        var quitGameButton = MenuPanel.transform.Find(GameObjectName.QuitGameButton).GetComponent<Button>();


        //set onclick
        returnToMenuButton.onClick.AddListener(() => 
        { 
            dataPersistenceManager.SaveGame();
            generalFunction.ReturnToMenu();
        });
        quitGameButton.onClick.AddListener(() => generalFunction.QuitGame());
    }

    public void ShowGameEndMenu()
    {
        MenuPanel.SetActive(true);
        Animator.SetTrigger(AnimationParameter.Start);
        IsOpen = true;
    }

    #region IDataPersistence
    public void LoadData(GameDataModel data)
    {
    }

    public void SaveData(GameDataModel data)
    {
        data.IsGameEnded = IsOpen;
    }
    #endregion
}
