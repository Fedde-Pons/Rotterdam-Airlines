[TestClass]
public class AirportTests
{
    [TestMethod]
    public void AddAirport_ValidInput_AddsAirportSuccessfully()
    {
        // arrange
        var result = AirportLogic.AddAirport("Schiphol Airport", "Terminal 1", "Amsterdam", "Netherlands");

        // act & assert
        Assert.IsTrue(result.Item1, "Valid airport input should add successfully");
    }

    [TestMethod]
    public void AddAirport_ValidInput_ReturnsSuccessMessage()
    {
        // arrange
        var result = AirportLogic.AddAirport("Rotterdam Airport", "Main Terminal", "Rotterdam", "Netherlands");

        // act & assert
        Assert.AreEqual(true, result.Item1);
        Assert.AreEqual("airport successfully added", result.Item2);
    }

    [TestMethod]
    public void AddAirport_DuplicateName_ReturnsError()
    {
        // arrange - First add an airport
        AirportLogic.AddAirport("Schiphol Airport", "Terminal 1", "Amsterdam", "Netherlands");
        
        // act & assert
        var result = AirportLogic.AddAirport("Schiphol Airport", "New Terminal", "Amsterdam", "Netherlands");
        Assert.IsFalse(result.Item1, "Duplicate airport should fail");
        Assert.AreEqual("airport already exists", result.Item2);
    }

    [TestMethod]
    public void AddAirport_InvalidCity_ReturnsError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("Test Airport", "Main Terminal", "123 Street", "Country");

        // assert
        Assert.IsFalse(result.Item1, "Invalid city should fail");
        Assert.IsFalse(string.IsNullOrEmpty(result.Item2), "Should return error message");
    }

    [TestMethod]
    public void AddAirport_InvalidCountry_ReturnsError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("Test Airport", "Main Terminal", "City", "123 Country");

        // assert
        Assert.IsFalse(result.Item1, "Invalid country should fail");
        Assert.IsFalse(string.IsNullOrEmpty(result.Item2), "Should return error message");
    }

    [TestMethod]
    public void AddAirport_NullCity_ReturnsError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("Test Airport", "Main Terminal", null, "Country");

        // assert
        Assert.IsFalse(result.Item1, "Null city should fail");
    }

    [TestMethod]
    public void AddAirport_NullCountry_ReturnsError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("Test Airport", "Main Terminal", "City", null);

        // assert
        Assert.IsFalse(result.Item1, "Null country should fail");
    }

    [TestMethod]
    public void AddAirport_EmptyName_ReturnsModelCreationError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("", "Terminal", "City", "Country");

        // assert 
        Assert.IsFalse(result.Item1);  
    }
    [TestMethod]
    public void AddAirport_EmptyAddress_ReturnsModelCreationError()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("Test", "", "City", "Country");

        // assert 
        Assert.IsFalse(result.Item1);  
    }

    [TestMethod]
    public void AddAirport_ThrowsException_ReturnsError()
    {
        // arrange & act - The catch block should handle this gracefully
        var result = AirportLogic.AddAirport("Test", "Terminal", "City!@#", "Country!");

        // assert
        Assert.IsFalse(result.Item1);
        Assert.IsTrue(string.IsNullOrEmpty(result.Item2) || 
                     result.Item2 == "not a real location or city" || 
                     result.Item2.Contains("convert") || 
                     result.Item2 == "undefined behavior happend");
    }

    [TestMethod]
    public void AddAirport_GetAllAirportsReturnsNonEmptyList()
    {
        // arrange & act
        List<AirportModel> airports = new AirportAccess().GetAllAirports();

        // assert - Should have at least the airports we added in tests + initial data
        Assert.IsNotNull(airports);
        Assert.IsTrue(airports.Count > 0, "Should return non-empty list of airports");
    }

    [TestMethod]
    public void AddAirport_MultipleValidAirportsAllAdded()
    {
        // arrange & act
        var result1 = AirportLogic.AddAirport("Heathrow", "Terminal 5", "London", "UK");
        var result2 = AirportLogic.AddAirport("Charles de Gaulle", "T3", "Paris", "France");
        var result3 = AirportLogic.AddAirport("Fiumicino", "FCO", "Rome", "Italy");

        // assert
        Assert.IsTrue(result1.Item1, "First airport should add successfully");
        Assert.IsTrue(result2.Item1, "Second airport should add successfully");
        Assert.IsTrue(result3.Item1, "Third airport should add successfully");
    }

    [TestMethod]
    public void AddAirport_DuplicateAfterAdditionDetected()
    {
        // arrange - Add same airport twice
        var result1 = AirportLogic.AddAirport("Schiphol", "Terminal 2", "Amsterdam", "Netherlands");
        var result2 = AirportLogic.AddAirport("Schiphol", "Another Terminal", "Amsterdam", "Netherlands");

        // assert
        Assert.IsTrue(result1.Item1, "First addition should succeed");
        Assert.IsFalse(result2.Item1, "Second addition with same name should fail");
        Assert.AreEqual("airport already exists", result2.Item2);
    }

    [TestMethod]
    public void AddAirport_CanAddWithExtraSpaces()
    {
        // arrange & act
        var result = AirportLogic.AddAirport("  Amsterdam Schiphol  ", "Terminal A", "  Amsterdam  ", "  Netherlands  ");

        // assert
        Assert.IsTrue(result.Item1, "Airports with extra spaces should be added successfully");
    }
}
