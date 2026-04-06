static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to do something else in the future");

        string input = Console.ReadLine();
        if (input == "1")
        {
<<<<<<< Updated upstream
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
=======
            case "1":
                // This calls the method to show all available flights.
                // This function is implemented in the "feature/show-available-flights" branch

                FlightList.ShowAllFlights();
                break;
            case "2":
                // This calls the method to book a flight (not implemented yet)
                // Order: 1. Show all flights with filter logic, 2. Show all flight details of a selected flight, 3. Book a flight
                FlightSearch.StartSearch();
                Start();
                break;
            case "3":
                // This calls the method to show the account menu where you can choose to login or register.
                AccountMenu();
                break;
            case "4":
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input, please try again.");
                Start();
                break;
>>>>>>> Stashed changes
        }

    }
}