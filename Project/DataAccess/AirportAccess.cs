using Microsoft.Data.Sqlite;
using Dapper;

public class AirportAccess
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";
    private readonly string Table = "Airports";

    public List<AirportModel> GetAllAirports()
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"SELECT * FROM {Table}";
        return connection.Query<AirportModel>(sql).ToList();
    }

    public int WriteAirport(AirportModel airport)
    {
        using var connection = new SqliteConnection(_connectionString);
        string sql = $@"
            INSERT INTO {Table}
            (name, address, city, county)
            VALUES
            (@Name, @Address, @City, @country)
        ";
        return connection.ExecuteScalar<int>(sql, airport);
    }
}