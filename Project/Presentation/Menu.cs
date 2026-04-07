static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.Clear();
        Console.WriteLine(@"
  ____       _   _               _                      _    _      _ _                 
 |  _ \ ___ | |_| |_ ___ _ __ __| | __ _ _ __ ___      / \  (_)_ __| (_)_ __   ___  ___ 
 | |_) / _ \| __| __/ _ \ '__/ _` |/ _` | '_ ` _ \    / _ \ | | '__| | | '_ \ / _ \/ __|
 |  _ < (_) | |_| ||  __/ | | (_| | (_| | | | | | |  / ___ \| | |  | | | | | |  __/\__ \
 |_| \_\___/ \__|\__\___|_|  \__,_|\__,_|_| |_| |_| /_/   \_\_|_|  |_|_|_| |_|\___||___/
");
        Console.WriteLine("Welcome to Rotterdam Airlines!\n");
        Console.WriteLine("1: View all available flights");
        Console.WriteLine("2: Book a flight");
        Console.WriteLine("3: Login/Register");
        Console.WriteLine("4: Exit program");
        Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                FlightList.ShowAllAvailableFlightsList();
                break;
            case "2":
                FlightSearch.StartSearch();
                Start();
                break;
            case "3":
                AccountMenu();
                break;
            case "4":
                Console.WriteLine("Thank you for using Rotterdam Airlines!\nWe hope to see you again soon!");
                Environment.Exit(0);
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                Start();
                break;
        }

    }

    static public void AccountMenu()
    {
        Console.Clear();
        Console.WriteLine("Would you like to login to your account or register a new account?");
        Console.WriteLine("1: Login");
        Console.WriteLine("2: Register");
        Console.WriteLine("3: Go back to the main menu");
        Console.WriteLine("\nPlease enter the number of the option you would like to choose:");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.Clear();
                Console.WriteLine("Login functionality is not implemented yet.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                AccountMenu();
                break;
            case "2":
                Console.Clear();
                CreateAccountPresent.AccountCreation();
                Start();
                break;
            case "3":
                Start();
                break;
            default:
                Console.Clear();
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                AccountMenu();
                break;
        }
    }
}