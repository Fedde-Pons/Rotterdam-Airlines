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
        Console.Clear();
        Console.WriteLine("=== All Bookings ===\n");

        List<BookingModel> bookings = new BookingAccess().GetAll().Where(x => x.Status == "pending").ToList();

        if (bookings.Count == 0)
        {
            Console.WriteLine("No bookings found.");
            Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
            Console.ReadKey();

        }
        else
        {
            Console.WriteLine($"{"ID",-6} {"Account ID",-12} {"Date",-22} {"Total Price",-14} {"Status"}");
            Console.WriteLine(new string('-', 70));
            int count = 0;
            foreach (BookingModel booking in bookings)
            {
                Console.WriteLine($"{booking.Id,-6} {booking.AccountId,-12} {booking.Date,-22} {"€" + booking.TotalPrice.ToString("F2"),-14} {booking.Status}");
            }
            Console.WriteLine("select 1 [id] to edit status by id\n");
            string? input = Console.ReadLine();
            string[] param = input.Split(" ");
            if (param.Length == 1)
            {
                Console.WriteLine("No bookings found.");
                Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
                Console.ReadKey();
            }
            else if (param.Length == 2)
            {
                if (int.TryParse(param[1], out int id))
                {
                    if (bookings.FirstOrDefault(x => x.Id == id) != null)
                    {
                        EditBookingStatus(bookings, id);
                    }
                    else
                    {
                        Console.WriteLine("something went wrong with id selection");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("please only put numbers in this forum");
                }

            }
            else
            {

            }
        }

    }

    private static void ShowAccounts()
    {
        Console.Clear();
        Console.WriteLine("=== All Accounts ===\n");

        List<AccountModel> accounts = new AccountsAccess().GetAll();

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
        Console.Clear();
        Console.WriteLine("=== Airports ===\n");
        Console.WriteLine("Airport management is not yet implemented.");
        Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
        Console.ReadKey();
    }

    private static void EditBookingStatus(List<BookingModel> bookings, int id)
    {
        BookingModel result = bookings.First(x => x.Id == id);
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"current status of booking with {result.Id}: {result.Status}");
            Console.WriteLine("+---------------------------+");
            Console.WriteLine("| number |  change status to|");
            Console.WriteLine("+---------------------------+");
            Console.WriteLine("| 1      |          pending |");
            Console.WriteLine("| 2      |         complete |");
            Console.WriteLine("| 3      |         canceled |");
            Console.WriteLine("+---------------------------+");
            Console.WriteLine("| press 0 to cancel and     |");
            Console.WriteLine("| return to admin screen    |");
            Console.WriteLine("+---------------------------+ \n");
            Console.Write(">");
            string input = Console.ReadLine();
            switch (input)
            {
                // pending
                case "1":
                    BookingLogic.EditBookingStatus(result, "pending");
                    return;
                // complete
                case "2":
                    BookingLogic.EditBookingStatus(result, "complete");
                    return;
                //cancel
                case "3":
                    BookingLogic.EditBookingStatus(result, "canceled");
                    return;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}