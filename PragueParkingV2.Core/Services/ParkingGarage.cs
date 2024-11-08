
using pragueParkingV2.Core.Models;
using pragueParkingV2.DataAccess;
using System.Text.Json;
using Spectre.Console;



namespace PragueParkingV2.Core.Services
{

    public class ParkingGarage
    {
        private List<ParkingSpot> parkingSpots;
        private ConfigData config;
        private Dictionary<string, int> pricing;

        // Denna är ny för att spara data men fungerar ej. 1
        public ParkingGarage(int totalSpots, ConfigData config, ConfigurationManager configManager)
        {
            this.config = config;  // Ladda konfiguration
            this.pricing = config.LoadPricing(configManager);  // Ladda prissättning
            parkingSpots = LoadParkingData();  // Ladda befintliga parkeringsplatser från fil

            if (parkingSpots.Count == 0)
            {
                for (int i = 1; i <= totalSpots; i++)
                {
                    parkingSpots.Add(new ParkingSpot(i)
                    {
                        MaxSize = 4  // Ställ in MaxSize till 4, vilket innebär att två motorcyklar får plats
                    });
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


        public bool TryParkVehicle(Vehicle vehicle)
        {
            foreach (var spot in parkingSpots)
            {
                // Kontrollera om det finns plats för motorcyklar (två motorcyklar får plats)
                if (!spot.IsOccupied || (vehicle is Motorcycle && spot.ParkedVehicles.Count < 2) || (vehicle is Car && spot.ParkedVehicles.Count == 0))
                {
                    if (spot.Park(vehicle))
                    {
                        Console.WriteLine($"{vehicle.GetType().Name} {vehicle.LicensePlate} parkerad i plats {spot.SpotId}");
                        SaveParkedVehicles();  // Se till att denna metod kallas när ett fordon parkeras
                        return true;
                    }
                }
            }
            Console.WriteLine("Ingen tillgänglig plats för detta fordon.");
            return false;
        }











        public int CalculateParkingFee(string licensePlate)
        {
            var spot = parkingSpots.Find(s => s.ParkedVehicle?.LicensePlate == licensePlate);
            if (spot != null && spot.ParkedVehicle != null)
            {
                var duration = DateTime.Now - spot.ParkedVehicle.ParkingTime;
                var vehicleType = spot.ParkedVehicle.GetType().Name;

                if (pricing.TryGetValue(vehicleType, out int hourlyRate) &&
                    pricing.TryGetValue("FreeMinutes", out int freeMinutes))
                {
                    double totalMinutes = duration.TotalMinutes;

                    if (totalMinutes > freeMinutes)
                    {
                        int chargeableMinutes = (int)(totalMinutes - freeMinutes);
                        int hours = (int)Math.Ceiling(chargeableMinutes / 60.0);
                        return hours * hourlyRate;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid pricing data.");
                }
            }
            return 0;
        }






        public bool ParkVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }

            // Använd TryParkVehicle-metoden istället för att parkera direkt
            bool isParked = TryParkVehicle(vehicle);

            if (!isParked)
            {
                Console.WriteLine("Parkering misslyckades.");
            }
            return isParked; // Returnera om parkeringen lyckades
        }




        public bool RemoveVehicle(string licensePlate)
        {
            Console.WriteLine("Trying to remove vehicle with license plate: " + licensePlate);

            var spotToRemove = parkingSpots.Find(s => s.ParkedVehicle?.LicensePlate == licensePlate);
            if (spotToRemove != null && spotToRemove.ParkedVehicle != null)
            {
                spotToRemove.RemoveVehicle(spotToRemove.ParkedVehicle);
                SaveParkedVehicles();  // Se till att denna metod kallas när ett fordon tas bort
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
            // Skapa en lista av parkerade fordon
            var parkedVehicles = parkingSpots
                .Where(s => s.IsOccupied)
                .Select(s => new
                {
                    SpotId = s.SpotId,
                    Vehicles = s.ParkedVehicles.Select(pv => new ParkedVehicle
                    {
                        LicensePlate = pv.LicensePlate,
                        ParkingTime = pv.ParkingTime,
                        VehicleType = pv.GetType().Name
                    }).ToList()
                })
                .ToList();

            // Skapa en JsonDataAccess-instans för att spara data
            var dataAccess = new JsonDataAccess();
            dataAccess.SaveParkingData(parkingSpots); // Kallar på den nya metoden för att spara data

            Console.WriteLine("Saving parked vehicles...");
        }






        public void LoadParkedVehicles()
        {
            try
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
                else
                {
                    Console.WriteLine("Parked vehicles file does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading parked vehicles: {ex.Message}");
            }
        }






        public bool MoveVehicle(string formattedLicensePlate, string vehicleType, int targetSpotId)
        {
            // Hitta parkeringsplatsen med fordonet
            var parkedSpot = parkingSpots.FirstOrDefault(s => s.ParkedVehicle?.LicensePlate == formattedLicensePlate);

            // Kontrollera att fordonet finns
            if (parkedSpot == null || !parkedSpot.IsOccupied)
            {
                return false; // Fordonet finns inte eller är inte parkerat
            }

            // Hitta den nya parkeringsplatsen
            var targetSpot = parkingSpots.FirstOrDefault(s => s.SpotId == targetSpotId);
            if (targetSpot == null || targetSpot.IsOccupied)
            {
                return false; // Målparkeringen är inte tillgänglig
            }

            // Flytta fordonet
            targetSpot.Park(parkedSpot.ParkedVehicle); // Parka fordonet på den nya platsen
            parkedSpot.RemoveVehicle(parkedSpot.ParkedVehicle);

            // Ta bort fordonet från den gamla platsen

            return true; // Flyttningen lyckades
        }

        public void RemoveAllVehicles()
        {
            foreach (var spot in parkingSpots)
            {
                if (spot.IsOccupied)
                {
                    spot.RemoveVehicle(spot.ParkedVehicle);
                    // Ta bort fordonet
                }
            }
        }

        public IEnumerable<ParkingSpot> GetParkingSpots()
        {
            return parkingSpots; // Där parkingSpots är av typen List<ParkingSpot> eller liknande
        }
    }

}
