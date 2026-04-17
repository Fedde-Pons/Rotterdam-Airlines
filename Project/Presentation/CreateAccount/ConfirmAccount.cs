
static class ConfirmAccount
{
    private static string _RepeatingConfirmSentence = @"
 ▗▄▖  ▗▄▄▖ ▗▄▄▖ ▗▄▖ ▗▖ ▗▖▗▖  ▗▖▗▄▄▄▖     ▗▄▄▖ ▗▄▖ ▗▖  ▗▖▗▄▄▄▖▗▄▄▄▖▗▄▄▖ ▗▖  ▗▖ ▗▄▖▗▄▄▄▖▗▄▄▄▖ ▗▄▖ ▗▖  ▗▖
▐▌ ▐▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▛▚▖▐▌  █      ▐▌   ▐▌ ▐▌▐▛▚▖▐▌▐▌     █  ▐▌ ▐▌▐▛▚▞▜▌▐▌ ▐▌ █    █  ▐▌ ▐▌▐▛▚▖▐▌
▐▛▀▜▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▌ ▝▜▌  █      ▐▌   ▐▌ ▐▌▐▌ ▝▜▌▐▛▀▀▘  █  ▐▛▀▚▖▐▌  ▐▌▐▛▀▜▌ █    █  ▐▌ ▐▌▐▌ ▝▜▌
▐▌ ▐▌▝▚▄▄▖▝▚▄▄▖▝▚▄▞▘▝▚▄▞▘▐▌  ▐▌  █      ▝▚▄▄▖▝▚▄▞▘▐▌  ▐▌▐▌   ▗▄█▄▖▐▌ ▐▌▐▌  ▐▌▐▌ ▐▌ █  ▗▄█▄▖▝▚▄▞▘▐▌  ▐▌";
    public static void ShowConfirmation(AccountModel account, CreateAccountLogic logic)
    {
        bool confirming = true;



        while (confirming)
        {
            Console.Clear();
            Console.WriteLine(_RepeatingConfirmSentence);

            Console.WriteLine("\n=== Confirm Your Details ===");
            Console.WriteLine($"First name: {account.FirstName}");
            Console.WriteLine($"Last name: {account.LastName}");
            Console.WriteLine($"Email Address: {account.EmailAddress}");
            Console.WriteLine($"Password: {new string('*', account.Password.Length)}");
            Console.WriteLine($"Phone Number: +{account.PhoneNumber}");
            Console.WriteLine($"Date Of Birth (dd/mm/yyyy): {account.DateOfBirth}");

            
            Console.WriteLine("\n=== Options ===");
            Console.WriteLine("1. Confirm and create account");
            Console.WriteLine("2. Edit details");
            Console.WriteLine("3. Cancel registration");
            
            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    CreateAccount(account, logic);
                    confirming = false;
                    break;
                case "2":
                    EditDetails(account, logic);
                    break;
                case "3":
                    confirming = CancelRegistration();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void EditDetails(AccountModel account, CreateAccountLogic logic)
    {
        bool SwitchCaseLoop = true;

        while (SwitchCaseLoop)
        {
            Console.Clear();
            Console.WriteLine(_RepeatingConfirmSentence);
            bool LoopValidation = true;

            Console.WriteLine();
            Console.WriteLine("\n=== Edit Details ===");
            Console.WriteLine($"1. First name: {account.FirstName}");
            Console.WriteLine($"2. Last name: {account.LastName}");
            Console.WriteLine($"3. Email Address: {account.EmailAddress}");
            Console.WriteLine($"4. Password: {new string('*', account.Password.Length)}");
            Console.WriteLine($"5. Phone Number: {account.PhoneNumber}");
            Console.WriteLine($"6. Date Of Birth (dd/mm/yyyy): {account.DateOfBirth}");
            Console.WriteLine($"X. Cancel editing prompt");

            string choice = Console.ReadLine().ToUpper();

            switch (choice)
            {
                case "X":
                    Console.Clear();
                    SwitchCaseLoop = false;
                    Console.WriteLine(_RepeatingConfirmSentence);
                    Console.WriteLine("Exiting editing prompt");
                    break;
                case "1":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current first name: {account.FirstName}");
                        Console.Write("Enter new first name: ");
                        string NewFirstName = Console.ReadLine();
                        bool Validated = logic.ValidateFirstName(NewFirstName);
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.FirstName = NewFirstName;
                            Console.WriteLine($"Successfully updated first name to:\n- {account.FirstName}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid entry for first name");
                        }
                        
                    }
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current last name: {account.LastName}");
                        Console.Write("Enter new last name: ");
                        string NewLastName = Console.ReadLine();
                        bool Validated = logic.ValidateLastName(NewLastName);
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.LastName = NewLastName;
                            Console.WriteLine($"Successfully updated last name to:\n- {account.LastName}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid entry for last name");
                        }
                        
                    }
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current email address: {account.EmailAddress}");
                        Console.Write("Enter new email address: ");
                        string NewEmailAddress = Console.ReadLine();
                        bool Validated = logic.ValidateEmailAddress(NewEmailAddress);
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.EmailAddress = NewEmailAddress.ToLower();
                            Console.WriteLine($"Successfully updated email address to:\n- {account.EmailAddress}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid entry for email address");
                        }
                        
                    }
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current password: {new string('*', account.Password.Length)}");
                        Console.Write("Enter new password: ");
                        string NewPassword = Console.ReadLine();

                        Console.WriteLine();
                        Console.Write("Verify new password: ");
                        string VerifyNewPassword = Console.ReadLine();
                        if (NewPassword == VerifyNewPassword)
                        {
                            bool Validated = logic.ValidatePassword(NewPassword);
                            if (Validated) 
                            {
                                LoopValidation = false;
                                account.Password = NewPassword;
                                Console.WriteLine($"Successfully updated password to:\n- {new string('*', account.Password.Length)}");
                            }
                            else
                            {
                                Console.WriteLine($"Invalid entry for password");
                            }
                        }
                        else { Console.WriteLine("Passwords don't match, please retry"); }
                        
                    }
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current phone number: {account.PhoneNumber}");
                        Console.Write("Enter your country code (e.g., 1 for USA, 31 for Netherlands) or X: +");
                        string countryCode = Console.ReadLine();
                        Console.Write($"Enter your phone number or X: +{countryCode} ");
                        string NewPhoneNumber = Console.ReadLine();
                        bool Validated = logic.ValidatePhoneNumber(countryCode, NewPhoneNumber);
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.PhoneNumber = $"{countryCode} {NewPhoneNumber}";
                            Console.WriteLine($"Successfully updated phone number to:\n- {account.PhoneNumber}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid entry for phone number");
                        }
                        
                    }
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine(_RepeatingConfirmSentence);
                    while (LoopValidation)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Current date of birth (dd/mm/yyyy): {account.DateOfBirth}");
                        Console.Write("Enter new date of birth (dd/mm/yyyy): ");
                        string stringNewDateOfBirth = Console.ReadLine();
                        bool Validated = logic.ValidateDateOfBirth(stringNewDateOfBirth);
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.DateOfBirth = stringNewDateOfBirth;
                            Console.WriteLine($"Successfully updated date of birth to:\n- {account.DateOfBirth}");
                        }
                        else
                        {
                            Console.WriteLine($"Invalid entry for date of birth. Please follow the format");
                        }
                        
                    }
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please choose a valid option from the menu.");
                    break;
            }
        }
    }

    private static void CreateAccount(AccountModel account, CreateAccountLogic logic)
    {
        try
        {
            logic.CreateAccount(account);
            Console.WriteLine(@"
 ▗▄▖  ▗▄▄▖ ▗▄▄▖ ▗▄▖ ▗▖ ▗▖▗▖  ▗▖▗▄▄▄▖     ▗▄▄▖▗▄▄▖ ▗▄▄▄▖ ▗▄▖▗▄▄▄▖▗▄▄▄▖▗▄▄▄ 
▐▌ ▐▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▛▚▖▐▌  █      ▐▌   ▐▌ ▐▌▐▌   ▐▌ ▐▌ █  ▐▌   ▐▌  █
▐▛▀▜▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▌ ▝▜▌  █      ▐▌   ▐▛▀▚▖▐▛▀▀▘▐▛▀▜▌ █  ▐▛▀▀▘▐▌  █
▐▌ ▐▌▝▚▄▄▖▝▚▄▄▖▝▚▄▞▘▝▚▄▞▘▐▌  ▐▌  █      ▝▚▄▄▖▐▌ ▐▌▐▙▄▄▖▐▌ ▐▌ █  ▐▙▄▄▖▐▙▄▄▀                                                                   
");
            Console.WriteLine("\n✓ Account created successfully!");
            Console.WriteLine($"Welcome, {account.FullName}!");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Error creating account: {ex.Message}");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }

    private static bool CancelRegistration()
    {
        Console.Clear();
        Console.WriteLine(_RepeatingConfirmSentence);
        Console.WriteLine();
        while (true)
        {
            Console.WriteLine("Opt out of account creation process?\nY/N");
            string Response1 = (Console.ReadLine() ?? "").ToUpper();
            if (Response1 == "N") {return false;}
            else if (Response1 == "Y")
            {
                Console.WriteLine("Are you sure you want to stop? \nCreating an account only takes a minute and gives you full access to discounts and member benefits.\n\nYour progress will be lost.\nY/N");
                string Response2 = (Console.ReadLine() ?? "").ToUpper();
                if (Response2 == "Y") 
                {
                    Console.Clear();
                    Console.WriteLine("We're sorry to see you going, you can always sign up and still earn membership benefits!");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    return true;
                }
                return false;
            }
            else { Console.WriteLine("Invalid Entry, try again."); }
        }
    }
}