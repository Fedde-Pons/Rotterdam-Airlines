using Microsoft.Data.Sqlite;

using Dapper;


public class FlightAccess
{

    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public List<FlightModel> GetAllAvailableFlights()
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            a.manufacturer AS AircraftManufacturer, a.model AS AircraftModel,
            dep.name AS DepartureAirportName, dep.city AS DepartureCity, dep.country AS DepartureCountry,
            dest.name AS DestinationAirportName, dest.city AS DestinationCity, dest.country AS DestinationCountry
            FROM Flights f
            JOIN Aircrafts a ON f.aircraftId = a.id
            JOIN Airports dep ON f.departureAirportId = dep.id
            JOIN Airports dest ON f.destinationAirportId = dest.id
            WHERE f.status = 'Scheduled' OR f.status = 'Delayed' OR f.status = 'Cancelled'";
        var flights = _connection.Query<FlightModel>(sql).ToList();
        return flights;
    }

    public List<FlightModel> GetAllFlights()
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            a.manufacturer AS AircraftManufacturer, a.model AS AircraftModel,
            dep.name AS DepartureAirportName, dep.city AS DepartureCity, dep.country AS DepartureCountry,
            dest.name AS DestinationAirportName, dest.city AS DestinationCity, dest.country AS DestinationCountry
            FROM Flights f
            JOIN Aircrafts a ON f.aircraftId = a.id
            JOIN Airports dep ON f.departureAirportId = dep.id
            JOIN Airports dest ON f.destinationAirportId = dest.id";
        var flights = _connection.Query<FlightModel>(sql).ToList();
        return flights;
    }
}