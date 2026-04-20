using Dapper;
using Microsoft.Data.Sqlite;
using Project.DataModels;

namespace Project.DataAccess
{
    public class BookingsAccess
    {
        private string connectionString = "Data Source=DataSources/project.db";

        public void CreateTable()
        {
            using var connection = new SqliteConnection(connectionString);

            string sql = @"
            CREATE TABLE IF NOT EXISTS Bookings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AccountId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                TotalPrice REAL NOT NULL,
                Status TEXT NOT NULL
            );";

            connection.Execute(sql);
        }

        public long Write(BookingsModel booking)
        {
            using var connection = new SqliteConnection(connectionString);

            string sql = @"
            INSERT INTO Bookings (AccountId, Date, TotalPrice, Status)
            VALUES (@AccountId, @Date, @TotalPrice, @Status);
            SELECT last_insert_rowid();";

            long id = connection.ExecuteScalar<long>(sql, booking);
            return id;
        }

        // 🔹 Nieuwe methode toegevoegd zodat BookingLogic kan updaten
        public void Update(BookingsModel booking)
        {
            using var connection = new SqliteConnection(connectionString);

            string sql = @"
            UPDATE Bookings
            SET AccountId = @AccountId,
                Date = @Date,
                TotalPrice = @TotalPrice,
                Status = @Status
            WHERE Id = @Id;";

            connection.Execute(sql, booking);
        }

        public void UpdateStatus(long bookingId, string status)
        {
            using var connection = new SqliteConnection(connectionString);

            string sql = @"
            UPDATE Bookings
            SET Status = @Status
            WHERE Id = @Id;";

            connection.Execute(sql, new { Id = bookingId, Status = status });
        }

        public BookingsModel? GetById(long id)
        {
            using var connection = new SqliteConnection(connectionString);

            string sql = "SELECT * FROM Bookings WHERE Id = @Id;";
            return connection.QueryFirstOrDefault<BookingsModel>(sql, new { Id = id });
        }
    }
}
