static class Menu
{

    // All methods in this class are commented out because they call methods that are not implemented yet. 
    // You can uncomment them when you have implemented the corresponding methods.

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Welcome to Rotterdam Airlines!");
        Console.WriteLine("1: View all available flights");
        Console.WriteLine("2: Book a flight");
        Console.WriteLine("3: Login/Register");
        Console.WriteLine("4: Exit program");
        Console.WriteLine("Please enter the number of the option you would like to choose:");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                FlightList.ShowAllFlights();
                break;
            case "2":
                FlightSearch.StartSearch();
                Start();
                break;
            case "3":
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
        }

    }

    static public void AccountMenu()
    {
        Console.WriteLine("Would you like to login to your account or register a new account?");
        Console.WriteLine("1: Login");
        Console.WriteLine("2: Register");
        Console.WriteLine("3: Go back to main menu");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.WriteLine("Login functionality is not implemented yet.");
                break;
            case "2":
                CreateAccountPresent.AccountCreation();
                Start();
                break;
            case "3":
                Start();
                break;
            default:
                Console.WriteLine("Invalid input, please try again.");
                AccountMenu();
                break;
        }
    }
}