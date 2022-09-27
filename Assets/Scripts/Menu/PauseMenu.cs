using System.Collections;
using System.Collections.Generic;
using Character;
using Constants;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IUIPersistence
{
    public GameObject MenuPanel;
    public GameObject Player;

    private DeathScript DeathScript;
    public UINumber Number => UINumber.PauseMenu;
    public bool IsOpen { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        DeathScript = Player.GetComponent<DeathScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!MenuPanel.activeSelf)
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
        MenuPanel.SetActive(true);
        IsOpen = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
        IsOpen = false;
    }

    public void Save()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void Load()
    {
        DataPersistenceManager.Instance.LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
