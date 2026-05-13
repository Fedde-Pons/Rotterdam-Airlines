namespace UnitTests;

[TestClass]
public class BookingTests
{
    [TestMethod]
    public void CreateBooking_WithValidInput_ReturnsSuccess()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking moet succesvol zijn bij geldige invoer.");
    }

    [TestMethod]
    public void CreateBooking_WithValidInput_ReturnsNonNullBooking()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsNotNull(result.booking, "Booking mag niet null zijn bij geldige invoer.");
    }

    [TestMethod]
    public void CreateBooking_WithValidInput_MessageIsEmpty()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual("", result.Message, "Message moet leeg zijn bij een succesvolle boeking.");
    }

    [TestMethod]
    public void CreateBooking_BookingHasCorrectAccountId()
    {
        // arrange
        int accountId = 42;
        string date = "2026-07-15";
        float totalPrice = 150.00f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual(accountId, result.booking!.AccountId, "AccountId van de boeking klopt niet.");
    }

    [TestMethod]
    public void CreateBooking_BookingHasCorrectDate()
    {
        // arrange
        int accountId = 1;
        string date = "2026-08-20";
        float totalPrice = 200.00f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual(date, result.booking!.Date, "Datum van de boeking klopt niet.");
    }

    [TestMethod]
    public void CreateBooking_BookingHasCorrectStatus()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
        string status = "Cancelled";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual(status, result.booking!.Status, "Status van de boeking klopt niet.");
    }

    [TestMethod]
    public void CreateBooking_BookingHasCorrectTotalPrice()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 499.95f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual((double)totalPrice, result.booking!.TotalPrice, 0.001, "TotalPrice van de boeking klopt niet.");
    }

    [TestMethod]
    public void CreateBooking_WithStatusConfirmed_ReturnsSuccess()
    {
        // arrange
        int accountId = 5;
        string date = "2026-09-10";
        float totalPrice = 350.00f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met status 'Confirmed' moet succesvol zijn.");
    }

    [TestMethod]
    public void CreateBooking_WithStatusCancelled_ReturnsSuccess()
    {
        // arrange
        int accountId = 5;
        string date = "2026-09-10";
        float totalPrice = 350.00f;
        string status = "Cancelled";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met status 'Cancelled' moet succesvol zijn.");
    }

    [TestMethod]
    public void CreateBooking_WithZeroPrice_ReturnsSuccess()
    {
        // arrange
        int accountId = 3;
        string date = "2026-10-01";
        float totalPrice = 0f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met prijs 0 moet succesvol zijn.");
        Assert.AreEqual(0.0, result.booking!.TotalPrice, 0.001, "TotalPrice moet 0 zijn.");
    }

    [TestMethod]
    public void CreateBooking_WithHighPrice_ReturnsSuccess()
    {
        // arrange
        int accountId = 7;
        string date = "2026-12-31";
        float totalPrice = 9999.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met hoge prijs moet succesvol zijn.");
        Assert.AreEqual((double)totalPrice, result.booking!.TotalPrice, 0.01, "TotalPrice klopt niet voor hoge prijs.");
    }
}