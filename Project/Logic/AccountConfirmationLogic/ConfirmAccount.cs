static class ConfirmAccount
{
    public static void ShowConfirmation(AccountModel account, AccountsLogic accountsLogic)
    {
        bool confirming = true;

        while (confirming)
        {
            Console.Clear();
            Console.WriteLine(@"
 ▗▄▖  ▗▄▄▖ ▗▄▄▖ ▗▄▖ ▗▖ ▗▖▗▖  ▗▖▗▄▄▄▖     ▗▄▄▖ ▗▄▖ ▗▖  ▗▖▗▄▄▄▖▗▄▄▄▖▗▄▄▖ ▗▖  ▗▖ ▗▄▖▗▄▄▄▖▗▄▄▄▖ ▗▄▖ ▗▖  ▗▖
▐▌ ▐▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▛▚▖▐▌  █      ▐▌   ▐▌ ▐▌▐▛▚▖▐▌▐▌     █  ▐▌ ▐▌▐▛▚▞▜▌▐▌ ▐▌ █    █  ▐▌ ▐▌▐▛▚▖▐▌
▐▛▀▜▌▐▌   ▐▌   ▐▌ ▐▌▐▌ ▐▌▐▌ ▝▜▌  █      ▐▌   ▐▌ ▐▌▐▌ ▝▜▌▐▛▀▀▘  █  ▐▛▀▚▖▐▌  ▐▌▐▛▀▜▌ █    █  ▐▌ ▐▌▐▌ ▝▜▌
▐▌ ▐▌▝▚▄▄▖▝▚▄▄▖▝▚▄▞▘▝▚▄▞▘▐▌  ▐▌  █      ▝▚▄▄▖▝▚▄▞▘▐▌  ▐▌▐▌   ▗▄█▄▖▐▌ ▐▌▐▌  ▐▌▐▌ ▐▌ █  ▗▄█▄▖▝▚▄▞▘▐▌  ▐▌");

            Console.WriteLine("\n=== Confirm Your Details ===");
            Console.WriteLine($"First name: {account.FirstName}");
            Console.WriteLine($"Last name: {account.LastName}");
            Console.WriteLine($"Email Address: {account.EmailAddress}");
            Console.WriteLine($"Password: {new string('*', account.Password.Length)}");
            Console.WriteLine($"Phone Number: {account.PhoneNumber}");
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
                    CreateAccount(account, accountsLogic);
                    confirming = false;
                    break;
                case "2":
                    EditDetails(account);
                    break;
                case "3":
                    CancelRegistration();
                    confirming = false;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void EditDetails(AccountModel account)
    {
        bool SwitchCaseLoop = true;
        bool LoopValidation = true;

        while (SwitchCaseLoop)
        {
            Console.WriteLine("\n=== Edit Details ===");
            Console.WriteLine($"1. First name: {account.FirstName}");
            Console.WriteLine($"2. Last name: {account.LastName}");
            Console.WriteLine($"3. Email Address: {account.EmailAddress}");
            Console.WriteLine($"4. Password: {new string('*', account.Password.Length)}");
            Console.WriteLine($"5. Phone Number: {account.PhoneNumber}");
            Console.WriteLine($"6. Date Of Birth (dd/mm/yyyy): {account.DateOfBirth}");
            Console.WriteLine($"X. Cancel editing prompt");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "X":
                    Console.Clear();
                    SwitchCaseLoop = false;
                    Console.WriteLine("Exiting editing prompt, press enter to continue...");
                    Console.ReadLine();
                    break;
                case "1":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current first name: {account.FirstName}");
                        Console.Write("Enter new first name: ");
                        string NewFirstName = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.FirstName = NewFirstName;
                            Console.WriteLine($"Successfully updated first name to:\n- {account.FirstName}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for first name, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                case "2":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current last name: {account.LastName}");
                        Console.Write("Enter new last name: ");
                        string NewLastName = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.LastName = NewLastName;
                            Console.WriteLine($"Successfully updated last name to:\n- {account.LastName}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for last name, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                case "3":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current email address: {account.EmailAddress}");
                        Console.Write("Enter new email address: ");
                        string NewEmailAddress = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.EmailAddress = NewEmailAddress;
                            Console.WriteLine($"Successfully updated email address to:\n- {account.EmailAddress}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for email address, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                case "4":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current password: {new string('*', account.Password.Length)}");
                        Console.Write("Enter new password: ");
                        string NewPassword = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.Password = NewPassword;
                            Console.WriteLine($"Successfully updated password to:\n- {new string('*', account.Password.Length)}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for password, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                case "5":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current phone number: {account.PhoneNumber}");
                        Console.Write("Enter new phone number: ");
                        string NewPhoneNumber = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.PhoneNumber = NewPhoneNumber;
                            Console.WriteLine($"Successfully updated phone number to:\n- {account.PhoneNumber}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for phone number, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                case "6":
                    while (LoopValidation)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current date of birth (dd/mm/yyyy): {account.DateOfBirth}");
                        Console.Write("Enter new date of birth (dd/mm/yyyy): ");
                        string NewDateOfBirth = Console.ReadLine();
                        bool Validated = false; // aanroepen validation functie, returned true of false
                        if (Validated) 
                        {
                            LoopValidation = false;
                            account.DateOfBirth = NewDateOfBirth;
                            Console.WriteLine($"Successfully updated date of birth to:\n- {account.DateOfBirth}");
                        }
                        else if (Validated == false)
                        {
                            Console.WriteLine($"Invalid entry for date of birth, press enter to retry...");
                            Console.ReadLine();
                        }
                        
                    }
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void CreateAccount(AccountModel account, AccountsLogic accountsLogic)
    {
        try
        {
            accountsLogic.CreateAccount(account);
            // hierzo logica om de account te creeeren in de database
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

    private static void CancelRegistration()
    {
        Console.WriteLine("\nRegistration cancelled. Returning to menu...");
        Console.WriteLine("Press enter to continue...");
        Console.ReadLine();
    }
}
