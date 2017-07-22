using System.IO;
using Newtonsoft.Json;

namespace VkBot.SettingsManager
{
    public class Settings
    {
        public string Token { get; set; }
    }

    public class VkSettingsManager : IVkSettingsManager
    {
        private const string SettingsFilename = "settings.ini";
        
        public Settings Settings => GetSettings();

        public Settings GetSettings()
        {
            if (File.Exists(SettingsFilename))
            {
                string serializedSettings = File.ReadAllText(SettingsFilename);

                Settings settings = JsonConvert.DeserializeObject<Settings>(serializedSettings);

                return settings;
            }

            return new Settings();
        }
        
        public void SaveSettings(Settings settings)
        {
            string serializedSettings = JsonConvert.SerializeObject(settings);

            File.WriteAllLines(SettingsFilename, new [] {serializedSettings});
        }
    }
}
