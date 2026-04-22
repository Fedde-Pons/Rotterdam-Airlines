public class FlightModel
{
    public int Id {get; set;}
    public string? FlightNumber {get; set;}
    public int AircraftId {get; set;}
    public int DepartureAirportId {get; set;}
    public int DestinationAirportId {get; set;}
    public string? DepartureTime {get; set;}
    public string? ArrivalTime {get; set;}
    public double BasePrice {get; set;}
    public string? Status {get; set;}

    // Aircraft info
    public string? AircraftManufacturer {get; set;}
    public string? AircraftModel {get; set;}

    // Departure airport info
    public string? DepartureAirportName {get; set;}
    public string? DepartureCity {get; set;}
    public string? DepartureCountry {get; set;}

    // Destination airport info
    public string? DestinationAirportName {get; set;}
    public string? DestinationCity {get; set;}
    public string? DestinationCountry {get; set;}
    public int distance_in_km {get; set;} = 0;
    public FlightModel(int id, string flightNumber, int aircraftId, int departureAirportId, int destinationAirportId, string departureTime, string arrivalTime, double basePrice, string status)
    {
        Id = id;
        FlightNumber = flightNumber;
        AircraftId = aircraftId;
        DepartureAirportId = departureAirportId;
        DestinationAirportId = destinationAirportId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        BasePrice = basePrice;
        Status = status;
    }

    public FlightModel() { }

    public void UpdateBasePriceInDB(
        double demandFactor,
        double timeUntilDepartureFactor,
        string seatType = "economy",
        double discount = 0)
    {
        // Bereken de nieuwe prijs met PricingCoreLogic
        double newPrice = PricingCoreLogic.CalculateFlightPrice(
            distance_in_km,
            demandFactor,
            timeUntilDepartureFactor,
            seatType,
            discount);
        
        // Update de BasePrice
        BasePrice = newPrice;
        
        // Sla op in de database
        FlightAccess flightAccess = new FlightAccess();
        flightAccess.StoreNewFlightDetails(this);
    }
}