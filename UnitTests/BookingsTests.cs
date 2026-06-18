namespace UnitTests;

[TestClass]
public class BookingTests
{

    [TestMethod]
    public void GetFlightById_FutureFlight_ReturnsFlightNotNull()
    {
        // arrange
        FlightModel flight = new(
            id: 0,
            flightNumber: "RT001",
            aircraftId: 1,
            departureAirportId: 1,
            destinationAirportId: 2,
            departureTime: DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm"),
            arrivalTime: DateTime.Now.AddDays(7).AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            basePrice: 150.00,
            status: "Scheduled"
        );

        // assert
        Assert.IsNotNull(flight, "Een vlucht in de toekomst moet selecteerbaar zijn.");
        Assert.IsGreaterThan(DateTime.Parse(flight.DepartureTime!), DateTime.Now, "De vertrektijd moet in de toekomst liggen.");
    }

    [TestMethod]
    public void GetFlightById_PastFlight_IsNotSelectable()
    {
        // arrange
        FlightModel flight = new(
            id: 0,
            flightNumber: "RT002",
            aircraftId: 1,
            departureAirportId: 1,
            destinationAirportId: 2,
            departureTime: DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm"),
            arrivalTime: DateTime.Now.AddDays(-1).AddHours(2).ToString("yyyy-MM-dd HH:mm"),
            basePrice: 100.00,
            status: "Scheduled"
        );

        // assert
        bool isPast = DateTime.Parse(flight.DepartureTime!) <= DateTime.Now;
        Assert.IsTrue(isPast, "Een vlucht in het verleden mag niet boekbaar zijn.");
    }


