using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject Player;

    private DeathScript DeathScript;
    private PlayerCombat PlayerCombat;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        DeathScript = Player.GetComponent<DeathScript>();
        PlayerCombat = Player.GetComponent<PlayerCombat>();
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
        PlayerCombat.enabled = false;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        MenuPanel.SetActive(false);
        PlayerCombat.enabled = true;
        Cursor.visible = false;
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
