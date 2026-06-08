using Microsoft.Data.Sqlite;
using Dapper;
using System.Data.Common;
using System.Net;

public class BookingAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Bookings";

    public List<BookingModel> GetAll()
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"SELECT * FROM {Table};";
        return connection.Query<BookingModel>(sql).ToList();
    }

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

    public List<BookingModel> GetByAccountId(int accountId)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT id, accountId, date, totalPrice, status
        FROM {Table}
        WHERE accountId = @AccountId
        ORDER BY id DESC;";

        return connection.Query<BookingModel>(sql, new { AccountId = accountId }).ToList();
    }
    public BookingModel? GetById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"
        SELECT id, accountId, date, totalPrice, status
        FROM {Table}
        WHERE id = @Id;";

        return connection.QueryFirstOrDefault<BookingModel>(sql, new { Id = id });
    }

    public int Cancel(int id)
    {
        using var connection = new SqliteConnection(_connectionString);

        string sql = $@"UPDATE {Table} SET status = 'Cancelled' WHERE id = @Id;";
        return connection.Execute(sql, new { Id = id });
    }
    public int UpdateBookingStatus(string status, int id)
    {
        using var connection =  new SqliteConnection(_connectionString);

        string sql = $@"
        UPDATE {Table}
        SET status = @status
        WHERE id = @id
        ";

        return connection.Execute(sql, new {status, id});
    }
}