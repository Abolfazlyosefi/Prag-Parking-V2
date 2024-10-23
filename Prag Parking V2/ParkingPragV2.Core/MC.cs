namespace pragueParkingV2.Core.Models
{
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string licensePlate) : base(licensePlate) { }

        public override decimal CalculateParkingFee()
        {
            var hoursParked = (DateTime.Now - ParkingTime).TotalHours;
            return (decimal)Math.Ceiling(hoursParked) * 10M; // 10 CZK per timme
        }
    }
}
