namespace UnitTests;

[TestClass]
public class SeatingLogicTests
{


    [TestMethod]
    public void GetLayout_Boeing737_ReturnsCorrectTotalRows()
    {
        // arrange
        int aircraftId = 1;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(33, layout.TotalRows, "Boeing 737 moet 33 rijen hebben.");
    }

    [TestMethod]
    public void GetLayout_AirbusA330_ReturnsCorrectTotalRows()
    {
        // arrange
        int aircraftId = 2;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(43, layout.TotalRows, "Airbus A330 moet 43 rijen hebben.");
    }

    [TestMethod]
    public void GetLayout_Boeing787_ReturnsCorrectTotalRows()
    {
        // arrange
        int aircraftId = 3;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(38, layout.TotalRows, "Boeing 787 moet 38 rijen hebben.");
    }

    [TestMethod]
    public void GetLayout_UnknownAircraftId_ReturnsDefaultLayout()
    {
        // arrange
        int aircraftId = 999;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(1, layout.TotalRows, "Onbekend vliegtuig moet 1 rij teruggeven.");
    }

    [TestMethod]
    public void GetLayout_Boeing737_HasSixSeatLetters()
    {
        // arrange
        int aircraftId = 1;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(6, layout.Letters.Length, "Boeing 737 moet 6 stoelletters hebben.");
    }

    [TestMethod]
    public void GetLayout_AirbusA330_HasEightSeatLetters()
    {
        // arrange
        int aircraftId = 2;

        // act
        var layout = SeatingLogic.GetLayout(aircraftId);

        // assert
        Assert.AreEqual(8, layout.Letters.Length, "Airbus A330 moet 8 stoelletters hebben.");
    }

    // busieness row tests

    [TestMethod]
    public void IsBusinessRow_RowOneOnBoeing737_ReturnsTrue()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        int row = 1;

        // act
        bool result = SeatingLogic.IsBusinessRow(row, layout);

        // assert
        Assert.IsTrue(result, "Rij 1 moet een business rij zijn op de Boeing 737.");
    }

    [TestMethod]
    public void IsBusinessRow_ExactlyAtBusinessLimit_ReturnsTrue()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1); 
        int row = 3;

        // act
        bool result = SeatingLogic.IsBusinessRow(row, layout);

        // assert
        Assert.IsTrue(result, "Rij 3 is de laatste business rij en moet true teruggeven.");
    }

    [TestMethod]
    public void IsBusinessRow_FirstEconomyRow_ReturnsFalse()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1); 
        int row = 4;

        // act
        bool result = SeatingLogic.IsBusinessRow(row, layout);

        // assert
        Assert.IsFalse(result, "Rij 4 is de eerste economy rij en moet false teruggeven.");
    }

    [TestMethod]
    public void IsBusinessRow_LastRowOnBoeing737_ReturnsFalse()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1); 
        int row = 33;

        // act
        bool result = SeatingLogic.IsBusinessRow(row, layout);

        // assert
        Assert.IsFalse(result, "De laatste rij mag nooit een business rij zijn.");
    }

    // seat price tests

    [TestMethod]
    public void GetSeatPrice_BusinessRow_ReturnsBusinessPrice()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        int row = 1;
        double economyPrice = 100;
        double businessPrice = 300;

        // act
        double result = SeatingLogic.GetSeatPrice(row, layout, economyPrice, businessPrice);

        // assert
        Assert.AreEqual(businessPrice, result, "Business rij moet de business prijs teruggeven.");
    }

    [TestMethod]
    public void GetSeatPrice_EconomyRow_ReturnsEconomyPrice()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        int row = 10;
        double economyPrice = 100;
        double businessPrice = 300;

        // act
        double result = SeatingLogic.GetSeatPrice(row, layout, economyPrice, businessPrice);

        // assert
        Assert.AreEqual(economyPrice, result, "Economy rij moet de economy prijs teruggeven.");
    }

    [TestMethod]
    public void GetSeatPrice_FirstEconomyRow_ReturnsEconomyPrice()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1); 
        int row = 4;
        double economyPrice = 150;
        double businessPrice = 400;

        // act
        double result = SeatingLogic.GetSeatPrice(row, layout, economyPrice, businessPrice);

        // assert
        Assert.AreEqual(economyPrice, result, "Eerste economy rij moet de economy prijs teruggeven.");
    }

    // first seat tests.

    [TestMethod]
    public void GetFirstAvailableSeat_WithOneAvailableSeat_ReturnsCorrectRow()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        var availableSeats = new List<SeatModel>
        {
            new SeatModel(1, "1F", 1, "Business", false, false, true, false)
        };

        // act
        var result = SeatingLogic.GetFirstAvailableSeat(availableSeats, layout);

        // assert
        Assert.IsNotNull(result, "Er moet een stoel gevonden worden.");
        Assert.AreEqual(1, result.Value.row, "De eerste beschikbare stoel moet in rij 1 zitten.");
    }

    [TestMethod]
    public void GetFirstAvailableSeat_NoAvailableSeats_ReturnsNull()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        var availableSeats = new List<SeatModel>();

        // act
        var result = SeatingLogic.GetFirstAvailableSeat(availableSeats, layout);

        // assert
        Assert.IsNull(result, "Als er geen stoelen beschikbaar zijn moet null teruggegeven worden.");
    }

    [TestMethod]
    public void GetFirstAvailableSeat_OnlyLaterRowAvailable_ReturnsCorrectRow()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        var availableSeats = new List<SeatModel>
        {
            new SeatModel(1, "10F", 10, "Economy", false, false, false, false)
        };

        // act
        var result = SeatingLogic.GetFirstAvailableSeat(availableSeats, layout);

        // assert
        Assert.IsNotNull(result, "Er moet een stoel gevonden worden.");
        Assert.AreEqual(10, result.Value.row, "De eerste beschikbare stoel moet in rij 10 zitten.");
    }

    [TestMethod]
    public void GetFirstAvailableSeat_MultipleSeatsAvailable_ReturnsEarliestRow()
    {
        // arrange
        var layout = SeatingLogic.GetLayout(1);
        var availableSeats = new List<SeatModel>
        {
            new SeatModel(1, "5F", 5, "Economy", false, false, false, false),
            new SeatModel(1, "2F", 2, "Business", false, false, false, false)
        };

        // act
        var result = SeatingLogic.GetFirstAvailableSeat(availableSeats, layout);

        // assert
        Assert.IsNotNull(result, "Er moet een stoel gevonden worden.");
        Assert.AreEqual(2, result.Value.row, "De vroegste beschikbare rij moet teruggegeven worden.");
    }
}