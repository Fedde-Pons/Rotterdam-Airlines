public class FlightModel
{
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

    public FlightModel(string flightNumber, int aircraftId, int departureAirportId, int destinationAirportId, string departureTime, string arrivalTime, double basePrice, string status)
    {
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
}