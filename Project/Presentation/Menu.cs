static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        while (true)
        {
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            Console.WriteLine(@"
  ____       _   _               _                      _    _      _ _                 
 |  _ \ ___ | |_| |_ ___ _ __ __| | __ _ _ __ ___      / \  (_)_ __| (_)_ __   ___  ___ 
 | |_) / _ \| __| __/ _ \ '__/ _` |/ _` | '_ ` _ \    / _ \ | | '__| | | '_ \ / _ \/ __|
 |  _ < (_) | |_| ||  __/ | | (_| | (_| | | | | | |  / ___ \| | |  | | | | | |  __/\__ \
 |_| \_\___/ \__|\__\___|_|  \__,_|\__,_|_| |_| |_| /_/   \_\_|_|  |_|_|_| |_|\___||___/
");

            if (AccountsLogic.CurrentAccount != null)
                Console.WriteLine($"Welcome to Rotterdam Airlines, {AccountsLogic.CurrentAccount.FullName}!\n");
            else
                Console.WriteLine("Welcome to Rotterdam Airlines!\n");

            Console.WriteLine("1: View all available flights");

            if (AccountsLogic.CurrentAccount == null)
            {
                Console.WriteLine("2: Login/Register");
                Console.WriteLine("3: Exit program");
            }
            else
            {
                if (AccountsLogic.CurrentAccount.IsAdmin)
                {
                    Console.WriteLine("2: Book a flight");
                    Console.WriteLine("3: My Bookings");
                    Console.WriteLine("4: My Account");
                    Console.WriteLine("5: Admin Dashboard");
                    Console.WriteLine("6: View All Accounts");
                    Console.WriteLine("7: Exit program");
                }
                else
                {
                    Console.WriteLine("2: Book a flight");
                    Console.WriteLine("3: My Bookings");
                    Console.WriteLine("4: My Account");
                    Console.WriteLine("5: Exit program");
                }
            }

            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();

            if (AccountsLogic.CurrentAccount == null)
            {
                switch (input)
                {
                    case "1":
                        FlightList.ShowAllAvailableFlightsList();
                        break;

                    case "2":
                        AccountMenu();
                        break;

                    case "3":
                        Console.WriteLine("Thank you for using Rotterdam Airlines!\nWe hope to see you again soon!");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input, please try again.");
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                }
            }
            else if (AccountsLogic.CurrentAccount.IsAdmin == false)
            {
                switch (input)
                {
                    case "1":
                        FlightList.ShowAllAvailableFlightsList();
                        break;

                    case "2":
                        FlightSearch.StartSearch();
                        break;

                    case "3":
                        MyBookings.Start();
                        break;

                    case "4":
                        AccountMenu();
                        break;

                    case "5":
                        Console.WriteLine("Thank you for using Rotterdam Airlines!\nWe hope to see you again soon!");
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input, please try again.");
                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                switch (input)
                {
                    case "1":
                        FlightList.ShowAllAvailableFlightsList();
                        break;

                    case "2":
                        FlightSearch.StartSearch();
                        break;

                    case "3":
                        MyBookings.Start();
                        break;

                    case "4":
                        AccountMenu();
                        break;

                    case "5":
                        AdminDashboard.Start();
                        break;

                    case "6":
                        ShowAllAccounts();
                        break;

                    case "7":
                        Console.WriteLine("Thank you for using Rotterdam Airlines!\nWe hope to see you again soon!");
                        Environment.Exit(0);
                        break;

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

    static public void AccountMenu()
    {
        Console.Clear();

        if (AccountsLogic.CurrentAccount != null)
        {
            Console.WriteLine($"Logged in as {AccountsLogic.CurrentAccount.FullName}\n");
            Console.WriteLine("1: Logout");
            Console.WriteLine("2: Go back to the main menu");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AccountsLogic.Logout();
                    Console.Clear();
                    Console.WriteLine("You have been logged out.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;

                case "2":
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
                    AccountMenu();
                    break;
            }
        }
        else
        {
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
                    UserLogin.Start();
                    break;

                case "2":
                    Console.Clear();
                    CreateAccountPresent.AccountCreation();
                    break;

                case "3":
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
                    AccountMenu();
                    break;
            }
        }
    }

    static public void ShowAllAccounts()
    {
        Console.Clear();

        AccountsAccess accountsAccess = new AccountsAccess();

        Console.WriteLine("=== ACCOUNT SEARCH ===");
        Console.WriteLine("Enter a name or email to search.");
        Console.WriteLine("Leave empty to show all accounts.\n");

        string search = Console.ReadLine();

        List<AccountModel> accounts;

        if (string.IsNullOrWhiteSpace(search))
        {
            accounts = accountsAccess.GetAll();
        }
        else
        {
            accounts = accountsAccess.Search(search);
        }

        Console.Clear();
        Console.WriteLine("=== ALL ACCOUNTS ===\n");

        foreach (AccountModel account in accounts)
        {
            Console.WriteLine($"ID: {account.Id}");
            Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
            Console.WriteLine($"Email: {account.EmailAddress}");
            Console.WriteLine($"Phone: {account.PhoneNumber}");
            Console.WriteLine($"Admin: {account.IsAdmin}");
            Console.WriteLine("-----------------------------------");
        }

        Console.WriteLine("\nOptions:");
        Console.WriteLine("D: Delete an account");
        Console.WriteLine("Any other key: Return to menu");

        string choice = Console.ReadLine();

        if (choice != null && choice.ToUpper() == "D")
        {
            Console.WriteLine("\nEnter the ID of the account you want to delete:");
            string idInput = Console.ReadLine();

            if (long.TryParse(idInput, out long accountId))
            {
                if (AccountsLogic.CurrentAccount != null &&
                    AccountsLogic.CurrentAccount.Id == accountId)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nYou cannot delete your own admin account.");
                    Console.ResetColor();
                }
                else
                {
                    AccountModel accountToDelete = accountsAccess.GetById(accountId);

                    if (accountToDelete == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nAccount not found.");
                        Console.ResetColor();
                    }
                    else
                    {
                        accountsAccess.Delete(accountId);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n=================================");
                        Console.WriteLine(" Account deleted successfully! ");
                        Console.WriteLine("=================================");
                        Console.ResetColor();
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nInvalid account ID.");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
}