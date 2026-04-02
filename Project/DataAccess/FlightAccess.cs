using Microsoft.Data.Sqlite;

using Dapper;


public class FlightAccess
{

    // In case we work with a database, we could use this query to get all the scheduled flights from the database to show all the available flights.
    // If we are going to work with another data source, we can change this method to fit the data source we are working with.
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public List<FlightModel> GetAllAvailableFlights()
    {
        string sql = @"SELECT 
            f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            a.manufacturer AS AircraftManufacturer, a.model AS AircraftModel,
            dep.name AS DepartureAirportName, dep.city AS DepartureCity, dep.country AS DepartureCountry,
            dest.name AS DestinationAirportName, dest.city AS DestinationCity, dest.country AS DestinationCountry
            FROM Flights f
            JOIN Aircrafts a ON f.aircraftId = a.id
            JOIN Airports dep ON f.departureAirportId = dep.id
            JOIN Airports dest ON f.destinationAirportId = dest.id
            WHERE f.status = 'Scheduled' OR f.status = 'Delayed'";
        var flights = _connection.Query<FlightModel>(sql).ToList();
        return flights;
    }
}