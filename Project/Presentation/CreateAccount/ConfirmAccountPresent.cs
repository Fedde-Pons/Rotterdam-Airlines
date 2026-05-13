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
                                                                     
Fields with a * are mandatory,
if you don't want to enter the specific information enter X if not mandatory.
Enter Q to quit. 
";
    public static void AccountCreation()
    {   
        // elke keer dat het in deze method gaat, is het true als je al 1x hebt gedaan, dus hiermee herstel je het

        CancelOperationEntirely = false;

        string firstName = "";
        string lastName = "";
        string email = "";
        string password = "";
        string phoneNumber = "";
        string dateOfBirth = "";

        Console.Clear();
        Console.WriteLine(_repeatingSentence);
        Console.WriteLine();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine();
            // Prompt for first name
            firstName = PromptForFirstName();
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Succesfully captured first name!");
            if (!CancelOperationEntirely)
            {
                // Prompt for last name
                lastName = PromptForLastName();
            }
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Succesfully captured last name!");
            if (!CancelOperationEntirely)
            {
                // Prompt for email
                email = PromptForEmail();
            }
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Succesfully captured email!");
            if (!CancelOperationEntirely)
            {
                // Prompt for password
                password = PromptForPassword();
            }
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Succesfully captured password!");
            if (!CancelOperationEntirely)
            {
                // Prompt for phone number
                phoneNumber = PromptForPhoneNumber();
            }
        }

        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine("Succesfully captured phone number");
            if (!CancelOperationEntirely)
            {
                // Prompt for date of birth
                dateOfBirth = PromptForDateOfBirth();
            }
        }
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);        
            Console.WriteLine("Succesfully captured birthdate!");
            Console.WriteLine();
            Console.WriteLine("Press enter to proceed to account confirmation...");
            Console.ReadLine();
        }

        if (!CancelOperationEntirely)
        {
            
            AccountModel accountModel = new(firstName, lastName, email, phoneNumber, password, dateOfBirth);
            // go through confirm mechanism by calling ShowConfirmation from ConfirmAccount static class
            ConfirmAccount.ShowConfirmation(accountModel, _logic);
        }

        // implementeren go back to main menu 
        // als t goed is moet je eig hierzo nix doen en gaat de script verder in de main menu

    }

    private static string PromptForFirstName()
    {

        while (true)
        {
            Console.Write("* \nEnter your first name: ");
            string firstName = Console.ReadLine() ?? "";

            if (firstName.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidateFirstName(firstName))
            {
                return firstName;
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid first name. First name cannot contain spaces or numbers.");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }
        }
    }

    private static string PromptForLastName()
    {
        while (true)
        {
            Console.Write("* \nEnter your last name: ");
            string lastName = Console.ReadLine() ?? "";

            if (lastName.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidateLastName(lastName))
            {
                return lastName;
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid last name. Last name cannot contain numbers.");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }
        }
    }

    private static string PromptForEmail()
    {
        while (true)
        {
            Console.Write("*\nEnter your email address: ");
            string email = Console.ReadLine() ?? "";

            if (email.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidateEmailAddress(email))
            {
                return email.ToLower();
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid email address. Email must contain @ and .");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }
        }
    }
    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        while (true)
        {
            key = Console.ReadKey(true); // true = don't display the key

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1); // remove last char
                Console.Write("\b \b");   // remove last *
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        return password;
    }
    private static string PromptForPassword()
    {
        while (true)
        {
            Console.Write("*\nEnter your password: ");
            string password = ReadPassword();

            if (password.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidatePassword(password))
            {
                Console.Write("*\nVerify your password: ");
                string verifyPassword = ReadPassword();
                
                if (verifyPassword.ToUpper() == "Q")
                {
                    ConfirmQuit();
                    if (CancelOperationEntirely) return "";
                }
                else if (verifyPassword == password) { return password; }
                else 
                { 
                    Console.WriteLine("");
                    Console.WriteLine("=== INVALID ENTRY ===");
                    Console.WriteLine("Password does not match, retry");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid password. Requirements: 8-20 characters, at least one uppercase letter, one lowercase letter, \none number, one special character (!@#$%^&*), and no spaces.");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }


        }
    }

    private static string PromptForPhoneNumber()
    {
        while (true)
        {
            Console.Write("Enter your country code (e.g., 1 for USA, 31 for Netherlands) or X: +");
            string countryCode = Console.ReadLine() ?? "";
    
            if (countryCode.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }

            Console.Write($"Enter your phone number or X: +{countryCode} ");
            string phoneNumber = Console.ReadLine() ?? "";

            if (phoneNumber.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidatePhoneNumber(countryCode, phoneNumber))
            {
                return $"{countryCode} {phoneNumber}";
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid phone number. Please enter a valid country code and phone number (6-15 digits).");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }
        }
    }

    private static string PromptForDateOfBirth()
    {
        while (true)
        {
            Console.Write("*\nEnter your date of birth (dd/mm/yyyy): ");
            string dateOfBirth = Console.ReadLine() ?? "";

            if (dateOfBirth.ToUpper() == "Q")
            {
                ConfirmQuit();
                if (CancelOperationEntirely) return "";
            }
            else if (_logic.ValidateDateOfBirth(dateOfBirth))
            {
                return dateOfBirth;
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("=== INVALID ENTRY ===");
                Console.WriteLine("Invalid date of birth. Format must be dd/mm/yyyy and year must be 1909 or later.");
                Console.WriteLine("");
                Console.WriteLine("=== RETRY ===");
                Console.WriteLine("");
            }
        }
    }

    private static void ConfirmQuit()
    {
        Console.WriteLine("Are you sure you want to stop? \nCreating an account only takes a minute and gives you full access to discounts and member benefits.\n\nYour progress will be lost.\nY/N");
        string Response2 = (Console.ReadLine() ?? "").ToUpper();
        if (Response2 == "Y") 
        {
            Console.Clear();
            Console.WriteLine("We're sorry to see you going, you can always sign up and still earn membership benefits!");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            CancelOperationEntirely = true;
        }
    }


}
