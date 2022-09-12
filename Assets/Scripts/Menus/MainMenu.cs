using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public LevelLoader LevelLoader;

    public void PlayGame(string sceneName)
    {
        LevelLoader.LoadLevel(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
