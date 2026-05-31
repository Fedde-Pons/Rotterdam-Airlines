static class AccountDetails
{
    private static AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();

            AccountModel currentAccount = AccountsLogic.CurrentAccount;

            if (currentAccount == null)
            {
                Console.WriteLine("You need to be logged in to view account details.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("=== MY ACCOUNT DETAILS ===\n");
            Console.WriteLine($"Name: {currentAccount.FullName}");
            Console.WriteLine($"Date of birth: {currentAccount.DateOfBirth}");
            Console.WriteLine($"Email: {currentAccount.EmailAddress}");
            Console.WriteLine($"Phone number: {currentAccount.PhoneNumber}");
            Console.WriteLine($"Created at: {currentAccount.CreatedAt}");
            Console.WriteLine($"Admin: {currentAccount.IsAdmin}");

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1: Edit email");
            Console.WriteLine("2: Edit phone number");
            Console.WriteLine("3: Edit password");
            Console.WriteLine("4: Go back");

            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EditEmail();
                    break;

                case "2":
                    EditPhoneNumber();
                    break;

                case "3":
                    EditPassword();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("\nInvalid input.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void EditEmail()
    {
        Console.Clear();

        AccountModel currentAccount = AccountsLogic.CurrentAccount;

        Console.WriteLine("=== EDIT EMAIL ===\n");
        Console.WriteLine($"Current email: {currentAccount.EmailAddress}");
        Console.WriteLine("Enter your new email address:");
        string newEmail = Console.ReadLine();

        try
        {
            accountsLogic.UpdateEmail(newEmail);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEmail updated successfully.");
            Console.ResetColor();
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void EditPhoneNumber()
    {
        Console.Clear();

        AccountModel currentAccount = AccountsLogic.CurrentAccount;

        Console.WriteLine("=== EDIT PHONE NUMBER ===\n");
        Console.WriteLine($"Current phone number: {currentAccount.PhoneNumber}");
        Console.WriteLine("Enter your new phone number:");
        string newPhoneNumber = Console.ReadLine();

        try
        {
            accountsLogic.UpdatePhoneNumber(newPhoneNumber);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPhone number updated successfully.");
            Console.ResetColor();
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void EditPassword()
    {
        Console.Clear();

        Console.WriteLine("=== EDIT PASSWORD ===\n");
        Console.WriteLine("Enter your current password:");
        string currentPassword = ReadPassword();

        Console.WriteLine("\nEnter your new password:");
        string newPassword = ReadPassword();

        Console.WriteLine("\nConfirm your new password:");
        string confirmPassword = ReadPassword();

        try
        {
            accountsLogic.UpdatePassword(currentPassword, newPassword, confirmPassword);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nPassword updated successfully.");
            Console.ResetColor();
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}