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
                AnsiConsole.MarkupLine(" ___    ____    ___                                        ___    ___");
                AnsiConsole.MarkupLine("  \\\\    //\\\\    //   II===   II        //===     ====      //\\\\  //\\\\      II===");
                AnsiConsole.MarkupLine("   \\\\  //  \\\\  //    II___   II       //       //    \\\\   //  \\\\//  \\\\     II___");
                AnsiConsole.MarkupLine("    \\\\//    \\\\//     II      II       \\\\       \\\\    //  //    --    \\\\    II   ");
                AnsiConsole.MarkupLine("     --      --      II===   II====    \\\\===     ====   //            \\\\   II===");
                AnsiConsole.MarkupLine("");

                AnsiConsole.MarkupLine("[bold green]Welcome to Prague Parking![/]");
                AnsiConsole.MarkupLine("");


                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                       .Title("What would you like to do today?")

                       .AddChoices(new[] { "Park a vehicle", "Retrieve a vehicle", "Show parking garage status", "Exit" }));

                switch (choice)
                {
                    case "Park a vehicle":
                        ParkVehicle();
                        break;
                    case "Retrieve a vehicle":
                        RetrieveVehicle();
                        break;
                    case "Show parking garage status":
                        ShowParkingGarageStatus();
                        break;
                    case "Exit":
                        Exit();
                        break;
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
        private void Exit()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold red]Are you sure you want to exit?[/]");

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] { "Yes", "No" }));

            if (choice == "Yes")
            {
                AnsiConsole.MarkupLine("[green]Exiting the application. Goodbye and please, drive safely![/]");
                // Eventuell städning eller avslutning av resurser kan göras här.
                Environment.Exit(0); // Avsluta programmet
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Returning to the main menu...[/]");
                // Här kan du anropa en metod för att gå tillbaka till huvudmenyn om det behövs.
            }
        }
    }

}
