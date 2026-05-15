namespace UnitTests;

[TestClass]
public sealed class VerificationToolboxTesting
{
    // ===== H1 / S2: ValidateFirstName =====

    [TestMethod]
    [DataRow("John")]
    [DataRow("Mary")]
    [DataRow("Anne")]
    public void ValidateFirstName_ValidInput_ReturnsTrue(string firstName)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateFirstName(firstName);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("John1")]  // contains number
    [DataRow("Jo hn")]  // contains space
    [DataRow("Ann3")]   // contains number
    public void ValidateFirstName_InvalidInput_ReturnsFalse(string firstName)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateFirstName(firstName);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidateLastName =====

    [TestMethod]
    [DataRow("Smith")]
    [DataRow("Van der Berg")]
    [DataRow("De Vries")]
    public void ValidateLastName_ValidInput_ReturnsTrue(string lastName)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateLastName(lastName);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("Smith1")]   // contains number
    [DataRow("Van2Berg")] // contains number
    public void ValidateLastName_InvalidInput_ReturnsFalse(string lastName)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateLastName(lastName);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidateEmailAddress =====

    [TestMethod]
    [DataRow("user@example.com")]
    [DataRow("test@test.nl")]
    [DataRow("john.doe@domain.org")]
    public void ValidateEmailAddress_ValidInput_ReturnsTrue(string emailAddress)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateEmailAddress(emailAddress);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("notanemail")]       // no @ and no .
    [DataRow("@nodomain.com")]    // nothing before @
    [DataRow("noatsign.com")]     // no @
    [DataRow("double@@test.com")] // two @ signs
    [DataRow("")]                 // empty
    public void ValidateEmailAddress_InvalidInput_ReturnsFalse(string emailAddress)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateEmailAddress(emailAddress);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidatePassword =====

    [TestMethod]
    [DataRow("Password1!")]
    [DataRow("Secure@99")]
    [DataRow("MyPass#1word")]
    public void ValidatePassword_ValidInput_ReturnsTrue(string password)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidatePassword(password);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("short1!")]        // too short (7 characters)
    [DataRow("abc")]            // too short (3 characters)
    [DataRow("nouppercase1!")]  // no uppercase letter
    [DataRow("NOLOWERCASE1!")]  // no lowercase letter
    [DataRow("NoSpecialChar1")] // no special character
    [DataRow("No Numbers!A")]   // no digit and contains space
    [DataRow("")]               // empty
    public void ValidatePassword_InvalidInput_ReturnsFalse(string password)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidatePassword(password);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidateDateOfBirth =====

    [TestMethod]
    [DataRow("01/01/1990")]
    [DataRow("15/06/2000")]
    [DataRow("29/02/2000")] // 2000 is a leap year
    public void ValidateDateOfBirth_ValidInput_ReturnsTrue(string dateOfBirth)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateDateOfBirth(dateOfBirth);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("1/1/1990")]     // no leading zeros
    [DataRow("31/13/2000")]   // invalid month (13)
    [DataRow("32/01/2000")]   // invalid day (32)
    [DataRow("29/02/1999")]   // 1999 is not a leap year
    [DataRow("01-01-1990")]   // wrong separator (dashes instead of slashes)
    [DataRow("")]             // empty
    public void ValidateDateOfBirth_InvalidInput_ReturnsFalse(string dateOfBirth)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidateDateOfBirth(dateOfBirth);

        // assert
        Assert.IsFalse(result);
    }

    // ===== H1 / S2: ValidatePhoneNumber =====

    [TestMethod]
    [DataRow("31", "612345678")]  // NL country code, valid 9-digit number
    [DataRow("1", "2025550100")] // USA country code, valid 10-digit number
    [DataRow("X", "X")]          // optional skip sentinel
    public void ValidatePhoneNumber_ValidInput_ReturnsTrue(string countryCode, string phoneNumber)
    {
        // arrange
        // (static method — no instance needed)

        // act
        bool result = VerificationToolbox.ValidatePhoneNumber(countryCode, phoneNumber);

        // assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow("999", "123456")] // invalid country code (999 not in list)
    [DataRow("31", "12abc")]   // letters in phone number
    [DataRow("31", "123")]     // phone number too short (3 digits, minimum is 6)
    [DataRow("31", "")]        // empty phone number
    public void ValidatePhoneNumber_InvalidInput_ReturnsFalse(string countryCode, string phoneNumber)
    {
        // arrange
        // (static method - no instance needed)

        // act
        bool result = VerificationToolbox.ValidatePhoneNumber(countryCode, phoneNumber);

        // assert
        Assert.IsFalse(result);
    }
}
