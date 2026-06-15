using System.Globalization;

public static class CreateAccountPresent
{
    private static CreateAccountLogic _logic = new();
    private static bool CancelOperationEntirely = false;

    private static string _repeatingSentence = @"
 ▗▄▄▖▗▄▄▖ ▗▄▄▄▖ ▗▄▖▗▄▄▄▖▗▄▄▄▖     ▗▄▖  ▗▄▄▖ ▗▄▄▖ ▗▄▖ ▗▖ ▗▖▗▖  ▗▖▗▄▄▄▖
▐▌   ▐▌ ▐▌▐▌   ▐▌ ▐▌ █  ▐▌       ▐▌ ▐▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▛▚▖▐▌  █  
▐▌   ▐▛▀▚▖▐▛▀▀▘▐▛▀▜▌ █  ▐▛▀▀▘    ▐▛▀▜▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▌ ▝▜▌  █  
▝▚▄▄▖▐▌ ▐▌▐▙▄▄▖▐▌ ▐▌ █  ▐▙▄▄▖    ▐▌ ▐▌▝▚▄▄▖▝▚▄▄▖▝▚▄▞▘▝▚▄▞▘▐▌  ▐▌  █                       
                                                                     
Fields marked with * are mandatory.
If you don't want to enter optional information, enter X.
Press Esc to quit.
";

    public static void AccountCreation()
    {
        CancelOperationEntirely = false;

        string firstName = "";
        string lastName = "";
        string email = "";
        string password = "";
        string phoneNumber = "";
        string dateOfBirth = "";

        Console.Clear();
        Console.WriteLine(_repeatingSentence);

        if (!CancelOperationEntirely)
        {
            firstName = PromptForFirstName();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured first name!");
            lastName = PromptForLastName();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured last name!");
            email = PromptForEmail();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured email!");
            password = PromptForPassword();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured password!");
            phoneNumber = PromptForPhoneNumber();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured phone number!");
            dateOfBirth = PromptForDateOfBirth();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Successfully captured birthdate!");
            Console.WriteLine();
            Console.WriteLine("Press enter to proceed to account confirmation...");
            Console.ReadLine();

            AccountModel accountModel = new(firstName, lastName, email, phoneNumber, password, dateOfBirth);
            ConfirmAccount.ShowConfirmation(accountModel, _logic);
        }
    }

    private static string PromptForFirstName()
    {
        while (true)
        {
            string firstName = ReadInputWithEscape("\nEnter your first name *: ");

            if (CancelOperationEntirely) return "";

            if (_logic.ValidateFirstName(firstName))
            {
                return firstName;
            }

            Console.WriteLine();
            Console.WriteLine("=== INVALID ENTRY ===");
            Console.WriteLine("Invalid first name. First name cannot contain spaces or numbers.");
            Console.WriteLine();
            Console.WriteLine("=== RETRY ===");
            Console.WriteLine();
        }
    }

    private static string PromptForLastName()
    {
        while (true)
        {
            string lastName = ReadInputWithEscape("\nEnter your last name *: ");

            if (CancelOperationEntirely) return "";

            if (_logic.ValidateLastName(lastName))
            {
                return lastName;
            }

            Console.WriteLine();
            Console.WriteLine("=== INVALID ENTRY ===");
            Console.WriteLine("Invalid last name. Last name cannot contain numbers.");
            Console.WriteLine();
            Console.WriteLine("=== RETRY ===");
            Console.WriteLine();
        }
    }

    private static string PromptForEmail()
    {
        while (true)
        {
            string email = ReadInputWithEscape("\nEnter your email address *: ");

            if (CancelOperationEntirely) return "";

            if (_logic.ValidateEmailAddress(email))
            {
                return email.ToLower();
            }

            Console.WriteLine();
            Console.WriteLine("=== INVALID ENTRY ===");
            Console.WriteLine("Invalid email address. Email must contain @ and .");
            Console.WriteLine();
            Console.WriteLine("=== RETRY ===");
            Console.WriteLine();
        }
    }

    private static string PromptForPassword()
    {
        while (true)
        {
            Console.Write("\nEnter your password *: ");
            string password = ReadPassword();

            if (CancelOperationEntirely) return "";

            if (_logic.ValidatePassword(password))
            {
                Console.Write("\nVerify your password *: ");
                string verifyPassword = ReadPassword();

                if (CancelOperationEntirely) return "";

                if (verifyPassword == password)
                {
                    return password;
                }

                Console.WriteLine();
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Password does not match, retry");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid password. Requirements: 8-20 characters, at least one uppercase letter, one lowercase letter, one number, one special character (!@#$%^&*), and no spaces.");
                Console.WriteLine();
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine();
            }
        }
    }

    private static string PromptForPhoneNumber()
    {
        while (true)
        {
            string countryCode = ReadInputWithEscape("\nEnter your country code (e.g., 1 for USA, 31 for Netherlands) or X: +");

            if (CancelOperationEntirely) return "";

            string phoneNumber = ReadInputWithEscape($"Enter your phone number or X: +{countryCode} ");

            if (CancelOperationEntirely) return "";

            if (_logic.ValidatePhoneNumber(countryCode, phoneNumber))
            {
                return $"{countryCode} {phoneNumber}";
            }

            Console.WriteLine();
            Console.WriteLine("=== INVALID ENTRY ===");
            Console.WriteLine("Invalid phone number. Please enter a valid country code and phone number (6-15 digits).");
            Console.WriteLine();
            Console.WriteLine("=== RETRY ===");
            Console.WriteLine();
        }
    }

    private static string PromptForDateOfBirth()
    {
        while (true)
        {
            string dateOfBirth = ReadInputWithEscape("\nEnter your date of birth (dd/mm/yyyy) *: ");

            if (CancelOperationEntirely) return "";

            if (_logic.ValidateDateOfBirth(dateOfBirth))
            {
                return dateOfBirth;
            }

            Console.WriteLine();
            Console.WriteLine("=== INVALID ENTRY ===");
            Console.WriteLine("Invalid date of birth. Format must be dd/mm/yyyy and year must be 1909 or later.");
            Console.WriteLine();
            Console.WriteLine("=== RETRY ===");
            Console.WriteLine();
        }
    }

    private static string ReadInputWithEscape(string prompt)
    {
        Console.Write(prompt);

        string input = "";

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                ConfirmQuit();

                if (CancelOperationEntirely)
                {
                    return "";
                }

                Console.Write(prompt);
                input = "";
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return input;
            }
            else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input = input[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }
    }

    private static string ReadPassword()
    {
        string password = "";

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                ConfirmQuit();

                if (CancelOperationEntirely)
                {
                    return "";
                }

                password = "";
                Console.Write("\nTry again: ");
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return password;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
    }

    private static void ConfirmQuit()
    {
        Console.WriteLine("Are you sure you want to stop?\n\nYour progress will be lost.\nY/N");

        string response = (Console.ReadLine() ?? "").ToUpper();

        if (response == "Y")
        {
            Console.Clear();
            Console.WriteLine("Registration cancelled.");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            CancelOperationEntirely = true;
        }
    }
}