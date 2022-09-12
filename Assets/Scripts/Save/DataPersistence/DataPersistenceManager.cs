using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Character;

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
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }

        instance = this;
    }

    private void Start() 
    {
        this.FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName, UseEncryption);
        this.DataPersistences = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.GameData = new GameData();
        GameData.PlayerHP = FindObjectsOfType<PlayerStatus>().FirstOrDefault()?.MaxHP ?? 0;
    }

    public void LoadGame()
    {
        //load any save data from a file using data handler.
        this.GameData = FileDataHandler.Load();

        //if no data can be loaded, intialize to a new game
        if(this.GameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //push the loaded data to all other scripts that need it
        foreach(IDataPersistence dataPersistence in DataPersistences)
        {
            dataPersistence.LoadData(GameData);
        }

        Debug.Log("Loaded Player HP = " + GameData.PlayerHP);
    }

    public void SaveGame()
    {
        //pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistence in DataPersistences)
        {
            dataPersistence.SaveData(ref GameData);
        }

        Debug.Log("Saved Player HP = " + GameData.PlayerHP);

        //save that data to a file using the data handler
        FileDataHandler.Save(GameData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }
}
