using UnityEngine;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Core.Constants;
using Infrastructure.InputSystem;
using Zenject;

namespace UI
{
    public class MainMenu : MonoBehaviour, IDataPersistence, IUIPersistence
    {
        public LevelLoader LevelLoader;
        public Transform MainMenuTransform;

        private string ContinueScene; 

        #region Dependencies
        [Inject]
        private DataPersistenceManager dataPersistenceManager;
        #endregion

        #region IUIPersistence
        public UINumber Number => UINumber.MainMenu;
        public bool IsOpen { get; private set; }
        public MouseEvent MouseEvent { get; private set; }
        #endregion

        private void Awake() 
        {
            MouseEvent = GetComponent<MouseEvent>();
        }

        private void Start() 
        {
            MainMenuTransform.Find(GameObjectName.ContinueButton).gameObject.SetActive(dataPersistenceManager.IsHasGameData());
            IsOpen = true;
        }

        public void NewGame(string sceneName)
        {
            //create a new game - which will initialize our game data
            dataPersistenceManager.NewGame();

            //load the gameplay scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
            LevelLoader.LoadLevel(sceneName);
        }

        public void Continue()
        {
            //load the next scene - which will in turn load the game because of OnScreenLoaded() in the DataPersistenceManager
            LevelLoader.LoadLevel(ContinueScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadData(GameDataModel data)
        {
            ContinueScene = data.CurrentScene; 
        }

        public void SaveData(GameDataModel data)
        {
        }
    }
 
}
