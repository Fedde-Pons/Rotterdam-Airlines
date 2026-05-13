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

    public List<PassangerModel> GetByBookingId(int bookingId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT p.* FROM {Table} p
        INNER JOIN Tickets t ON t.passengerId = p.id
        WHERE t.bookingId = @BookingId;";

        return connection.Query<PassangerModel>(sql, new { BookingId = bookingId }).ToList();
    }
}