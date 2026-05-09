static class AdminDashboard
{
    public static void Start()
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
                FlightList.ShowAllAvailableFlightsList();
                break;
            case "2":
                ShowAirports();
                break;
            case "3":
                ShowBookings();
                break;
            case "4":
                ShowAccounts();
                break;
            case "5":
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine("Press any key to return to the Admin Dashboard...");
                Console.ReadKey();
                Start();
                break;
        }
    }

    private static void ShowBookings()
    {
        Console.Clear();
        Console.WriteLine("=== All Bookings ===\n");

        var bookings = new BookingAccess().GetAll();

        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found.");
        }
        else
        {
            Console.WriteLine($"{"ID",-6} {"Account ID",-12} {"Date",-22} {"Total Price",-14} {"Status"}");
            Console.WriteLine(new string('-', 70));
            foreach (var b in bookings)
            {
                Console.WriteLine($"{b.Id,-6} {b.AccountId,-12} {b.Date,-22} {"€" + b.TotalPrice.ToString("F2"),-14} {b.Status}");
            }
        }

        Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
        Console.ReadKey();
        Start();
    }

    private static void ShowAccounts()
    {
        Console.Clear();
        Console.WriteLine("=== All Accounts ===\n");

        var accounts = new AccountsAccess().GetAll();

        Console.WriteLine($"{"ID",-6} {"Name",-24} {"Email",-30} {"Phone",-16} {"Admin"}");
        Console.WriteLine(new string('-', 85));
        foreach (var a in accounts)
        {
            Console.WriteLine($"{a.Id,-6} {a.FullName,-24} {a.EmailAddress,-30} {a.PhoneNumber,-16} {(a.IsAdmin ? "Yes" : "No")}");
        }

        Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
        Console.ReadKey();
        Start();
    }

    private static void ShowAirports()
    {
        Console.Clear();
        Console.WriteLine("=== Airports ===\n");
        Console.WriteLine("Airport management is not yet implemented.");
        Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
        Console.ReadKey();
        Start();
    }
}
