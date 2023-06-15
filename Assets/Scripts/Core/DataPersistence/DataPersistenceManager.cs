using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Core.DataPersistence.Data;
using Character.Combat;
using Character.Status;
using Core.Configs;
using Zenject;

namespace Core.DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        public string FileName;
        public bool UseEncryption; 

        private GameDataModel GameData;
        private List<IDataPersistence> DataPersistences;
        private FileDataHandler FileDataHandler;

        #region Dependencies
        [Inject]
        private readonly IAppSettingsContext appSettingsContext;
        #endregion 

        private void Awake() 
        {
            this.FileDataHandler = new FileDataHandler(Application.persistentDataPath, FileName, UseEncryption);
        }

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() 
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.DataPersistences = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame()
        {
            GameData = new GameDataModel();
            GameData.PlayerData.CurrentHP = appSettingsContext.Config.Status.MaxHP;
            GameData.PlayerData.MaxHP = appSettingsContext.Config.Status.MaxHP;
            GameData.PlayerData.AttackMaxDamage = appSettingsContext.Config.Combat.Attacking.DefaultMaxDamage;
            GameData.PlayerData.AttackDuration = appSettingsContext.Config.Combat.Attacking.DefaultAttackDuration;
            GameData.PlayerData.ReduceDamagePercent = appSettingsContext.Config.Combat.Blocking.DefaultReduceDamagePercent;
            GameData.PlayerData.TimeBetweenBlock = appSettingsContext.Config.Combat.Blocking.DefaultTimeBetweenBlock;
            SaveGame();
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
                dataPersistence.SaveData(GameData);
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
            IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistences);
        }
    }
}

