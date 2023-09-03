using System.Collections;
using System.Collections.Generic;
using Character;
using Infrastructure.InputSystem;
using Statistics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Infrastructure.Attributes;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine.Rendering.Universal;
using LDtkUnity;

public class CheckPointController : MonoBehaviour, IDataPersistence
{
    public string ID { get; private set; }
    public Transform InteractDisplayTransform;
    public GameObject CampFire;
    public bool EnabledSave = true;

    public bool IsActivated { get; private set; } = false;
    private StatisticsManagement StatisticsManagement;
    private PlayerHandler PlayerHandler;

    #region Dependencies
    [Inject]
    private PlayerInputControl playerInputControl;
    [Inject]
    private DataPersistenceManager dataPersistenceManager;
    #endregion

    private void Awake() 
    {
        StatisticsManagement = GetComponent<StatisticsManagement>();    
        ID = GetComponent<LDtkIid>()?.Iid;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerHandler = other.GetComponent<PlayerHandler>();
        if(PlayerHandler != null)
        {
            InteractDisplayTransform.gameObject.SetActive(true);
            if (IsActivated == false)
            {
                PlayerHandler.InteractAction = ActivateCheckPoint;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        PlayerHandler = other.GetComponent<PlayerHandler>();
        if(PlayerHandler != null)
        {
            InteractDisplayTransform.gameObject.SetActive(false);
            if (IsActivated == false)
            {
                PlayerHandler.InteractAction = null;
            }
        }
    }

    private void SetActivatedGUI()
    {
        IsActivated = true;
        InteractDisplayTransform.GetComponentInChildren<TextMeshProUGUI>().SetText("(E) Up Status");

        var particals = CampFire.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var partical in particals)
        {
            partical.gameObject.SetActive(true);
        }

        var light = CampFire.GetComponentInChildren<Light2D>(true);
        if(light != null)
        {
            light.gameObject.SetActive(true);
        }
    }

    private void ActivateCheckPoint()
    {
        SetActivatedGUI();
        PlayerHandler.AddCheckedPointID(ID);
        PlayerHandler.InteractAction = StatisticsManagement.ToggleStatisticsUI;
        if(EnabledSave)
        {
            dataPersistenceManager.SaveGame();
        }
    }

    public void LoadData(GameDataModel data)
    {
        if(data.CheckedPointIDs.Contains(ID))
        {
            SetActivatedGUI();
        }
    }

    public void SaveData(GameDataModel data)
    {
    }
}
