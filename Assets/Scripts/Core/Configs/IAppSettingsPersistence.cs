namespace Core.Configs
{
    public interface IAppSettingsPersistence
    {
        int SeqNo { get; }
        void SetConfig(AppSettingsModel config);
    }
}