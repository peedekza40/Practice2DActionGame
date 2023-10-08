using UnityEngine;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Core.Constants;
using Infrastructure.InputSystem;
using Zenject;
using UnityEngine.UI;
using Core.Configs;

namespace UI
{
    public class MainMenu : MonoBehaviour, IUIPersistence
    {
        public Transform MainMenuTransform;

        #region Dependencies
        [Inject]
        private readonly DataPersistenceManager dataPersistenceManager;

        [Inject]
        private readonly IAppSettingsContext appSettingsContext;

        [Inject] 
        private readonly GeneralFunction generalFunction;
        #endregion

        #region IUIPersistence
        public UINumber Number => UINumber.MainMenu;
        public bool IsOpen { get; private set; }
        public MouseEvent MouseEvent { get; private set; }
        #endregion

        private void Awake() 
        {
            MouseEvent = GetComponent<MouseEvent>();
        }

        private void Start() 
        {
            var continueButton = MainMenuTransform.Find(GameObjectName.ContinueButton).GetComponent<Button>();
            var newGameButton = MainMenuTransform.Find(GameObjectName.NewGameButton).GetComponent<Button>();
            var newGamePlusButton = MainMenuTransform.Find(GameObjectName.NewGamePlusButton).GetComponent<Button>();
            var quitGameButton = MainMenuTransform.Find(GameObjectName.QuitGameButton).GetComponent<Button>();

            //set onclick
            continueButton.onClick.AddListener(() => generalFunction.Continue());
            newGameButton.onClick.AddListener(() => generalFunction.NewGame(appSettingsContext.Config.StartSceneName));
            newGamePlusButton.onClick.AddListener(() => generalFunction.NewGamePlus(appSettingsContext.Config.StartSceneName));
            quitGameButton.onClick.AddListener(() => generalFunction.QuitGame());

            var isGameEnded = dataPersistenceManager.GameData?.IsGameEnded ?? false;
            var isHasGameData = dataPersistenceManager.IsHasGameData();
            continueButton.gameObject.SetActive(isHasGameData && isGameEnded == false);
            newGamePlusButton.gameObject.SetActive(isGameEnded);
            IsOpen = true;
        }
    }
 
}
