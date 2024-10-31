using Newtonsoft.Json;
using pragueParkingV2.Core.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace pragueParkingV2.Core.Services
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "DataAccess/config.json"; // Filväg för konfigurationsfilen
        private const string PricingFilePath = "DataAccess/pricelist.txt"; // Filväg för prissättningsfilen

        // Laddar konfigurationsdata från en JSON-fil
        public ConfigData LoadConfig()
        {
            // Kontrollera om konfigurationsfilen existerar
            if (!File.Exists(ConfigFilePath))
                throw new FileNotFoundException("Configuration file not found."); // Utlöser undantag om filen inte hittas

            // Läser JSON-innehållet från filen
            var json = File.ReadAllText(ConfigFilePath);
            // Deserialiserar JSON till ConfigData-objekt
            return JsonConvert.DeserializeObject<ConfigData>(json);
        }

        // Laddar prissättningskonfiguration från en textfil
        public Dictionary<string, int> LoadPricingConfig()
        {
            // Kontrollera om prissättningsfilen existerar
            if (!File.Exists(PricingFilePath))
                throw new FileNotFoundException("Pricing file not found."); // Utlöser undantag om filen inte hittas

            var pricing = new Dictionary<string, int>(); // Skapar en ordbok för prissättning
            var lines = File.ReadAllLines(PricingFilePath); // Läser alla rader från prissättningsfilen

            // Bearbetar varje rad i prissättningsfilen
            foreach (var line in lines)
            {
                var parts = line.Split(':'); // Dela raden i nyckel och värde
                // Kontrollera om det finns exakt två delar och att värdet är ett heltal
                if (parts.Length == 2 && int.TryParse(parts[1], out int price))
                {
                    string key = parts[0].Trim(); // Tar bort eventuella extra mellanslag
                    if (key == "FreeMinutes")
                    {
                        // Lägg till gratis minuter till prissättningen
                        if (int.TryParse(parts[1], out int freeMinutes))
                        {
                            pricing["FreeMinutes"] = freeMinutes; // Lägg till gratis minuter i ordboken
                        }
                    }
                    else
                    {
                        pricing[key] = price; // Lägg till övriga prissättningar
                    }
                }
            }
            return pricing; // Returnera prissättningsordboken
        }

        // Sparar konfigurationsdata till en JSON-fil
        public void SaveConfig(ConfigData config)
        {
            // Serialiserar konfigurationsdata till JSON
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            // Skriver JSON-innehållet till filen
            File.WriteAllText(ConfigFilePath, json);
        }

        // Sparar prissättningsdata till en textfil
        public void SavePricingConfig(Dictionary<string, int> pricing)
        {
            // Omvandlar ordboken till en array av strängar i formatet "nyckel:värde"
            var lines = pricing.Select(kvp => $"{kvp.Key}:{kvp.Value}").ToArray();
            // Skriver raderna till prissättningsfilen
            File.WriteAllLines(PricingFilePath, lines);
        }
    }

    // Modell för konfigurationsdata
    public class ConfigData
    {
        public int TotalParkingSpots { get; set; } // Totalt antal parkeringsplatser
        public Dictionary<string, int>? VehicleTypes { get; set; } // Typ av fordon (nullable)
        public int FreeMinutes { get; set; } // Antal gratis minuter

        // Laddar prissättningsdata via ConfigurationManager
        public Dictionary<string, int> LoadPricing(ConfigurationManager configManager)
        {
            return configManager.LoadPricingConfig(); // Anropar metoden för att ladda prissättningen
        }
    }
}

