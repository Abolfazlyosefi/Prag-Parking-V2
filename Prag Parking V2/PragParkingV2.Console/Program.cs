﻿using pragueParkingV2.Core.Services;

namespace pragueParkingV2.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {


            // Ladda konfigurationen från fil
            var configManager = new ConfigurationManager();
            var config = configManager.LoadConfig();

            // Skapa ett nytt parkeringsgarage
            var garage = new ParkingGarage(config.TotalParkingSpots, config);

            garage.LoadParkedVehicles();

            // Skapa DisplayManager och starta huvudmenyn
            var display = new DisplayManager(garage);

            AppDomain.CurrentDomain.ProcessExit += (s, e) => garage.SaveParkedVehicles();

            display.ShowMainMenu();
           
        }
    }
}
