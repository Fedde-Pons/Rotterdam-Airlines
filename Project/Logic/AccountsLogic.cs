
//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public static void Logout()
    {
        CurrentAccount = null;
    }

    public AccountModel CheckLogin(string email, string password)
    {


        AccountModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }

    public void CreateAccount(AccountModel account)
    {
        // Validate account details
        if (string.IsNullOrWhiteSpace(account.FirstName) || string.IsNullOrWhiteSpace(account.LastName))
        {
            throw new ArgumentException("First name and last name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(account.EmailAddress) || !account.EmailAddress.Contains("@"))
        {
            throw new ArgumentException("Please enter a valid email address.");
        }

        if (string.IsNullOrWhiteSpace(account.Password) || account.Password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        // Check if email already exists
        AccountModel existingAccount = _access.GetByEmail(account.EmailAddress);
        if (existingAccount != null)
        {
            throw new ArgumentException("An account with this email already exists.");
        }

        // Create the account
        _access.Write(account);
        CurrentAccount = account;
    }
}


