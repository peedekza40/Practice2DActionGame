namespace Core.Configs
{
    public interface IAppSettingsContext
    {
        AppSettingsModel Configure { get; }
        void FetchRemoteConfig();
    }
}