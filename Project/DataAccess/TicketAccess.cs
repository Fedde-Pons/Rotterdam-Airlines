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
    
    public void UpdateCheckInStatus(int ticketId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        UPDATE {Table}
        SET isCheckedIn = 1
        WHERE id = @TicketId;";

        connection.Execute(sql, new { TicketId = ticketId });
    }
}