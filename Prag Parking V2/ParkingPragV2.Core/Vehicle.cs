using System;

namespace pragueParkingV2.Core.Models
{
    public abstract class Vehicle
    {
        public string LicensePlate { get; set; }
        public DateTime ParkingTime { get; set; }

        public Vehicle(string licensePlate)
        {
            LicensePlate = licensePlate;
            ParkingTime = DateTime.Now;
        }

        public abstract decimal CalculateParkingFee();
    }
}
