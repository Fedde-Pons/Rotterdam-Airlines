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
        // check if everything is filled in
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(dob) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(phoneNumber) ||
            string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        // check if email already exists
        if (_access.EmailExists(email))
        {
            return false;
        }

        // create account object
        AccountModel newAccount = new AccountModel(
            0,
            firstName,
            lastName,
            dob,
            email,
            phoneNumber,
            password
        );

        // save to database
        _access.Write(newAccount);

        return true;
    }
}