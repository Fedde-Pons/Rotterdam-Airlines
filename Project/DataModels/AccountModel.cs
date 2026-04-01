public class AccountModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DOB { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }

    public AccountModel(long id, string firstName, string lastName, string dob, string emailAddress, string phoneNumber, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DOB = dob;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        Password = password;
    }
}



