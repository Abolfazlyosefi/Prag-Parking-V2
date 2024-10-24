using Newtonsoft.Json;

namespace pragueParkingV2.Core.Services
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "DataAccess/config.json";

        public ConfigData LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
                throw new FileNotFoundException("Configuration file not found.");

            var json = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<ConfigData>(json);
        }

        public void SaveConfig(ConfigData config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }
    }

    public class ConfigData
    {
        public int TotalParkingSpots { get; set; }
        public Dictionary<string, int>? VehicleTypes { get; set; } // Gör nullable
    }

}
