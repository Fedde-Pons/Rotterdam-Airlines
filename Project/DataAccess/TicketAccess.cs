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
        (bookingId, flightId, seatId, passengerId, price, extraBaggageKg)
        VALUES
        (@BookingId, @FlightId, @SeatId, @PassengerId, @Price, @ExtraBaggageKg);

        SELECT last_insert_rowid();";

        return connection.ExecuteScalar<int>(sql, ticket);
    }

    public List<TicketModel> GetByBookingId(int bookingId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT id, bookingId, flightId, seatId, passengerId, price, extraBaggageKg
        FROM {Table}
        WHERE bookingId = @BookingId;";

        return connection.Query<TicketModel>(sql, new { BookingId = bookingId }).ToList();
    }
}