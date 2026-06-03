using Microsoft.Data.Sqlite;
using Dapper;

public class FlightAccess
{
    private SqliteConnection _connection = new SqliteConnection("Data Source=DataSources/project.db");

    public List<FlightModel> GetAllAvailableFlights()
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            COALESCE(a.manufacturer, 'Unknown') AS AircraftManufacturer,
            COALESCE(a.model, 'Unknown') AS AircraftModel,
            COALESCE(dep.name, 'Unknown airport') AS DepartureAirportName,
            COALESCE(dep.city, 'Airport ID ' || f.departureAirportId) AS DepartureCity,
            COALESCE(dep.country, 'Unknown country') AS DepartureCountry,
            COALESCE(dest.name, 'Unknown airport') AS DestinationAirportName,
            COALESCE(dest.city, 'Airport ID ' || f.destinationAirportId) AS DestinationCity,
            COALESCE(dest.country, 'Unknown country') AS DestinationCountry
            FROM Flights f
            LEFT JOIN Aircrafts a ON f.aircraftId = a.id
            LEFT JOIN Airports dep ON f.departureAirportId = dep.id
            LEFT JOIN Airports dest ON f.destinationAirportId = dest.id
            WHERE f.status = 'Scheduled' OR f.status = 'Delayed' OR f.status = 'Cancelled'";

        return _connection.Query<FlightModel>(sql).ToList();
    }

    public List<FlightModel> GetAllFlights()
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            COALESCE(a.manufacturer, 'Unknown') AS AircraftManufacturer,
            COALESCE(a.model, 'Unknown') AS AircraftModel,
            COALESCE(dep.name, 'Unknown airport') AS DepartureAirportName,
            COALESCE(dep.city, 'Airport ID ' || f.departureAirportId) AS DepartureCity,
            COALESCE(dep.country, 'Unknown country') AS DepartureCountry,
            COALESCE(dest.name, 'Unknown airport') AS DestinationAirportName,
            COALESCE(dest.city, 'Airport ID ' || f.destinationAirportId) AS DestinationCity,
            COALESCE(dest.country, 'Unknown country') AS DestinationCountry
            FROM Flights f
            LEFT JOIN Aircrafts a ON f.aircraftId = a.id
            LEFT JOIN Airports dep ON f.departureAirportId = dep.id
            LEFT JOIN Airports dest ON f.destinationAirportId = dest.id";

        return _connection.Query<FlightModel>(sql).ToList();
    }

    public bool AddFlight(FlightModel flight)
    {
        try
        {
            string sql = @"INSERT INTO Flights
            (flightNumber, aircraftId, departureAirportId, destinationAirportId, departureTime, arrivalTime, basePrice, status)
            VALUES
            (@FlightNumber, @AircraftId, @DepartureAirportId, @DestinationAirportId, @DepartureTime, @ArrivalTime, @BasePrice, @Status)";

            int result = _connection.Execute(sql, flight);
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
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

            int result = _connection.Execute(sql, flight);
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public FlightModel? RetrieveFlight(int id)
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            COALESCE(a.manufacturer, 'Unknown') AS AircraftManufacturer,
            COALESCE(a.model, 'Unknown') AS AircraftModel,
            COALESCE(dep.name, 'Unknown airport') AS DepartureAirportName,
            COALESCE(dep.city, 'Airport ID ' || f.departureAirportId) AS DepartureCity,
            COALESCE(dep.country, 'Unknown country') AS DepartureCountry,
            COALESCE(dest.name, 'Unknown airport') AS DestinationAirportName,
            COALESCE(dest.city, 'Airport ID ' || f.destinationAirportId) AS DestinationCity,
            COALESCE(dest.country, 'Unknown country') AS DestinationCountry
            FROM Flights f
            LEFT JOIN Aircrafts a ON f.aircraftId = a.id
            LEFT JOIN Airports dep ON f.departureAirportId = dep.id
            LEFT JOIN Airports dest ON f.destinationAirportId = dest.id
            WHERE f.id = @Id";

        return _connection.QueryFirstOrDefault<FlightModel>(sql, new { Id = id });
    }

    public FlightModel? RetrieveFlight(string flightNumber)
    {
        string sql = @"SELECT 
            f.id, f.flightNumber, f.aircraftId, f.departureAirportId, f.destinationAirportId, 
            f.departureTime, f.arrivalTime, f.basePrice, f.status,
            COALESCE(a.manufacturer, 'Unknown') AS AircraftManufacturer,
            COALESCE(a.model, 'Unknown') AS AircraftModel,
            COALESCE(dep.name, 'Unknown airport') AS DepartureAirportName,
            COALESCE(dep.city, 'Airport ID ' || f.departureAirportId) AS DepartureCity,
            COALESCE(dep.country, 'Unknown country') AS DepartureCountry,
            COALESCE(dest.name, 'Unknown airport') AS DestinationAirportName,
            COALESCE(dest.city, 'Airport ID ' || f.destinationAirportId) AS DestinationCity,
            COALESCE(dest.country, 'Unknown country') AS DestinationCountry
            FROM Flights f
            LEFT JOIN Aircrafts a ON f.aircraftId = a.id
            LEFT JOIN Airports dep ON f.departureAirportId = dep.id
            LEFT JOIN Airports dest ON f.destinationAirportId = dest.id
            WHERE f.flightNumber = @FlightNumber";

        return _connection.QueryFirstOrDefault<FlightModel>(sql, new { FlightNumber = flightNumber });
    }

    public (List<SeatModel> availableSeats, List<SeatModel> allSeats, int bookedSeats) GetLiveSeatData(int flightId, int aircraftId)
    {
        string getSeatsQuery = "SELECT * FROM Seats WHERE aircraftId = @AircraftId";

        List<SeatModel> allSeats = _connection
            .Query<SeatModel>(getSeatsQuery, new { AircraftId = aircraftId })
            .ToList();

        string getBookedSeatsQuery = @"
            SELECT t.seatId
            FROM Tickets t
            INNER JOIN Bookings b ON b.id = t.bookingId
            WHERE t.flightId = @FlightId
              AND LOWER(b.status) != 'cancelled'";

        List<int> bookedSeatIds = _connection
            .Query<int>(getBookedSeatsQuery, new { FlightId = flightId })
            .ToList();

        List<SeatModel> availableSeats = allSeats
            .Where(seat => !bookedSeatIds.Contains(seat.Id))
            .ToList();

        return (availableSeats, allSeats, bookedSeatIds.Count);
    }

    public SeatModel? RetrieveSeat(int seatId)
    {
        string sql = "SELECT * FROM Seats WHERE id = @Id";
        return _connection.QueryFirstOrDefault<SeatModel>(sql, new { Id = seatId });
    }

    public List<AircraftModel> GetAllAircrafts()
    {
        string sql = "SELECT * FROM Aircrafts";
        return _connection.Query<AircraftModel>(sql).ToList();
    }
}