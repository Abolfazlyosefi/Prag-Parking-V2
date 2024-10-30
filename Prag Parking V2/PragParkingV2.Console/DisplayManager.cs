using pragueParkingV2.Core.Services;
using pragueParkingV2.Core.Models;
using Spectre.Console;
using Prag_Parking_V2.PragParkingV2.Console;

using Microsoft.Extensions.Configuration;

namespace pragueParkingV2.ConsoleApp
{
    public class DisplayManager
    {
        private ParkingGarage garage;
        private SecretMenu secretMenu;
        private pragueParkingV2.Core.Services.ConfigurationManager configurationManager;
        
        public DisplayManager(ParkingGarage parkingGarage)
        {
            garage = parkingGarage;
            secretMenu = new SecretMenu(garage, "Muhammad");
            configurationManager = new pragueParkingV2.Core.Services.ConfigurationManager();
        }
        
        public void ShowMainMenu()
        {
            ShowPricingList();
            while (true)
            {
                ShowWelcomeAnimation();
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("    __   __        __        ___     __        __               __  ");
                AnsiConsole.MarkupLine("   |__) |__)  /\\  / _` |  | |__     |__)  /\\  |__) |__/ | |\\ | / _` ");
                AnsiConsole.MarkupLine("   |  \\ |  \\ /~~\\ \\__> \\__/ |___    |    /~~\\ |  \\ |  \\ | | \\| \\__> ");
                AnsiConsole.MarkupLine("                                                                      ");




                AnsiConsole.MarkupLine("");

                AnsiConsole.MarkupLine("[bold green]Welcome to Prague Parking![/]");

                
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to do today?")
                        .AddChoices(new[] { "Park a vehicle", "Retrieve a vehicle", "Show parking garage status", "Reload pricing list", "Secret menu",  "Exit" })); // Lägg till menyval
                
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
                    case "Reload pricing list":
                        ReloadPricingConfig(); // Anropa nya metoden
                        break;
                    case "Show pricing list": // Lägg till detta alternativ
                        ShowPricingList(); // Anropa metoden för att visa prissättning
                        break;
                    case "Secret menu":
                        secretMenu.EnterSecretMenu();
                        break;
                    case "Exit":
                        Exit();
                        break;
                }
                
            }
            
        }

        static void ShowWelcomeAnimation()
        {

            AnsiConsole.Clear();

            // Welcome animation
            var lines = new[]
{
    " ___    ____    ___                                        ___    ___",
    "  \\\\    //\\\\    //   II===   II        //===     ====      //\\\\  //\\\\      II===",
    "   \\\\  //  \\\\  //    II___   II       //       //    \\\\   //  \\\\//  \\\\     II___",
    "    \\\\//    \\\\//     II      II       \\\\       \\\\    //  //    --    \\\\    II   ",
    "     --      --      II===   II====    \\\\===     ====   //            \\\\   II===",
};

            // Skriv ut varje rad med kort fördröjning
            foreach (var line in lines)
            {
                AnsiConsole.MarkupLine($"[bold green]{line}[/]");
                Thread.Sleep(200);
            }


            // Kort paus efter "Welcome"
            AnsiConsole.MarkupLine("");
            AnsiConsole.MarkupLine("[bold blue]Welcome to Prague Parking![/]");
            Thread.Sleep(800);
            AnsiConsole.MarkupLine("");

            // Öppningsfraser visas en i taget
            AnsiConsole.MarkupLine("");
            AnsiConsole.Markup("                  [bold red]Secure Parking![/]");
            Thread.Sleep(800); // Kort paus
            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine(" ");
            AnsiConsole.Markup("                                      [bold blue]Cheap Prices![/]");
            Thread.Sleep(800); // Kort paus innan huvudmenyn visas
            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine("");
            AnsiConsole.Markup("                                                        [bold yellow]Best Service![/]");
            Thread.Sleep(1200); // Kort paus innan huvudmenyn visas

        }


        public void ShowParkingGarageStatus()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold blue]Parking Garage Status:[/]");

            // Anropa den nya metoden för att visa kartan
            garage.DisplayGarageMap();

            AnsiConsole.MarkupLine("[grey](Press any key to return)[/]");
            Console.ReadKey(true);
        }


        private void ParkVehicle()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Park a Vehicle[/]");
            var licensePlate = AnsiConsole.Ask<string>("Enter the vehicle's license plate (6-10 characters):");

            // Kontrollera längden på registreringsnumret
            if (licensePlate.Length < 6 || licensePlate.Length > 10)
            {
                AnsiConsole.MarkupLine("[red]License plate must be between 6 and 10 characters.[/]");
                Thread.Sleep(1200);
                return; // Avbryt om längden är ogiltig
            }

            // Fråga användaren om fordonstyp
            var vehicleType = AnsiConsole.Ask<string>("Is it a Car or a Motorcycle? (C/M)").ToUpper();
            Vehicle vehicle;

            if (vehicleType == "C")
            {
                vehicle = new Car(licensePlate); // Skapa Car
            }
            else if (vehicleType == "M")
            {
                vehicle = new Motorcycle(licensePlate); // Skapa Motorcycle
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid vehicle type. Please enter C or M.[/]");
                Thread.Sleep(1200);
                return; // Avbryt om fordonstypen är ogiltig
            }

            // Animering för parkering
            for (int i = 10; i > 0; i--)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"{new string(' ', i)}🚗 [bold green]Parking in progress...[/]");
                Thread.Sleep(100);
            }

            // Försök att parkera fordonet
            if (garage.ParkVehicle(vehicle))
            {
                AnsiConsole.MarkupLine($"[green]Vehicle with license plate {licensePlate} has been parked.[/]");
                Thread.Sleep(1200);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No available parking spots or vehicle already parked.[/]");
            }

            AnsiConsole.MarkupLine("[grey](Press any key to return)[/]");
            Console.ReadKey(true);
        }


        private void RetrieveVehicle()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold yellow]Retrieve a Vehicle[/]");

            var licensePlate = AnsiConsole.Ask<string>("Enter the vehicle's license plate:");

            for (int i = 0; i < 10; i++)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"{new string(' ', i)}🚗 [bold green]Retrieving car in progress...[/]");
                Thread.Sleep(100);
            }

            // Deklarera parkingFee här
            int parkingFee = 0;

            // Kontrollera om fordonet finns och ta bort det
            if (garage.RemoveVehicle(licensePlate))
            {
                // Tilldela parkingFee efter att fordonet tagits bort
                parkingFee = garage.CalculateParkingFee(licensePlate);

                AnsiConsole.MarkupLine($"[green]Vehicle with license plate {licensePlate} has been retrieved.[/]");
                AnsiConsole.MarkupLine($"[bold yellow]Total parking fee: {parkingFee} kr[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Vehicle not found.[/]");
            }

            AnsiConsole.MarkupLine("[grey](Press any key to return)[/]");
            Console.ReadKey(true);
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
                // Animerat farvälmeddelande
                var goodbyeMessage = "Goodbye and drive safely! ✌️";

                // Skriv ut varje bokstav med kort fördröjning för animeringseffekten
                foreach (var letter in goodbyeMessage)
                {
                    AnsiConsole.Markup($"[bold blue]{letter}[/]");
                    Thread.Sleep(50); // Paus mellan varje bokstav
                }

                AnsiConsole.MarkupLine(""); // Ny rad efter meddelandet
                Thread.Sleep(800); // Paus för att låta användaren se meddelandet

                AnsiConsole.MarkupLine("[bold green]Thank you for using Prague Parking![/] 🚗");
                Thread.Sleep(1200); // Paus innan programmet avslutas

                Environment.Exit(0); // Avsluta programmet
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Returning to the main menu...[/]");
                // Här kan du lägga till kod för att gå tillbaka till huvudmenyn om det behövs.
            }
        }



        public void ReloadPricingConfig()
        {
            var newPricing = configurationManager.LoadPricingConfig();
            garage.UpdatePricing(newPricing); // Lägg till denna metod i ParkingGarage (se steg 3)
            AnsiConsole.MarkupLine("[green]Pricing list has been reloaded successfully![/]");
        }
        private void ShowPricingList()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold blue]Pricing List:[/]");

            // Hämta prissättningen från garaget
            var pricing = garage.GetPricing(); // Se till att du har en GetPricing metod i ParkingGarage

            foreach (var item in pricing)
            {
                AnsiConsole.MarkupLine($"[bold yellow]{item.Key}:[/] {item.Value} kr");
            }

            AnsiConsole.MarkupLine("[grey](Press any key to return)[/]");
            Console.ReadKey(true);
        }

    }

}

  
                       
          