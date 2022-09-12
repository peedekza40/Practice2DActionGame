using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject Player;

    private DeathScript deathScript;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        deathScript = Player.GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseMenu.activeSelf)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void Save()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void Load()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
