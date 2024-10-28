namespace pragueParkingV2.Core.Models
{
    public enum ParkingSpotSize
    {
        Small,
        Medium,
        Large
    }
    public class ParkingSpot
    {
        public int SpotId { get; }
        public Vehicle? ParkedVehicle { get; set; }
        public bool IsOccupied => ParkedVehicle != null;
        public ParkingSpotSize Size { get; set; } // Ny egenskap för storlek

        public ParkingSpot(int spotId, ParkingSpotSize size)
        {
            SpotId = spotId;
            Size = size;
        }


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
