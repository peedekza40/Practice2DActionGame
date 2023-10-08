using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GeneralFunction : MonoBehaviour, IDataPersistence
{
    private LevelLoader LevelLoader;

    private string ContinueScene; 

    #region Dependencies
    [Inject]
    private readonly DataPersistenceManager dataPersistenceManager;
    #endregion

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
        LevelLoader = FindFirstObjectByType<LevelLoader>();
    }


    public void NewGame(string sceneName)
    {
        //create a new game - which will initialize our game data
        dataPersistenceManager.NewGame(false);

        //load the gameplay scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        LevelLoader?.LoadLevel(sceneName);
    }

    public void NewGamePlus(string sceneName)
    {
        //create a new game - which will initialize our game data
        dataPersistenceManager.NewGame(true);

        //load the gameplay scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistenceManager
        LevelLoader?.LoadLevel(sceneName);
    }

    public void Continue()
    {
        //load the next scene - which will in turn load the game because of OnScreenLoaded() in the DataPersistenceManager
        LevelLoader?.LoadLevel(ContinueScene);
    }
    
    public void Save()
    {
        dataPersistenceManager.SaveGame();
    }

    public void Load()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        LevelLoader?.LoadLevel("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region IUIPersistence
    public void LoadData(GameDataModel data)
    {
        ContinueScene = data.CurrentScene; 
    }

    public void SaveData(GameDataModel data)
    {}
    #endregion
}
