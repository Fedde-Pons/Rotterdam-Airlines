using Microsoft.Data.Sqlite;
using Dapper;

public class PassangerAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Passengers";

    public int Write(PassangerModel passanger)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        INSERT INTO {Table}
        (firstName, lastName, dateOfBirth, passportNumber)
        VALUES
        (@FirstName, @LastName, @DateOfBirth, @PassportNumber);

        SELECT last_insert_rowid();";

        return connection.ExecuteScalar<int>(sql, passanger);
    }

    public PassangerModel? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"SELECT * FROM {Table} WHERE id = @Id;";
        return connection.QueryFirstOrDefault<PassangerModel>(sql, new { Id = id });
    }

    public List<PassangerModel> GetByBookingId(int bookingId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT p.* FROM {Table} p
        INNER JOIN Tickets t ON t.passengerId = p.id
        WHERE t.bookingId = @BookingId;";

        return connection.Query<PassangerModel>(sql, new { BookingId = bookingId }).ToList();
    }

    public List<PassangerListEntry> GetPassengerListForFlight(int flightId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT
            p.firstName     AS FirstName,
            p.lastName      AS LastName,
            p.passportNumber AS PassportNumber,
            s.seatNumber    AS SeatNumber,
            s.seatclass     AS SeatClass,
            t.extraBaggageKg AS ExtraBaggageKg
        FROM Tickets t
        INNER JOIN {Table} p ON p.id = t.passengerId
        INNER JOIN Seats s   ON s.id = t.seatId
        INNER JOIN Bookings b ON b.id = t.bookingId
        WHERE t.flightId = @FlightId
          AND LOWER(b.status) != 'cancelled'
        ORDER BY s.rowNumber, s.seatNumber;";

        return connection.Query<PassangerListEntry>(sql, new { FlightId = flightId }).ToList();
    }
}