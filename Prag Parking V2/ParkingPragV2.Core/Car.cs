namespace pragueParkingV2.Core.Models
{
    public class Car : Vehicle
    {
        public Car(string licensePlate) : base(licensePlate) { }

        public override decimal CalculateParkingFee()
        {
            var hoursParked = (DateTime.Now - ParkingTime).TotalHours;
            return (decimal)Math.Ceiling(hoursParked) * 20M; // 20 CZK per timme
        }
    }
}
