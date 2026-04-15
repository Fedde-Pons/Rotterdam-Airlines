public class AccountModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
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


