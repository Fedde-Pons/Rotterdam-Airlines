using Microsoft.Data.Sqlite;
using Dapper;

public class BookingAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Bookings";

    public int Write(BookingModel booking)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        INSERT INTO {Table}
        (accountId, date, totalPrice, status)
        VALUES
        (@AccountId, @Date, @TotalPrice, @Status);

        SELECT last_insert_rowid();";

        return connection.ExecuteScalar<int>(sql, booking);
    }
}