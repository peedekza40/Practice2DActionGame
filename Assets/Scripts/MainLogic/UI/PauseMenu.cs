using Character;
using Core.Constants;
using Core.DataPersistence;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI
{
    public class PauseMenu : MonoBehaviour, IUIPersistence
    {
        public GameObject MenuPanel;
        public OptionMenu OptionMenu;
        public GameObject Player;

        private CanvasGroup MenuGroup;
        private FadeUI MenuFadeUI;

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
            MenuGroup = MenuPanel.GetComponent<CanvasGroup>();
            MenuFadeUI = MenuPanel.GetComponent<FadeUI>(); 
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
                if(!MenuPanel.activeSelf && !OptionMenu.gameObject.activeSelf)
                {
                    Pause();
                }
                else if(MenuPanel.activeSelf)
                {
                    Resume();
                }
            }
        }

        private void Pause()
        {
            MenuPanel.SetActive(true);
            MenuFadeUI?.ShowUI(() => { Time.timeScale = 0f; });
            IsOpen = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            MenuFadeUI?.HideUI(() => { MenuPanel.SetActive(false); });
            IsOpen = false;
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

        public void Quit()
        {
            Application.Quit();
        }
    }

}
