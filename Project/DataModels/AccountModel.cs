public class AccountModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string dateOfBirth { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }

    public string CreatedAt { get; set; }

    public string FullName
    {
        get { return $"{FirstName} {LastName}"; }
    }
    public AccountModel()
    {
    }
    public AccountModel(long id, string firstName, string lastName, string dateOfBirth, string emailAddress, string phoneNumber, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        this.dateOfBirth = dateOfBirth;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Password = password;
    }
}



