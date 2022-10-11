using UnityEngine;
using Infrastructure.Dependency;
using Core.DataPersistence;
using Core.DataPersistence.Data;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    public LevelLoader LevelLoader;
    public GameObject MainMenuNoSave;
    public GameObject MainMenuHasSave;

    private string ContinueScene; 
    private DataPersistenceManager DataPersistenceManager { get; set; }

    private void Start() 
    {
        DataPersistenceManager = DependenciesContext.Dependencies.Get<DataPersistenceManager>();    
        if(DataPersistenceManager.IsHasGameData())
        {
            MainMenuNoSave.SetActive(false);
            MainMenuHasSave.SetActive(true);
        }
        else
        {
            MainMenuNoSave.SetActive(true);
            MainMenuHasSave.SetActive(false);
        }
    }

    public void NewGame(string sceneName)
    {
        //create a new game - which will initialize our game data
        DataPersistenceManager.NewGame();

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

    public void LoadData(GameData data)
    {
        ContinueScene = data.CurrentScene; 
    }

    public void SaveData(ref GameData data)
    {
    }
}
