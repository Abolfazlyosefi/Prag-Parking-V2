namespace pragueParkingV2.Core.Models
{
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string licensePlate) : base(licensePlate)
        {
            Size = 2; // Sätt storleken till 1 för motorcyklar (inte 2 som för bilar)
        }

        public override decimal CalculateParkingFee()
        {
            var totalTimeParked = (DateTime.Now - ParkingTime).TotalMinutes;
            if (totalTimeParked <= FreeMinutes) return 0;

            var hoursParked = (decimal)Math.Ceiling((totalTimeParked - FreeMinutes) / 60);
            return hoursParked * 10M; // 10 CZK per timme för motorcyklar
        }
    }
}

