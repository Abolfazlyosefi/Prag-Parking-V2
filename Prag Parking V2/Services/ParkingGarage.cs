
using pragueParkingV2.Core.Models;
using pragueParkingV2.DataAccess;
using System.Text.Json;
using Spectre.Console;

namespace pragueParkingV2.Core.Services
{

    public class ParkingGarage
    {
        private List<ParkingSpot> parkingSpots;
        private ConfigData config;
        private Dictionary<string, int> pricing;

        // Denna är ny för att spara data men fungerar ej. 1
        public ParkingGarage(int totalSpots, ConfigData config, ConfigurationManager configManager)
        {
            this.config = config; // Ladda konfiguration
            this.pricing = config.LoadPricing(configManager); // Ladda prissättning
            parkingSpots = LoadParkingData(); // Ladda befintliga parkeringsplatser från fil

            if (parkingSpots.Count == 0)
            {
                for (int i = 1; i <= totalSpots; i++)
                {
                    ParkingSpotSize size = (i <= 10) ? ParkingSpotSize.Small :
                                           (i <= 20 ? ParkingSpotSize.Medium :
                                           ParkingSpotSize.Large);
                    parkingSpots.Add(new ParkingSpot(i, size));
                }
            }
        }

        public Dictionary<string, int> GetPricing()
        {
            return pricing; // Returnera den laddade prissättningen
        }


        // Metod för att uppdatera prislistan från ConfigurationManager
        public void ReloadPricing(ConfigurationManager configManager)
        {
            var newPricing = configManager.LoadPricingConfig();
            UpdatePricing(newPricing); // Uppdatera prislistan i `ParkingGarage`
        }
        public void UpdatePricing(Dictionary<string, int> newPricing)
        {
            pricing = newPricing;
        }


        // Ny metod för att ladda data
        private List<ParkingSpot> LoadParkingData()
        {
            var dataAccess = new JsonDataAccess();
            return dataAccess.LoadParkingData();
        } // Tills hit är de nytt. 1





        public int CalculateParkingFee(string licensePlate)
        {
            var spot = parkingSpots.Find(s => s.ParkedVehicle?.LicensePlate == licensePlate);
            if (spot != null && spot.ParkedVehicle != null)
            {
                var duration = DateTime.Now - spot.ParkedVehicle.ParkingTime;
                var vehicleType = spot.ParkedVehicle.GetType().Name;

                // Hämta priset för fordonstypen från prislistan
                if (pricing.TryGetValue(vehicleType, out int hourlyRate))
                {
                    int hours = (int)Math.Ceiling(duration.TotalHours);
                    return hours * hourlyRate;
                }
            }
            return 0; // Returnera 0 om priset eller fordonet inte finns
        }



        public bool ParkVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }

            // Kontrollera om ett fordon med samma registreringsnummer redan är parkerat
            if (parkingSpots.Any(s => s.ParkedVehicle?.LicensePlate == vehicle.LicensePlate))
            {
                // Om fordonet redan är parkerat, returnera false
                return false;
            }

            var spot = parkingSpots.Find(s => !s.IsOccupied);
            if (spot != null)
            {
                spot.Park(vehicle);
                return true;
            }
            return false; // Ingen ledig plats
        }


        public bool RemoveVehicle(string licensePlate)
        {
            var spot = parkingSpots.Find(s => s.ParkedVehicle?.LicensePlate == licensePlate);
            if (spot != null)
            {
                spot.RemoveVehicle();
                return true;
            }
            return false;
        }

        public void DisplayGarageMap()
        {

            AnsiConsole.Clear(); // Rensa konsolen för en ren vy
            AnsiConsole.MarkupLine("[bold blue]Parking Garage Map:[/]");
            AnsiConsole.MarkupLine("-------------------");

            // Anta att vi har ett fast antal kolumner (exempelvis 10)
            int columns = 10;
            int rows = (int)Math.Ceiling((double)parkingSpots.Count / columns);

            // Skapa en ny rad för varje parkeringsplats
            for (int row = 0; row < rows; row++)
            {
                var rowString = string.Empty;

                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;

                    if (index < parkingSpots.Count) // Kontrollera att index är inom gränserna
                    {
                        var spot = parkingSpots[index];
                        if (spot.IsOccupied)
                        {
                            // Om platsen är upptagen, visa den med röd färg
                            rowString += $"[red]X[/] "; // 'X' betyder upptagen
                        }
                        else
                        {
                            // Om platsen är ledig, visa den med grön färg
                            rowString += $"[green]O[/] "; // 'O' betyder ledig
                        }
                    }
                    else
                    {
                        // Om det inte finns fler platser, fyll med tomma utrymmen
                        rowString += "[grey] [/] ";
                    }
                }

                AnsiConsole.MarkupLine(rowString); // Visa raden i konsolen
            }

            AnsiConsole.MarkupLine("-------------------");
            AnsiConsole.MarkupLine($"Total available spots: [green]{GetAvailableSpots().Count()}[/]");

        }
        



        public IEnumerable<int> GetAvailableSpots()
        {
            return parkingSpots.Where(s => !s.IsOccupied).Select(s => s.SpotId);
        }
        public TimeSpan GetParkingDuration(string licensePlate)
        {
            var spot = parkingSpots.Find(s => s.ParkedVehicle?.LicensePlate == licensePlate);
            if (spot != null && spot.ParkedVehicle != null)
            {
                return DateTime.Now - spot.ParkedVehicle.ParkingTime;
            }
            return TimeSpan.Zero; // Om fordonet inte finns
        }

        public void SaveParkedVehicles()
        {
            var parkedVehicles = parkingSpots
                .Where(s => s.IsOccupied)
                .Select(s => new ParkedVehicle(s.ParkedVehicle.LicensePlate, s.ParkedVehicle.ParkingTime, s.ParkedVehicle.GetType().Name))
                .ToList();

            var json = JsonSerializer.Serialize(parkedVehicles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("DataAccess/parkedVehicles.json", json); // Spara till fil
        }

        public void LoadParkedVehicles()
        {
            if (File.Exists("DataAccess/parkedVehicles.json"))
            {
                var json = File.ReadAllText("DataAccess/parkedVehicles.json");
                var parkedVehicles = JsonSerializer.Deserialize<List<ParkedVehicle>>(json);

                foreach (var parkedVehicle in parkedVehicles)
                {
                    Vehicle vehicle = parkedVehicle.VehicleType switch
                    {
                        nameof(Car) => new Car(parkedVehicle.LicensePlate) { ParkingTime = parkedVehicle.ParkingTime },
                        nameof(Motorcycle) => new Motorcycle(parkedVehicle.LicensePlate) { ParkingTime = parkedVehicle.ParkingTime },
                        _ => throw new InvalidOperationException("Unknown vehicle type")
                    };

                    var spot = parkingSpots.FirstOrDefault(s => !s.IsOccupied);
                    if (spot != null)
                    {
                        spot.Park(vehicle);
                    }
                }
            }
        }

        public void RemoveAllVehicles()
        {
            foreach (var spot in parkingSpots)
            {
                if (spot.IsOccupied)
                {
                    spot.RemoveVehicle(); // Ta bort fordonet
                }
            }
        }

        internal IEnumerable<object> GetParkingSpots()
        {
            throw new NotImplementedException();
        }
    }

}
