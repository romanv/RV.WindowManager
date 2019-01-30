namespace RV.WM2.Infrastructure.Core
{
    using System;
    using System.IO;
    using System.Windows;

    using Newtonsoft.Json;

    using RV.WM2.Infrastructure.Models;

    public static class ConfigManager
    {
        private const string ProductName = "RV.WindowManager";
        private const string SlotConfigFileName = "slots.json";

        public static SlotConfig GetSlotConfig()
        {
            var configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ProductName,
                SlotConfigFileName);

            if (File.Exists(configPath))
            {
                var config = LoadSlotConfig(configPath);
                return config;
            }
            else
            {
                var config = new SlotConfig();
                SaveSlotConfig(config);
                return config;
            }
        }

        public static void SaveSlotConfig(SlotConfig config)
        {
            var configPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ProductName,
                SlotConfigFileName);

            try
            {
                var jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                var dir = Path.GetDirectoryName(configPath);

                if (dir == null)
                {
                    return;
                }

                Directory.CreateDirectory(dir);
                File.WriteAllText(configPath, jsonString);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while trying to save slot config:\n" + e.Message);
            }
        }

        private static SlotConfig LoadSlotConfig(string configPath)
        {
            try
            {
                var jsonString = File.ReadAllText(configPath);
                var config = JsonConvert.DeserializeObject<SlotConfig>(jsonString);
                return config;
            }
            catch (Exception e)
            {
                MessageBox.Show("Config load error:\n" + e.Message);
                return null;
            }
        }
    }
}
