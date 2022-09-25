using UnityEngine;
using Constants;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    public LevelLoader LevelLoader;
    public GameObject MainMenuNoSave;
    public GameObject MainMenuHasSave;

    private string ContinueScene; 

    private void Start() 
    {
        if(DataPersistenceManager.Instance.IsHasGameData())
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
        DataPersistenceManager.Instance.NewGame();

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
