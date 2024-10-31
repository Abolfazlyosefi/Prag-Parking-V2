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

        // Ny metod för att flytta ett fordon till en annan parkeringsplats
        public bool MoveVehicleTo(ParkingSpot targetSpot)
        {
            if (targetSpot == null)
            {
                throw new ArgumentNullException(nameof(targetSpot), "Target spot cannot be null.");
            }

            // Kontrollera om målet är ledigt
            if (targetSpot.IsOccupied)
            {
                Console.WriteLine("Target spot is already occupied.");
                return false; // Kan inte flytta till en upptagen plats
            }

            // Flytta fordonet
            targetSpot.Park(ParkedVehicle); // Parka fordonet på målet
            RemoveVehicle(); // Ta bort fordonet från den aktuella platsen
            return true; // Flyttning lyckades
        }
    }
}
