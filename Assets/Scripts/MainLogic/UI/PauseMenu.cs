using Core.Constants;
using Core.DataPersistence;
using Infrastructure.Dependency;
using Infrastructure.InputSystem;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IUIPersistence
{
    public GameObject MenuPanel;
    public GameObject Player;

    private DeathScript DeathScript;

    #region Dependencies
    private DataPersistenceManager DataPersistenceManager { get; set; }
    #endregion

    #region IUIPersistence
    public UINumber Number => UINumber.PauseMenu;
    public bool IsOpen { get; private set; }
    public MouseEvent MouseEvent { get; private set; }
    #endregion

    private void Awake() 
    {
        DeathScript = Player.GetComponent<DeathScript>();
        MouseEvent = GetComponent<MouseEvent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DataPersistenceManager = DependenciesContext.Dependencies.Get<DataPersistenceManager>();
        
        Cursor.visible = false;
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
        DataPersistenceManager.SaveGame();
    }

    public void Load()
    {
        DataPersistenceManager.LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
