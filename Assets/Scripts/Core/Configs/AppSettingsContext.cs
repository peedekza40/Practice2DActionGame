using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Core.Configs
{
    public class AppSettingsContext : IAppSettingsContext
    {
        public AppSettingsModel Configure { get; private set; }
        public struct UserAttributes {}
        public struct AppAttributes {}

        public AppSettingsContext(AppSettingsModel configure)
        {
            Configure = configure;
            SetupRemoteConfig();
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
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            
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
                    JsonUtility.FromJsonOverwrite(appSettingsJson, Configure);
                }
                break;
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

    }
}
