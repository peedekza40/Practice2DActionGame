using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Configs
{

    [DefaultExecutionOrder(-1)]
    public class AppSettingsContext : MonoBehaviour, IAppSettingsContext
    {
        public AppSettingsModel Config { get; private set; } = new AppSettingsModel();
        public struct UserAttributes {}
        public struct AppAttributes {}

        private List<IAppSettingsPersistence> AppSettingsPersistences;

        private void Awake() 
        {
            AppSettingsPersistences = FindAllAppSettingsPersistencesObjects().OrderBy(x => x.SeqNo).ToList();
            SetupRemoteConfig();
        }

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
            AppSettingsPersistences = FindAllAppSettingsPersistencesObjects().OrderBy(x => x.SeqNo).ToList();
            foreach(var item in AppSettingsPersistences)
            {
                item.SetConfig(Config);
            }
        }
        
        public void FetchRemoteConfig()
        {
            RemoteConfigService.Instance.FetchConfigs<UserAttributes, AppAttributes>(new UserAttributes(), new AppAttributes());
        }

        private async void SetupRemoteConfig()
        {
            if (Utilities.CheckForInternetConnection()) 
            {
                await InitializeRemoteConfigAsync();
            }
            // Add a listener to apply settings when successfully retrieved:
            RemoteConfigService.Instance.FetchCompleted += (configResponse) => { ApplyRemoteSettings(configResponse); };

            // Fetch configuration settings from the remote service:
            FetchRemoteConfig();
        }

        private void ApplyRemoteSettings(ConfigResponse configResponse)
        {
             // Conditionally update settings, depending on the response's origin:
            switch (configResponse.requestOrigin) {
                case ConfigOrigin.Default:
                    Debug.Log ("No settings loaded this session; using default values.");
                    break;
                case ConfigOrigin.Cached:
                    Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log ("New settings loaded this session; update values accordingly.");
                    string appSettingsJson = RemoteConfigService.Instance.appConfig.GetJson("AppSettings");
                    if(appSettingsJson != null)
                    {
                        JsonUtility.FromJsonOverwrite(appSettingsJson, Config);
                    }
                    break;
            }

            foreach(var item in AppSettingsPersistences)
            {
                item.SetConfig(Config);
            }
        }

        private async Task InitializeRemoteConfigAsync()
        {
                var options = new InitializationOptions()
                    .SetOption("com.unity.services.core.environment-name", "development");
                await UnityServices.InitializeAsync(options);

                // remote config requires authentication for managing environment information
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }
        }

        private List<IAppSettingsPersistence> FindAllAppSettingsPersistencesObjects()
        {
            IEnumerable<IAppSettingsPersistence> appSettingsPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IAppSettingsPersistence>();
            return new List<IAppSettingsPersistence>(appSettingsPersistences);
        }
    }
}
