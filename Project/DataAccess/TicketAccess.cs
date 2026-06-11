using Microsoft.Data.Sqlite;
using Dapper;

public class TicketAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Tickets";

    public int Write(TicketModel ticket)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        INSERT INTO {Table}
        (bookingId, flightId, seatId, passengerId, price, extraBaggageKg, isCheckedIn)
        VALUES
        (@BookingId, @FlightId, @SeatId, @PassengerId, @Price, @ExtraBaggageKg, @IsCheckedIn);

        SELECT last_insert_rowid();";

        return connection.ExecuteScalar<int>(sql, ticket);
    }

    public List<TicketModel> GetByBookingId(int bookingId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT id, bookingId, flightId, seatId, passengerId, price, extraBaggageKg, isCheckedIn
        FROM {Table}
        WHERE bookingId = @BookingId;";

        return connection.Query<TicketModel>(sql, new { BookingId = bookingId }).ToList();
    }
    public List<TicketModel> GetByFlightId(int flightId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT *
        FROM {Table}
        WHERE flightID = @FlightId;";

        return connection.Query<TicketModel>(sql, new { FlightId = flightId }).ToList();
    }

    public void UpdateCheckInStatus(int ticketId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        UPDATE {Table}
        SET isCheckedIn = 1
        WHERE id = @TicketId;";

        connection.Execute(sql, new { TicketId = ticketId });
    }

    public void ResetCheckInForBooking(int bookingId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        UPDATE {Table}
        SET isCheckedIn = 0
        WHERE bookingId = @BookingId;";

        connection.Execute(sql, new { BookingId = bookingId });
    }

    public (int businessBooked, int economyBooked) GetSeatOccupancyByFlightId(int flightId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = @"
        SELECT
            COUNT(CASE WHEN s.seatClass = 'Business' THEN 1 END) AS BusinessBooked,
            COUNT(CASE WHEN s.seatClass = 'Economy'  THEN 1 END) AS EconomyBooked
        FROM Tickets t
        JOIN Seats s ON t.seatId = s.id
        WHERE t.flightId = @FlightId;";

        return connection.QueryFirstOrDefault<(int BusinessBooked, int EconomyBooked)>(
            sql, new { FlightId = flightId });
    }
}