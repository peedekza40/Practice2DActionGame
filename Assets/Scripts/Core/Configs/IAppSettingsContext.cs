using UnityEngine.Events;

namespace Core.Configs
{
    public interface IAppSettingsContext
    {
        AppSettingsModel Config { get; }
        void FetchRemoteConfig();
    }
}