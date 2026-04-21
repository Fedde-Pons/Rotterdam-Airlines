public class PassangerModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public int PassportNumber { get; set; }

    public PassangerModel(string firstName, string lastName, string dateOfBirth, int passportNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PassportNumber = passportNumber;
    }

}

}
fab11e6c03fc3a7043fa742b4054f75b81c17d2b
