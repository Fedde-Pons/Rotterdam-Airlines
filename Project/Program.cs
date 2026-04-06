using Dapper;
using Microsoft.Data.Sqlite;

namespace project;

class Program
{
    static void Main(string[] args)
    {
        // 🔥 DATABASE INITIALIZEN
        var db = new DatabaseInitializer();
        db.Initialize();

        // 👇 jouw bestaande code
        var access = new AccountsAccess();
        access.CreateTable();

        var acc = access.GetByEmail("somaya@test.nl");

        if (acc != null)
        {
            acc.FirstName = "SomayaUpdated";
            access.Update(acc);

            var updated = access.GetByEmail("somaya@test.nl");

            Console.WriteLine("Updated name: " + updated.FirstName);
        }
        else
        {
            Console.WriteLine("Account not found");
        }

        // 🔍 CHECK OF TABELLEN BESTAAN
        using var connection = new SqliteConnection("Data Source=DataSources/project.db");

        var tables = connection.Query<string>(
            "SELECT name FROM sqlite_master WHERE type='table';"
        );

        Console.WriteLine("\n--- TABLES IN DATABASE ---");
        foreach (var table in tables)
        {
            Console.WriteLine(table);
        }
    }
}