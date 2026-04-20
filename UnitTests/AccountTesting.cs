namespace UnitTests;

[TestClass]
public sealed class AccountTesting
{
    private static readonly string TestEmail = "unittest_account@example.com";
    private readonly AccountsAccess _access = new();

    [TestCleanup]
    public void Cleanup()
    {
        AccountModel existing = _access.GetByEmail(TestEmail);
        if (existing != null)
            _access.Delete(existing.Id);
    }

    // ===== H1 / S2: ValidateFirstName =====

    [DataTestMethod]
    [DataRow("John")]
    [DataRow("Mary")]
    [DataRow("Anne")]
    public void ValidateFirstName_ValidInput_ReturnsTrue(string firstName)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateFirstName(firstName);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("John1")]   // contains number
    [DataRow("Jo hn")]   // contains space
    [DataRow("Ann3")]    // contains number
    public void ValidateFirstName_InvalidInput_ReturnsFalse(string firstName)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateFirstName(firstName);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidateLastName =====

    [DataTestMethod]
    [DataRow("Smith")]
    [DataRow("Van der Berg")]   // Dutch last name with spaces is allowed
    [DataRow("De Vries")]
    public void ValidateLastName_ValidInput_ReturnsTrue(string lastName)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateLastName(lastName);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("Smith1")]     // contains number
    [DataRow("Van2Berg")]   // contains number
    public void ValidateLastName_InvalidInput_ReturnsFalse(string lastName)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateLastName(lastName);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2 / S3: ValidateEmail =====

    [DataTestMethod]
    [DataRow("user@example.com")]
    [DataRow("test@test.nl")]
    [DataRow("john.doe@domain.org")]
    public void ValidateEmail_ValidInput_ReturnsTrue(string email)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateEmailAddress(email);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("notanemail")]         // no @ and no .
    [DataRow("@nodomain.com")]      // nothing before @
    [DataRow("noatsign.com")]       // no @
    [DataRow("double@@test.com")]   // two @ signs
    [DataRow("")]                   // S3: empty
    public void ValidateEmail_InvalidInput_ReturnsFalse(string email)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateEmailAddress(email);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2 / S3: ValidatePassword =====

    [DataTestMethod]
    [DataRow("Password1!")]
    [DataRow("Secure@99")]
    [DataRow("MyPass#1word")]
    public void ValidatePassword_ValidInput_ReturnsTrue(string password)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidatePassword(password);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("short1!")]           // too short 
    [DataRow("abc")]                // S2: too short
    [DataRow("nouppercase1!")]      // no uppercase
    [DataRow("NOLOWERCASE1!")]      // S2: no lowercase // ERROR, fix this
    [DataRow("NoSpecialChar1")]     // no special character
    [DataRow("No Numbers!A")]       // no number (has space too)
    [DataRow("")]                   // S3: empty
    public void ValidatePassword_InvalidInput_ReturnsFalse(string password)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidatePassword(password);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2 / S3: ValidateDateOfBirth =====

    [DataTestMethod]
    [DataRow("01/01/1990")]
    [DataRow("15/06/2000")]
    [DataRow("29/02/2000")]     // leap year
    public void ValidateDateOfBirth_ValidInput_ReturnsTrue(string dob)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateDateOfBirth(dob);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("1/1/1990")]           // wrong format (no leading zeros)
    [DataRow("31/13/2000")]         // invalid month
    [DataRow("32/01/2000")]         // invalid day
    [DataRow("29/02/1999")]         // not a leap year
    [DataRow("01-01-1990")]         // wrong separator
    [DataRow("")]                   // S3: empty
    public void ValidateDateOfBirth_InvalidInput_ReturnsFalse(string dob)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidateDateOfBirth(dob);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2 / S3: ValidatePhoneNumber =====

    [DataTestMethod]
    [DataRow("31", "612345678")]    // Netherlands
    [DataRow("1", "2025550100")]    // USA
    [DataRow("X", "X")]            // optional field skipped
    public void ValidatePhoneNumber_ValidInput_ReturnsTrue(string countryCode, string phoneNumber)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidatePhoneNumber(countryCode, phoneNumber);

        // assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("999", "123456")]      // invalid country code
    [DataRow("31", "12abc")]        // phone contains letters
    [DataRow("31", "123")]          // phone too short
    [DataRow("31", "")]             // S3: empty phone
    public void ValidatePhoneNumber_InvalidInput_ReturnsFalse(string countryCode, string phoneNumber)
    {
        // arrange
        CreateAccountLogic logic = new();

        // act
        bool result = logic.ValidatePhoneNumber(countryCode, phoneNumber);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H2 / H3: CreateAccount stores account in the database =====

    [TestMethod]
    public void CreateAccount_ValidData_AccountExistsInDatabase()
    {
        // arrange
        AccountsLogic logic = new();
        AccountModel account = new("Test", "User", TestEmail, "31 612345678", "Password1!", "01/01/1990");

        // act
        logic.CreateAccount(account);

        // assert
        AccountModel result = _access.GetByEmail(TestEmail);
        Assert.IsNotNull(result);
        Assert.AreEqual(TestEmail, result.EmailAddress);
    }

    // ===== H4 / S4: Duplicate email is blocked =====

    [TestMethod]
    public void CreateAccount_DuplicateEmail_ThrowsException()
    {
        // arrange
        AccountsLogic logic = new();
        AccountModel first = new("Test", "User", TestEmail, "31 612345678", "Password1!", "01/01/1990");
        AccountModel duplicate = new("Other", "Person", TestEmail, "31 699999999", "Different2@", "15/06/1995");

        logic.CreateAccount(first);

        // act & assert
        Assert.ThrowsException<ArgumentException>(() => logic.CreateAccount(duplicate));
    }

    // ===== S1: Missing required fields are rejected =====

    [DataTestMethod]
    [DataRow("", "User", "unittest_account@example.com", "31 612345678", "Password1!", "01/01/1990")]     // missing first name
    [DataRow("Test", "", "unittest_account@example.com", "31 612345678", "Password1!", "01/01/1990")]     // missing last name
    [DataRow("Test", "User", "", "31 612345678", "Password1!", "01/01/1990")]                             // missing email
    [DataRow("Test", "User", "unittest_account@example.com", "31 612345678", "abc", "01/01/1990")]        // password too short
    public void CreateAccount_MissingRequiredFields_ThrowsException(
        string firstName, string lastName, string email, string phone, string password, string dob)
    {
        // arrange
        AccountsLogic logic = new();
        AccountModel account = new(firstName, lastName, email, phone, password, dob);

        // act & assert
        Assert.ThrowsException<ArgumentException>(() => logic.CreateAccount(account));
    }
}
