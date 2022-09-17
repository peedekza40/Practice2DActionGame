using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Character;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    public string FileName;
    public bool UseEncryption; 

    public static DataPersistenceManager instance { get; private set; }

    private GameData GameData;
    private List<IDataPersistence> DataPersistences;
    private FileDataHandler FileDataHandler;

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroy the newest one.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName, UseEncryption);
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;    
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;    
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded Called");
        this.DataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded Called");
        SaveGame();
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    public void NewGame()
    {
        var playerStatus = FindObjectsOfType<PlayerStatus>(true).FirstOrDefault();
        this.GameData = new GameData();
        GameData.PlayerHP = playerStatus?.MaxHP ?? 0;
        GameData.Scale = playerStatus?.transform.localScale ?? Vector3.zero;
    }

    public void LoadGame()
    {
        //load any save data from a file using data handler.
        this.GameData = FileDataHandler.Load();

        //if no data can be loaded, intialize to a new game
        if(this.GameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        //push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistence in DataPersistences)
        {
            dataPersistence.LoadData(GameData);
        }

        Debug.Log("Loaded game data.");
    }

    public void SaveGame()
    {
        //if we don't have any data to save, log a warning here
        if(this.GameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        //pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistence in DataPersistences)
        {
            dataPersistence.SaveData(ref GameData);
        }

        //save current scene
        GameData.CurrentScene = SceneManager.GetActiveScene().name;

        Debug.Log("Saved game data.");

        //save that data to a file using the data handler
        FileDataHandler.Save(GameData);
    }

    public bool IsHasGameData()
    {
        return GameData != null;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }
}
