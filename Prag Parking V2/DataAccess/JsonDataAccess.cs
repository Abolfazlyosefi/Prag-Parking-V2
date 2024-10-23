using Newtonsoft.Json;
using pragueParkingV2.Core.Models;


namespace pragueParkingV2.DataAccess
{
    public class JsonDataAccess
    {
        private const string ParkingDataFile = "parking_data.json";

        public List<ParkingSpot> LoadParkingData()
        {
            if (!File.Exists(ParkingDataFile))
                return new List<ParkingSpot>();

            var json = File.ReadAllText(ParkingDataFile);
            return JsonConvert.DeserializeObject<List<ParkingSpot>>(json);
        }

        public void SaveParkingData(List<ParkingSpot> parkingSpots)
        {
            var json = JsonConvert.SerializeObject(parkingSpots, Formatting.Indented);
            File.WriteAllText(ParkingDataFile, json);
        }
    }
}
