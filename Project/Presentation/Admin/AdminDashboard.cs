using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Xml.Schema;

static class AdminDashboard
{
    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Admin Dashboard ===\n");
            Console.WriteLine("1: Flights");
            Console.WriteLine("2: Airports");
            Console.WriteLine("3: Bookings");
            Console.WriteLine("4: Accounts");
            Console.WriteLine("5: Back to main menu");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AdminFlightManagement();
                    break;
                case "2":
                    AdminAirportManagement();
                    break;
                case "3":
                    ShowBookings();
                    break;
                case "4":
                    AdminAccountManagement();
                    break;
                case "5":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return to the Admin Dashboard...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void AdminAirportManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Airport Management ===\n");
            Console.WriteLine("1: View All Airports");
            Console.WriteLine("2: Add An Airport");
            Console.WriteLine("3: Return to Admin Dashboard");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AirportManagement.ViewAllAirports();
                    break;
                case "2":
                    ShowAirports();
                    break;
                case "3":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return to the Admin Dashboard...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void AdminAccountManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Account Management ===\n");
            Console.WriteLine("1: View All Accounts");
            Console.WriteLine("2: Search for an Account");
            Console.WriteLine("3: Return to Admin Dashboard");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ShowAccounts();
                    break;
                case "2":
                    Menu.ShowAllAccounts();
                    break;
                case "3":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return to the Admin Dashboard...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void AdminFlightManagement()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Flight Management ===\n");
            Console.WriteLine("1: View All Flights");
            Console.WriteLine("2: Add a flight");
            Console.WriteLine("3: Return to Admin Dashboard");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AdminFlightList flights  = new();
                    flights.ShowAllAvailableFlightsList();
                    break;
                case "2":
                    AdminFlightMenu.Start();
                    break;
                case "3":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return to the Admin Dashboard...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ShowBookings()
    {
        AdminBookings.Show();
    }

    private static void ShowAccounts()
    {
        Console.Clear();
        Console.WriteLine("=== All Accounts ===\n");

        List<AccountModel> accounts = new AccountsLogic().GetAll();

        Console.WriteLine($"{"ID",-6} {"Name",-24} {"Email",-30} {"Phone",-16} {"Admin"}");
        Console.WriteLine(new string('-', 85));
        foreach (AccountModel account in accounts)
        {
            Console.WriteLine($"{account.Id,-6} {account.FullName,-24} {account.EmailAddress,-30} {account.PhoneNumber,-16} {(account.IsAdmin ? "Yes" : "No")}");
        }

        Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
        Console.ReadKey();
    }

    private static void ShowAirports()
    {
        CreateAirport();
    }

    private static string? ReadLineOrEsc(string prompt)
    {
        Console.Write(prompt);
        var input = new System.Text.StringBuilder();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape)
                return null;
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input.ToString();
            }
            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Remove(input.Length - 1, 1);
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
    }

    private static void ShowCancelled()
    {
        Console.Clear();
        Console.WriteLine("Airport creation cancelled.");
        Console.WriteLine("Press any key to return to Airport Management...");
        Console.ReadKey();
    }

    private static void CreateAirport()
    {
        Console.Clear();
        Console.WriteLine("=== Add Airport ===");
        Console.WriteLine("Press Esc at any time to cancel.\n");

        string? name = ReadLineOrEsc("Name:    ");
        if (name == null) { ShowCancelled(); return; }

        string? address = ReadLineOrEsc("Address: ");
        if (address == null) { ShowCancelled(); return; }

        string? city = ReadLineOrEsc("City:    ");
        if (city == null) { ShowCancelled(); return; }

        string? country = ReadLineOrEsc("Country: ");
        if (country == null) { ShowCancelled(); return; }

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) ||
            string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
        {
            Console.WriteLine("\nAll fields are required. Press any key to go back...");
            Console.ReadKey();
            return;
        }

        (bool isSucces, string message) result = AirportLogic.AddAirport(name, address, city, country);
        Console.WriteLine($"\n{result.message}");
        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();
    }
}