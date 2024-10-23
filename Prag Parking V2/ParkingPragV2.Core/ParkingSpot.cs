namespace pragueParkingV2.Core.Models
{
    public class ParkingSpot
    {
        public int SpotId { get; }
        public Vehicle? ParkedVehicle { get; set; } // Gör ParkedVehicle nullable
        public bool IsOccupied => ParkedVehicle != null;

        // Primär konstruktor
        public ParkingSpot(int spotId) => SpotId = spotId;

        public void Park(Vehicle vehicle)
        {
            ParkedVehicle = vehicle;
        }

        public void RemoveVehicle()
        {
            ParkedVehicle = null;
        }
    }
}
