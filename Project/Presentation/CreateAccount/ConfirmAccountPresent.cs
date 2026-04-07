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
                                                                     
Fields with a * are mandatory,\nif you don't want to enter the specific information enter X if not mandatory.";
    public static void AccountCreation()
    {   
        // elke keer dat het in deze method gaat, is het true als je al 1x hebt gedaan, dus hiermee herstel je het

        CancelOperationEntirely = false;
        
        bool CancelOperation = false;

        string firstName = "";
        string lastName = "";
        string email = "";
        string password = "";
        string phoneNumber = "";
        string dateOfBirth = "";

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            Console.WriteLine();
            // Prompt for first name
            firstName = PromptForFirstName();
            Console.WriteLine("Succesfully captured first name, press enter to continue...");
            Console.ReadLine();
        }

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            // Prompt for last name
            lastName = PromptForLastName();
            Console.WriteLine("Succesfully captured last name, press enter to continue...");
            Console.ReadLine();
        }

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            // Prompt for email
            email = PromptForEmail();
            Console.WriteLine("Succesfully captured email, press enter to continue...");
            Console.ReadLine();
        }

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            // Prompt for password
            password = PromptForPassword();
            Console.WriteLine("Succesfully captured password, press enter to continue...");
            Console.ReadLine();
        }

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            // Prompt for phone number
            phoneNumber = PromptForPhoneNumber();
            Console.WriteLine("Succesfully captured phone number, press enter to continue...");
            Console.ReadLine();
        }

        CancelOperation = PromptToCancel();
        if (!CancelOperationEntirely)
        {
            Console.Clear();
            Console.WriteLine(_repeatingSentence);
            // Prompt for date of birth
            dateOfBirth = PromptForDateOfBirth();
            Console.WriteLine("Succesfully captured birthdate, press enter to continue...");
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
            string firstName = Console.ReadLine();

            if (_logic.ValidateFirstName(firstName))
            {
                return firstName;
            }
            else
            {
                Console.WriteLine("Invalid first name. First name cannot contain spaces or numbers.");
            }
        }
    }

    private static string PromptForLastName()
    {
        while (true)
        {
            Console.Write("* \nEnter your last name: ");
            string lastName = Console.ReadLine();

            if (_logic.ValidateLastName(lastName))
            {
                return lastName;
            }
            else
            {
                Console.WriteLine("Invalid last name. Last name cannot contain numbers.");
            }
        }
    }

    private static string PromptForEmail()
    {
        while (true)
        {
            Console.Write("*\nEnter your email address: ");
            string email = Console.ReadLine();

            if (_logic.ValidateEmailAddress(email))
            {
                return email;
            }
            else
            {
                Console.WriteLine("Invalid email address. Email must contain @ and .");
            }
        }
    }

    private static string PromptForPassword()
    {
        while (true)
        {
            Console.Write("*\nEnter your password: ");
            string password = Console.ReadLine();

            if (_logic.ValidatePassword(password))
            {
                return password;
            }
            else
            {
                Console.WriteLine("Invalid password. Requirements: 8-20 characters, at least one uppercase letter, one lowercase letter, one number, one special character (!@#$%^&*), and no spaces.");
            }
        }
    }

    private static string PromptForPhoneNumber()
    {
        while (true)
        {
            Console.Clear();
            Console.Write("Enter your country code (e.g., 1 for USA, 31 for Netherlands) or X: +");
            string countryCode = Console.ReadLine();
    
            Console.Write($"Enter your phone number or X: +{countryCode} ");
            string phoneNumber = Console.ReadLine();

            if (_logic.ValidatePhoneNumber(countryCode, phoneNumber))
            {
                return $"{countryCode} {phoneNumber}";
            }
            else
            {
                Console.WriteLine("Invalid phone number. Please enter a valid country code and phone number (6-15 digits).");
            }
        }
    }

    private static string PromptForDateOfBirth()
    {
        while (true)
        {
            Console.Write("*\nEnter your date of birth (dd/mm/yyyy): ");
            string dateOfBirth = Console.ReadLine();

            if (_logic.ValidateDateOfBirth(dateOfBirth))
            {
                return dateOfBirth;
            }
            else
            {
                Console.WriteLine("Invalid date of birth. Format must be dd/mm/yyyy and year must be 1909 or later.");
            }
        }
    }

    private static bool PromptToCancel()
    {

        Console.WriteLine("Opt out of account creation process?\nY/N");
        string Response1 = (Console.ReadLine() ?? "").ToUpper();
        if (Response1 == "N") {return false;}

        Console.WriteLine("Are you sure you want to stop? \nCreating an account only takes a minute and gives you full access to discounts and member benefits.\n\nYour progress will be lost.");
        string Response2 = (Console.ReadLine() ?? "").ToUpper();
        if (Response2 == "N") 
        {
            Console.Clear();
            Console.WriteLine("We're sorry to see you going, you can always sign up and still earn membership benefits!");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            CancelOperationEntirely = true;
            return false;
        }
        return true;
    }


}