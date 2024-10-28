
using pragueParkingV2.Core.Models;
using pragueParkingV2.DataAccess;

namespace pragueParkingV2.Core.Services
{

    public class ParkingGarage
    {
        private List<ParkingSpot> parkingSpots;
        private ConfigData config;

        // Denna är ny för att spara data men fungerar ej. 1
        public ParkingGarage(int totalSpots, ConfigData config)
        {
            this.config = config; // Tilldela config
            parkingSpots = LoadParkingData(); // Ladda befintliga parkeringsplatser från fil

            if (parkingSpots.Count == 0)
            {
                for (int i = 1; i <= totalSpots; i++) //Detta är nytt för att hantera storlek på p-platser. 1
                {
                    ParkingSpotSize size = (i <= 10) ? ParkingSpotSize.Small : (i <= 20 ? ParkingSpotSize.Medium : ParkingSpotSize.Large);
                    parkingSpots.Add(new ParkingSpot(i, size)); // till hit är det ny. 1
                }

            }
        }

        // Ny metod för att ladda data
        private List<ParkingSpot> LoadParkingData()
        {
            var dataAccess = new JsonDataAccess();
            return dataAccess.LoadParkingData();
        } // Tills hit är de nytt. 1


        // Överlagrad konstruktor som skapar en standardkonfiguration
        public ParkingGarage(int totalSpots) : this(totalSpots, new ConfigData())
        {
        }

        public bool ParkVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle cannot be null.");
            }

            var spot = parkingSpots.Find(s => !s.IsOccupied);
            if (spot != null)
            {
                spot.Park(vehicle);
                return true;
            }
            return false;
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
            foreach (var spot in parkingSpots)
            {
                if (spot.IsOccupied)
                    System.Console.WriteLine($"Spot {spot.SpotId}: {spot.ParkedVehicle.LicensePlate}");
                else
                    System.Console.WriteLine($"Spot {spot.SpotId}: [Empty]");
            }
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
        

    }

}
