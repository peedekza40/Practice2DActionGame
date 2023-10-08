using Character;
using Core.Constants;
using Core.DataPersistence;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        private readonly DataPersistenceManager dataPersistenceManager;

        [Inject] 
        private readonly GeneralFunction generalFunction;
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
        private void Start()
        {
            var saveButton = MenuPanel.transform.Find(GameObjectName.SaveButton).GetComponent<Button>();
            var loadButton = MenuPanel.transform.Find(GameObjectName.LoadButton).GetComponent<Button>();
            var returnToMenuButton = MenuPanel.transform.Find(GameObjectName.ReturnToMenuButton).GetComponent<Button>();
            var quitGameButton = MenuPanel.transform.Find(GameObjectName.QuitGameButton).GetComponent<Button>();


            //set onclick
            saveButton.onClick.AddListener(() => generalFunction.Save());
            loadButton.onClick.AddListener(() => generalFunction.Load());
            returnToMenuButton.onClick.AddListener(() => generalFunction.ReturnToMenu());
            quitGameButton.onClick.AddListener(() => generalFunction.QuitGame());

            Cursor.visible = false;
        }

        public void TogglePauseMenu()
        {
            if(IsOpen == false && !OptionMenu.gameObject.activeSelf)
            {
                Pause();
            }
            else if(IsOpen && !OptionMenu.gameObject.activeSelf)
            {
                Resume();
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
    }

}
