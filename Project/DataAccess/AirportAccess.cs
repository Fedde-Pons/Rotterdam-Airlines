using Microsoft.Data.Sqlite;
using Dapper;

public class AirportAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Airports";

    public List<AirportModel> GetAllAirports()
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"SELECT * FROM {Table};";
        return connection.Query<AirportModel>(sql).ToList();
    }

    public AirportModel? GetAirportById(long id)
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"SELECT * FROM {Table} WHERE id = @Id;";
        return connection.QueryFirstOrDefault<AirportModel>(sql, new { Id = id });
    }

    public int UpdateAirport(AirportModel airport)
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"
            UPDATE {Table}
            SET name = @Name,
                address = @Address,
                city = @City,
                country = @Country
            WHERE id = @Id;
        ";
        return connection.Execute(sql, airport);
    }

    public int WriteAirport(AirportModel airport)
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"
            INSERT INTO Airports
            (name, address, city, country)
            VALUES
            (@Name, @Address, @City, @Country);
            SELECT last_insert_rowid();
        ";
        return connection.ExecuteScalar<int>(sql, airport);
    }

    public int DeleteAirport(long id)
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"DELETE FROM {Table} WHERE id = @Id;";
        return connection.Execute(sql, new { Id = id });
    }
}