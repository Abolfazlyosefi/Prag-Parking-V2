using pragueParkingV2.Core.Services;
using pragueParkingV2.Core.Models;
using Spectre.Console;

namespace pragueParkingV2.ConsoleApp
{
    public class DisplayManager
    {
        private ParkingGarage garage;

        public DisplayManager(ParkingGarage parkingGarage)
        {
            garage = parkingGarage;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[bold green]Welcome to Prague Parking![/]");
                AnsiConsole.MarkupLine("[1] Park a vehicle");
                AnsiConsole.MarkupLine("[2] Retrieve a vehicle");
                AnsiConsole.MarkupLine("[3] Show parking garage status");
                AnsiConsole.MarkupLine("[4] Exit");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .AddChoices(new[] { "1", "2", "3", "4" }));

                switch (choice)
                {
                    case "1":
                        ParkVehicle();
                        break;
                    case "2":
                        RetrieveVehicle();
                        break;
                    case "3":
                        ShowParkingGarageStatus();
                        break;
                    case "4":
                        return;
                }
            }
        }

        public void ShowParkingGarageStatus()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold blue]Parking Garage Status:[/]");
            garage.DisplayGarageMap();
            AnsiConsole.Prompt(new TextPrompt<string>("[grey](Press any key to return)[/]"));
        }

        private void ParkVehicle()
        {
            // Hantering av parkering
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Park a Vehicle[/]");
            var licensePlate = AnsiConsole.Ask<string>("Enter the vehicle's license plate:");

            // Skapa ett nytt fordon av typen Car
            var vehicle = new Car(licensePlate); // Använd Car eller en annan konkret klass

            if (garage.ParkVehicle(vehicle))
            {
                AnsiConsole.MarkupLine($"[green]Vehicle with license plate {licensePlate} has been parked.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No available parking spots.[/]");
            }

            AnsiConsole.Prompt(new TextPrompt<string>("[grey](Press any key to return)[/]"));
        }

        private void RetrieveVehicle()
        {
            // Hantering av uthämtning
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Retrieve a Vehicle[/]");
            var licensePlate = AnsiConsole.Ask<string>("Enter the vehicle's license plate:");

            if (garage.RemoveVehicle(licensePlate))
            {
                AnsiConsole.MarkupLine($"[green]Vehicle with license plate {licensePlate} has been retrieved.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Vehicle not found.[/]");
            }

            AnsiConsole.Prompt(new TextPrompt<string>("[grey](Press any key to return)[/]"));
        }
    }
}
