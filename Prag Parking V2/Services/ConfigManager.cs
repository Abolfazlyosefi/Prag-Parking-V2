using Newtonsoft.Json;
using pragueParkingV2.Core.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace pragueParkingV2.Core.Services
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "DataAccess/config.json";
        private const string PricingFilePath = "DataAccess/pricelist.txt"; // Prissättningsfil

        public ConfigData LoadConfig()
        {
            if (!File.Exists(ConfigFilePath))
                throw new FileNotFoundException("Configuration file not found.");

            var json = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<ConfigData>(json);
        }

        public Dictionary<string, int> LoadPricingConfig()
        {
            if (!File.Exists(PricingFilePath))
                throw new FileNotFoundException("Pricing file not found.");

            var pricing = new Dictionary<string, int>();
            var lines = File.ReadAllLines(PricingFilePath);

            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1], out int price))
                {
                    string key = parts[0].Trim();
                    if (key == "FreeMinutes")
                    {
                        // Lägg till gratis minuter till konfigurationsdata
                        if (int.TryParse(parts[1], out int freeMinutes))
                        {
                            pricing["FreeMinutes"] = freeMinutes;
                        }
                    }
                    else
                    {
                        pricing[key] = price;
                    }
                }
            }
            return pricing;
        }


        public void SaveConfig(ConfigData config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(ConfigFilePath, json);
        }

        public void SavePricingConfig(Dictionary<string, int> pricing)
        {
            var lines = pricing.Select(kvp => $"{kvp.Key}:{kvp.Value}").ToArray();
            File.WriteAllLines(PricingFilePath, lines);
        }
    }

    public class ConfigData
    {
        public int TotalParkingSpots { get; set; }
        public Dictionary<string, int>? VehicleTypes { get; set; } // Gör nullable
        public int FreeMinutes { get; set; } // Lägg till gratisminuterna här

        public Dictionary<string, int> LoadPricing(ConfigurationManager configManager)
        {
            return configManager.LoadPricingConfig();
        }
    }

}

