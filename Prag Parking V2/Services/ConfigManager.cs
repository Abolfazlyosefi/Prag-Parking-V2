using Newtonsoft.Json;
using pragueParkingV2.Core.Models;
using System.IO;

namespace pragueParkingV2.Core.Services
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "DataAccess/config.json";
        private const string PricingFilePath = "DataAccess/pricing.json"; // Lägg till denna rad

        public ConfigData LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
                throw new FileNotFoundException("Configuration file not found.");

            var json = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<ConfigData>(json);
        }

        public List<Pricing> LoadPricingConfig()
        {
            if (!File.Exists(PricingFilePath))
                throw new FileNotFoundException("Pricing file not found.");

            var json = File.ReadAllText(PricingFilePath);
            return JsonConvert.DeserializeObject<List<Pricing>>(json);
        }

        public void SaveConfig(ConfigData config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }

        public void SavePricingConfig(List<Pricing> pricing)
        {
            var json = JsonConvert.SerializeObject(pricing, Formatting.Indented);
            File.WriteAllText(PricingFilePath, json);
        }
    }

    public class ConfigData
    {
        public int TotalParkingSpots { get; set; }
        public Dictionary<string, int>? VehicleTypes { get; set; } // Gör nullable
    }
}