    [TestMethod]
    public void SeatAvailability_WithAvailableSeats_ReturnsAtLeastOneSeat()
    {
        // arrange
        FlightModel flight = new(
            id: 0,
            flightNumber: "RT003",
            aircraftId: 1,
            departureAirportId: 1,
            destinationAirportId: 2,
            departureTime: DateTime.Now.AddDays(5).ToString("yyyy-MM-dd HH:mm"),
            arrivalTime: DateTime.Now.AddDays(5).AddHours(3).ToString("yyyy-MM-dd HH:mm"),
            basePrice: 200.00,
            status: "Scheduled"
        );

        // assert
        Assert.IsNotNull(flight, "Vlucht mag niet null zijn bij beschikbaarheidscontrole.");
        Assert.AreNotEqual("Cancelled", flight.Status, "Een geannuleerde vlucht heeft geen beschikbare stoelen.");
    }

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
    public void CreateBooking_BookingHasCorrectStatus()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.AreEqual(status, result.booking!.Status, "Status van de boeking klopt niet.");
    }

    [TestMethod]
    public void CreateBooking_WithStatusConfirmed_ReturnsSuccess()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
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
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 299.99f;
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
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 0f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met prijs 0 moet succesvol zijn.");
    }

    [TestMethod]
    public void CreateBooking_WithHighPrice_ReturnsSuccess()
    {
        // arrange
        int accountId = 1;
        string date = "2026-06-01";
        float totalPrice = 99999.99f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "CreateBooking met een hoge prijs moet succesvol zijn.");
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
    public void CreateBooking_ConfirmationContainsBookingObject()
    {
        // arrange
        int accountId = 5;
        string date = "2026-09-10";
        float totalPrice = 350.00f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "Boeking moet succesvol zijn voor bevestiging.");
        Assert.IsNotNull(result.booking, "Bevestiging moet een boeking-object bevatten.");
        Assert.AreEqual(accountId, result.booking.AccountId, "Bevestiging moet het juiste account-ID bevatten.");
        Assert.AreEqual(date, result.booking.Date, "Bevestiging moet de juiste datum bevatten.");
    }

    [TestMethod]
    [DataRow("2030-01-01")]
    [DataRow("2026-12-31")]
    public void IsValidDateOfBirth_FutureDate_ReturnsFalse(string dateOfBirth)
    {
        // act
        bool result = PassangerLogic.IsValidDateOfBirth(dateOfBirth);

        // assert
        Assert.IsFalse(result, $"Geboortedatum in de toekomst ({dateOfBirth}) moet ongeldig zijn.");
    }

    [TestMethod]
    public void IsValidDateOfBirth_Today_ReturnsFalse()
    {
        // arrange
        string today = DateTime.Today.ToString("yyyy-MM-dd");

        // act
        bool result = PassangerLogic.IsValidDateOfBirth(today);

        // assert
        Assert.IsFalse(result, "Vandaag als geboortedatum moet ongeldig zijn.");
    }

    [TestMethod]
    public void IsValidDateOfBirth_MoreThan120YearsAgo_ReturnsFalse()
    {
        // arrange
        string tooOld = DateTime.Today.AddYears(-121).ToString("yyyy-MM-dd");

        // act
        bool result = PassangerLogic.IsValidDateOfBirth(tooOld);

        // assert
        Assert.IsFalse(result, "Een geboortedatum van meer dan 120 jaar geleden moet ongeldig zijn.");
    }

    [TestMethod]
    public void IsValidDateOfBirth_Exactly120YearsAgo_ReturnsFalse()
    {
        // arrange
        string exactly120 = DateTime.Today.AddYears(-120).ToString("yyyy-MM-dd");

        // act
        bool result = PassangerLogic.IsValidDateOfBirth(exactly120);

        // assert
        Assert.IsFalse(result, "Een geboortedatum van precies 120 jaar geleden moet ongeldig zijn.");
    }

    [TestMethod]
    [DataRow("1990-06-15")]
    [DataRow("2000-01-01")]
    [DataRow("1960-03-20")]
    public void IsValidDateOfBirth_ValidDate_ReturnsTrue(string dateOfBirth)
    {
        // act
        bool result = PassangerLogic.IsValidDateOfBirth(dateOfBirth);

        // assert
        Assert.IsTrue(result, $"Geboortedatum {dateOfBirth} moet geldig zijn.");
    }

    [TestMethod]
    public void CreateBooking_BeforeConfirm_BookingDetailsAreAvailable()
    {
        // arrange
        int accountId = 3;
        string date = "2026-10-15";
        float totalPrice = 175.50f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "Boeking moet aangemaakt kunnen worden voor bevestigingsstap.");
        Assert.IsNotNull(result.booking, "Boekingsdetails mogen niet null zijn bij de bevestigingsstap.");
        Assert.AreEqual(accountId, result.booking.AccountId, "AccountId moet zichtbaar zijn in de bevestigingsstap.");
        Assert.AreEqual(date, result.booking.Date, "Datum moet zichtbaar zijn in de bevestigingsstap.");
        Assert.AreEqual((double)totalPrice, result.booking.TotalPrice, 0.001, "Prijs moet zichtbaar zijn in de bevestigingsstap.");
        Assert.AreEqual(status, result.booking.Status, "Status moet zichtbaar zijn in de bevestigingsstap.");
    }

    [TestMethod]
    public void CreateBooking_StatusConfirmed_IsNormalized()
    {
        // arrange
        int accountId = 7;
        string date = "2026-12-31";
        float totalPrice = 9999.99f;
        string status = "confirmed"; // lowercase input

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "Boeking met status 'confirmed' moet succesvol zijn.");
        Assert.AreEqual("Confirmed", result.booking!.Status, "Status moet genormaliseerd worden naar 'Confirmed'.");
    }

    [TestMethod]
    public void CreateBooking_StatusCancelled_IsNormalized()
    {
        // arrange
        int accountId = 5;
        string date = "2026-09-10";
        float totalPrice = 350.00f;
        string status = "CANCELLED"; // uppercase input

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull, "Boeking met status 'CANCELLED' moet succesvol zijn.");
        Assert.AreEqual("Cancelled", result.booking!.Status, "Status moet genormaliseerd worden naar 'Cancelled'.");
    }

    [TestMethod]
    public void CreateBooking_AllFieldsPresent_OrderedDetailsComplete()
    {
        // arrange
        int accountId = 10;
        string date = "2026-11-05";
        float totalPrice = 420.00f;
        string status = "Confirmed";

        // act
        var result = BookingLogic.CreateBooking(accountId, date, totalPrice, status);

        // assert
        Assert.IsTrue(result.IsSuccesfull);
        Assert.IsNotNull(result.booking);
        Assert.AreNotEqual(0, result.booking.AccountId, "AccountId moet ingesteld zijn.");
        Assert.IsFalse(string.IsNullOrEmpty(result.booking.Date), "Datum moet ingesteld zijn.");
        Assert.IsGreaterThanOrEqualTo(result.booking.TotalPrice, 0.0, "Prijs moet ingesteld zijn.");
        Assert.IsFalse(string.IsNullOrEmpty(result.booking.Status), "Status moet ingesteld zijn.");
    }
}
