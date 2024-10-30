using pragueParkingV2.Core.Models;
using pragueParkingV2.Core.Services;
using Spectre.Console;

namespace Prag_Parking_V2.PragParkingV2.Console
{
    public class SecretMenu
    {

        private readonly ParkingGarage _garage;
        private readonly string _password;

        public SecretMenu(ParkingGarage garage, string password)
        {
            _garage = garage;
            _password = password;
        }

        public void EnterSecretMenu()
        {
            // Fråga efter lösenord
            var inputPassword = AnsiConsole.Ask<string>("Enter the secret password:");

            if (inputPassword != _password)
            {
                AnsiConsole.MarkupLine("[red]Incorrect password! Access denied.[/]");
                Thread.Sleep(1500);
                return;
            }

            // Om lösenordet är korrekt, visa menyn
            ShowMenu();
        }

        private void ShowMenu()
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine("[bold red] ______     ______     ______     ______     ______     ______      __    __     ______     __   __     __  __    [/]");
            AnsiConsole.MarkupLine("[bold red]/\\  ___\\   /\\  ___\\   /\\  ___\\   /\\  == \\   /\\  ___\\   /\\__  _\\    /\\ \"-./  \\   /\\  ___\\   /\\ \"-.\\ \\   /\\ \\/\\ \\   [/]");
            AnsiConsole.MarkupLine("[bold red]\\ \\___  \\  \\ \\  __\\   \\ \\ \\____  \\ \\  __<   \\ \\  __\\   \\/_/\\ \\/    \\ \\ \\-./\\ \\  \\ \\  __\\   \\ \\ \\-.  \\  \\ \\ \\_\\ \\  [/]");
            AnsiConsole.MarkupLine("[bold red] \\/\\_____\\  \\ \\_____\\  \\ \\_____/  \\ \\_\\ \\_\\  \\ \\_____\\    \\ \\_\\     \\ \\_\\ \\ \\_\\  \\ \\_____\\  \\ \\ _\\\"\\_\\  \\ \\_____\\ [/]");
            AnsiConsole.MarkupLine("[bold red]  \\/_____/   \\/_____/   \\/_____/   \\/_/ /_/   \\/_____/     \\/_/      \\/_/  \\/_/   \\/_____/   \\/_/ \\/_/   \\/_____/  [/]");
            AnsiConsole.MarkupLine("[bold green]                                                                                                                    [/]");




            AnsiConsole.MarkupLine("[bold yellow]Welcome back Admin, what would you like to today?[/]");

            // Använd SelectionPrompt för att skapa en mer interaktiv meny
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .PageSize(10) // Antal alternativ som visas i menyn
                    .AddChoices(new[] {
                "View all parked vehicles",
                "Update pricing",
                "Remove all vehicles",
                "Exit secret menu"
                    }));

            switch (choice)
            {
                case "View all parked vehicles":
                    ViewParkedVehicles();
                    break;
                case "Update pricing":
                    UpdatePricing();
                    break;
                case "Remove all vehicles":
                    RemoveAllVehicles();
                    break;
                case "Exit secret menu":
                    AnsiConsole.MarkupLine("[yellow]Exiting secret menu...[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid choice![/]");
                    break;
            }
        }


        private void RemoveAllVehicles()
        {
            var confirmation = AnsiConsole.Confirm("Are you sure you want to remove all vehicles?");
            if (confirmation)
            {
                _garage.RemoveAllVehicles(); // Anropa metoden i ParkingGarage för att radera bilar
                AnsiConsole.MarkupLine("[green]All vehicles have been removed.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Operation canceled.[/]");
            }
        }

        private void ViewParkedVehicles()
        {
            try
            {
                AnsiConsole.MarkupLine("[bold blue]Currently parked vehicles:[/]");

                foreach (var spot in _garage.GetParkingSpots())
                {
                    var parkingSpot = spot as ParkingSpot;

                    if (parkingSpot != null)
                    {
                        if (parkingSpot.IsOccupied)
                        {
                            AnsiConsole.MarkupLine($"[green]Spot {parkingSpot.SpotId}: {parkingSpot.ParkedVehicle.LicensePlate}[/]");
                            Thread.Sleep(12200);

                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[red]Spot {parkingSpot.SpotId}: Empty[/]");
                        }
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Error: Invalid parking spot.[/]");
                    }
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            }

            // Vänta på att användaren ska trycka på en tangent
            AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
            //Console.Readline();
        }


        private void UpdatePricing()
        {
            // Implementera logik för att uppdatera prissättning
            AnsiConsole.MarkupLine("[bold blue]Update Pricing[/]");
            // Exempel på hur man kan fråga efter nya priser
            // Logik för att uppdatera prissättning kan läggas här
        }
    }
}

