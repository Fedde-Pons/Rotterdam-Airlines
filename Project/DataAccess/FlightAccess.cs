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

    public bool StoreNewFlightDetails(FlightModel flight)
    {
        try
        {
            string sql = @"UPDATE Flights 
                SET flightNumber = @FlightNumber, 
                    aircraftId = @AircraftId, 
                    departureAirportId = @DepartureAirportId, 
                    destinationAirportId = @DestinationAirportId, 
                    departureTime = @DepartureTime, 
                    arrivalTime = @ArrivalTime, 
                    basePrice = @BasePrice, 
                    status = @Status 
                WHERE id = @Id";
            
            var result = _connection.Execute(sql, flight);
            return result > 0;
        }
        catch
        {
            return false;
        }
    }

    public FlightModel? RetrieveFlight(int id)
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
            WHERE f.id = @Id";
        return _connection.QueryFirstOrDefault<FlightModel>(sql, new { Id = id });
    }

    public FlightModel? RetrieveFlight(string flightNumber)
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
            WHERE f.flightNumber = @FlightNumber";
        return _connection.QueryFirstOrDefault<FlightModel>(sql, new { FlightNumber = flightNumber });
    }

    public (List<SeatModel> availableSeats, int totalSeats, int bookedSeats) GetLiveSeatData(int flightId, int aircraftId)
    {

        string getSeatsQuery = "SELECT * FROM Seats WHERE aircraftId = @AircraftId";
        List<SeatModel> allSeats = _connection.Query<SeatModel>(getSeatsQuery, new { AircraftId = aircraftId }).ToList();


        string getBookedSeatsQuery = "SELECT seatId FROM Tickets WHERE flightId = @FlightId";
        List<int> bookedSeatIds = _connection.Query<int>(getBookedSeatsQuery, new { FlightId = flightId }).ToList();


        List<SeatModel> availableSeats = allSeats.Where(seat => !bookedSeatIds.Contains(seat.Id)).ToList();

        return (availableSeats, allSeats.Count, bookedSeatIds.Count);
    }


}