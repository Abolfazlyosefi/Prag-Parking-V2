using Microsoft.VisualStudio.TestTools.UnitTesting;
using pragueParkingV2.Core.Models;
using pragueParkingV2.Core.Services;

namespace pragueParkingV2.Tests
{
    [TestClass]
    public class ParkingGarageTests
    {
        [TestMethod]
        public void TestParkVehicle()
        {
            var garage = new ParkingGarage(100, new ConfigData());
            var car = new Car("ABC123");

            var result = garage.ParkVehicle(car);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestRemoveVehicle()
        {
            var garage = new ParkingGarage(100, new ConfigData());
            var car = new Car("ABC123");

            garage.ParkVehicle(car);
            var result = garage.RemoveVehicle("ABC123");

            Assert.IsTrue(result);
        }
    }
}
