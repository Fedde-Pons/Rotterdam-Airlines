// this handles creating a new account
public class CreateAccountLogic
{
    // this is how we talk to the database / file thing
    private AccountsAccess _access = new();

    public CreateAccountLogic()
    {
        // nothing special just needed so we can use the class
    }

    // this tries to create an account
    // returns true = success
    // returns false = something went wrong
    public bool CreateAccount(string firstName, string lastName, string dob, string email, string phoneNumber, string password)
    {
        // first we check if the user actually filled everything in
        // if not stop
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(dob) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        // now we check if the email already exists
        // because we don't want duplicate accounts
        AccountModel existingAccount = _access.GetByEmail(email);

        // if we find one stop again
        if (existingAccount != null)
        {
            return false;
        }

        // if everything is fine we make a new account object
        AccountModel newAccount = new AccountModel(0, firstName, lastName, dob, email, phoneNumber, password);

        // save it using the access layer
        _access.Write(newAccount);

        // done account created
        return true;
    }
}