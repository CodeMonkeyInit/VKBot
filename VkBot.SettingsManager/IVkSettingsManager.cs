namespace VkBot.SettingsManager
{
    public interface IVkSettingsManager
    {
        Settings Settings { get; }

        Settings GetSettings();
        void SaveSettings(Settings settings);
    }
}