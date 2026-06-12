// This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{
    // Static properties are shared across all instances of the class
    // This can be used to get the current logged in account from anywhere in the program
    // private set, so this can only be set by the class itself
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

    public List<AccountModel> GetAll()
    {
        return _access.GetAll();
    }

    public AccountModel CheckLogin(string email, string password)
    {
        AccountModel acc = _access.GetByEmail(email.ToLower());

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

    public void UpdateEmail(string newEmail)
    {
        if (CurrentAccount == null)
        {
            throw new ArgumentException("You need to be logged in.");
        }

        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains("@"))
        {
            throw new ArgumentException("Please enter a valid email address.");
        }

        AccountModel existingAccount = _access.GetByEmail(newEmail);

        if (existingAccount != null && existingAccount.Id != CurrentAccount.Id)
        {
            throw new ArgumentException("An account with this email already exists.");
        }

        CurrentAccount.EmailAddress = newEmail.ToLower();
        _access.Update(CurrentAccount);
    }

    public void UpdatePhoneNumber(string newPhoneNumber)
    {
        if (CurrentAccount == null)
        {
            throw new ArgumentException("You need to be logged in.");
        }

        if (string.IsNullOrWhiteSpace(newPhoneNumber))
        {
            throw new ArgumentException("Phone number cannot be empty.");
        }

        CurrentAccount.PhoneNumber = newPhoneNumber;
        _access.Update(CurrentAccount);
    }

    public void UpdatePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        if (CurrentAccount == null)
        {
            throw new ArgumentException("You need to be logged in.");
        }

        if (CurrentAccount.Password != currentPassword)
        {
            throw new ArgumentException("Current password is incorrect.");
        }

        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        if (newPassword != confirmPassword)
        {
            throw new ArgumentException("Passwords do not match.");
        }

        CurrentAccount.Password = newPassword;
        _access.Update(CurrentAccount);
    }
}