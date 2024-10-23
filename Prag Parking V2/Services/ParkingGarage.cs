
using pragueParkingV2.Core.Models;

namespace pragueParkingV2.Core.Services
{

    public class ParkingGarage
    {
        private List<ParkingSpot> parkingSpots;
        private ConfigData config;

        // Konstruktorn som tar emot totalSpots och en ConfigData-parameter
        public ParkingGarage(int totalSpots, ConfigData config)
        {
            this.config = config; // Tilldela config
            parkingSpots = new List<ParkingSpot>();
            for (int i = 1; i <= totalSpots; i++)
            {
                parkingSpots.Add(new ParkingSpot(i));
            }
        }

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
    }

}
