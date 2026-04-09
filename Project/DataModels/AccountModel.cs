public class AccountModel
{
    public long Id { get; set; }
    private string _firstName;
    public string FirstName
    {
        get { return _firstName; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                _firstName = value;
                return;
            }

            _firstName = char.ToUpper(value[0]) + value.Substring(1);
        }
    }
    private string _lastName;
    public string LastName
    {
        get { return _lastName; }
        set 
        { 
            if (string.IsNullOrEmpty(value))
            {
                _lastName = value;
                return;
            }

            _lastName = char.ToUpper(value[0]) + value.Substring(1);
        }
    }
    public string DateOfBirth { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string CreatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public AccountModel() { }

    public AccountModel(string firstName, string lastName, string emailAddress, string phoneNumber, string password, string dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Password = password;
        DateOfBirth = dateOfBirth;
    }
}

