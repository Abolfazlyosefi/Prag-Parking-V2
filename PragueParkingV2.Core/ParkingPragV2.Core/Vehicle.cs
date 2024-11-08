namespace pragueParkingV2.Core.Models
{

    public class Vehicle
    {
        public string LicensePlate { get; set; } // Registreringsnummer för fordonet
        public DateTime ParkingTime { get; set; } // Tidpunkt då fordonet parkerades

        // Egenskap för att definiera antal gratis minuter (standardvärde 10)
        public virtual int FreeMinutes { get; } = 10;

        // Ny egenskap för fordonets storlek
        public int Size { get; protected set; }

        // Konstruktör som initierar registreringsnummer och sätter parkeringsstarttiden
        public Vehicle(string licensePlate)
        {
            LicensePlate = licensePlate;
            ParkingTime = DateTime.Now;
        }

        // Abstrakt metod för att beräkna parkeringsavgiften, som nu implementeras av underklasser
        public virtual decimal CalculateParkingFee()
        {
            return 0; // Standard logik för avgift kan sättas här om du vill.
        }
    }
}
