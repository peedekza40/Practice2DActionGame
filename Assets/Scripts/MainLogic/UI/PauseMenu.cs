using Character;
using Core.Constants;
using Core.DataPersistence;
using Infrastructure.InputSystem;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PauseMenu : MonoBehaviour, IUIPersistence
    {
        public GameObject MenuPanel;
        public GameObject Player;

        #region Dependencies
        [Inject]
        private DataPersistenceManager dataPersistenceManager;
        #endregion

        #region IUIPersistence
        public UINumber Number => UINumber.PauseMenu;
        public bool IsOpen { get; private set; }
        public MouseEvent MouseEvent { get; private set; }
        #endregion

        private void Awake() 
        {
            MouseEvent = GetComponent<MouseEvent>();
        }

        // Start is called before the first frame update
        void Start()
        {
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
            dataPersistenceManager.SaveGame();
        }

        public void Load()
        {
            dataPersistenceManager.LoadGame();
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

}
