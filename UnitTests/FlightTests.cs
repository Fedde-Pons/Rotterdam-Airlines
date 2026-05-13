namespace UnitTests;

[TestClass]
public class FlightTests
{
    [TestMethod]
    public void GetAllFlights_AllFlightsHaveUniqueFlightNumber()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        var flightNumbers = result.Select(f => f.FlightNumber).ToList();
        var uniqueFlightNumbers = flightNumbers.Distinct().ToList();
        Assert.AreEqual(flightNumbers.Count, uniqueFlightNumbers.Count, "Niet alle FlightNumbers zijn uniek.");
    }

    [TestMethod]
    public void GetAllFlights_ArrivalTimeIsAfterDepartureTime()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            if (DateTime.TryParse(flight.DepartureTime, out var dep) && DateTime.TryParse(flight.ArrivalTime, out var arr))
            {
                Assert.IsTrue(arr > dep, $"ArrivalTime ({flight.ArrivalTime}) is niet later dan DepartureTime ({flight.DepartureTime}) voor vlucht {flight.FlightNumber}");
            }
        }
    }

    [TestMethod]
    public void GetAllFlights_BasePriceWithinRealisticRange()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsTrue(flight.BasePrice >= 50 && flight.BasePrice <= 5000, $"BasePrice ({flight.BasePrice}) is buiten het realistische bereik voor vlucht {flight.FlightNumber}");
        }
    }

    [TestMethod]
    public void GetAllFlights_NoDomesticFlights()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.AreNotEqual(flight.DepartureCountry, flight.DestinationCountry, $"Binnenlandse vlucht gevonden: {flight.FlightNumber} ({flight.DepartureCountry} -> {flight.DestinationCountry})");
        }
    }
    [TestMethod]
    public void GetAllFlights_ReturnsNonNullList()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetAllFlights_ReturnsFlightsWithValidFlightNumber()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        Assert.IsTrue(result.Count > 0, "Er zijn geen vluchten gevonden in de database.");
        foreach (var flight in result)
        {
            Assert.IsFalse(string.IsNullOrEmpty(flight.FlightNumber), "FlightNumber mag niet leeg zijn.");
        }
    }

    [TestMethod]
    public void GetAllFlights_AllFlightsHaveScheduledOrDelayedStatus()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsTrue(
                flight.Status == "Scheduled" || flight.Status == "Delayed" || flight.Status == "Cancelled",
                $"Vlucht {flight.FlightNumber} heeft een onverwachte status: {flight.Status}");
        }
    }

    [TestMethod]
    public void GetAllFlights_AllFlightsHaveDepartureAndDestinationInfo()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsFalse(string.IsNullOrEmpty(flight.DepartureCity), $"DepartureCity is leeg voor vlucht {flight.FlightNumber}");
            Assert.IsFalse(string.IsNullOrEmpty(flight.DestinationCity), $"DestinationCity is leeg voor vlucht {flight.FlightNumber}");
            Assert.IsFalse(string.IsNullOrEmpty(flight.DepartureCountry), $"DepartureCountry is leeg voor vlucht {flight.FlightNumber}");
            Assert.IsFalse(string.IsNullOrEmpty(flight.DestinationCountry), $"DestinationCountry is leeg voor vlucht {flight.FlightNumber}");
        }
    }

    [TestMethod]
    public void GetAllFlights_AllFlightsHaveValidBasePrice()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsTrue(flight.BasePrice > 0, $"BasePrice moet groter zijn dan 0 voor vlucht {flight.FlightNumber}");
        }
    }

    [TestMethod]
    public void GetAllFlights_AllFlightsHaveAircraftInfo()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsFalse(string.IsNullOrEmpty(flight.AircraftManufacturer), $"AircraftManufacturer is leeg voor vlucht {flight.FlightNumber}");
            Assert.IsFalse(string.IsNullOrEmpty(flight.AircraftModel), $"AircraftModel is leeg voor vlucht {flight.FlightNumber}");
        }
    }

    [TestMethod]
    public void GetAllFlights_CanBeSortedByDepartureTime()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();
        List<FlightModel> sorted = result.OrderBy(f => f.DepartureTime).ToList();

        // assert
        Assert.IsNotNull(sorted);
        Assert.AreEqual(result.Count, sorted.Count);
        for (int i = 1; i < sorted.Count; i++)
        {
            Assert.IsTrue(
                string.Compare(sorted[i - 1].DepartureTime, sorted[i].DepartureTime) <= 0,
                "Vluchten zijn niet correct gesorteerd op vertrektijd.");
        }
    }

    [TestMethod]
    public void GetAllFlights_AllFlightsHaveDepartureAndArrivalTime()
    {
        // arrange
        FlightLogic logic = new();

        // act
        List<FlightModel> result = logic.GetAllFlights();

        // assert
        foreach (var flight in result)
        {
            Assert.IsFalse(string.IsNullOrEmpty(flight.DepartureTime), $"DepartureTime is leeg voor vlucht {flight.FlightNumber}");
            Assert.IsFalse(string.IsNullOrEmpty(flight.ArrivalTime), $"ArrivalTime is leeg voor vlucht {flight.FlightNumber}");
        }
    }
}