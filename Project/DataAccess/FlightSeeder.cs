using System;
using Microsoft.Data.Sqlite;

public static class FlightSeeder
{
    public static void Run(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        InsertAirports(connection);
        InsertAircrafts(connection);
        InsertSeats(connection);
        InsertFlights(connection);

        Console.WriteLine("✅ Database gevuld met mock data");
    }

    private static void InsertAirports(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Airports (id, name, address, city, country) VALUES
        (1, 'London Heathrow Airport', 'Longford TW6', 'London', 'United Kingdom'),
        (2, 'Frankfurt Airport', '60547 Frankfurt', 'Frankfurt', 'Germany'),
        (3, 'Charles de Gaulle Airport', '95700 Roissy', 'Paris', 'France'),
        (4, 'Leonardo da Vinci Airport', '00054 Fiumicino', 'Rome', 'Italy'),
        (5, 'Stockholm Arlanda Airport', '190 45 Stockholm', 'Stockholm', 'Sweden'),
        (6, 'Glasgow Airport', 'Paisley PA3', 'Glasgow', 'United Kingdom');";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertAircrafts(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Aircrafts (id, manufacturer, model, totalSeats, businessSeats, economySeats) VALUES
        (1, 'Boeing', '737', 180, 20, 160),
        (2, 'Airbus', 'A320', 170, 16, 154);";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertSeats(SqliteConnection connection)
    {
        for (int aircraftId = 1; aircraftId <= 2; aircraftId++)
        {
            for (int row = 1; row <= 30; row++)
            {
                foreach (char seatLetter in "ABCDEF")
                {
                    string seatNumber = $"{row}{seatLetter}";
                    string seatClass = row <= 3 ? "Business" : "Economy";
                    bool isWindow = seatLetter == 'A' || seatLetter == 'F';
                    bool isExit = row == 10 || row == 20;
                    bool isFirst = row == 1;
                    bool isLast = row == 30;

                    string query = @"
                    INSERT OR IGNORE INTO Seats 
                    (aircraftId, seatNumber, rowNumber, seatClass, isWindow, isExitRow, isFirstRow, isLastRow)
                    VALUES
                    (@aircraftId, @seatNumber, @rowNumber, @seatClass, @isWindow, @isExit, @isFirst, @isLast);";

                    using var cmd = new SqliteCommand(query, connection);
                    cmd.Parameters.AddWithValue("@aircraftId", aircraftId);
                    cmd.Parameters.AddWithValue("@seatNumber", seatNumber);
                    cmd.Parameters.AddWithValue("@rowNumber", row);
                    cmd.Parameters.AddWithValue("@seatClass", seatClass);
                    cmd.Parameters.AddWithValue("@isWindow", isWindow ? 1 : 0);
                    cmd.Parameters.AddWithValue("@isExit", isExit ? 1 : 0);
                    cmd.Parameters.AddWithValue("@isFirst", isFirst ? 1 : 0);
                    cmd.Parameters.AddWithValue("@isLast", isLast ? 1 : 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    private static void InsertFlights(SqliteConnection connection)
    {
        var flights = new[]
        {
            (Id: 1, FlightNumber: "BA101", AircraftId: 1, DepartureAirportId: 1, DestinationAirportId: 2, DepartureTime: "2026-05-01 08:00:00", ArrivalTime: "2026-05-01 10:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 2, FlightNumber: "LH102", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 3, DepartureTime: "2026-05-01 09:00:00", ArrivalTime: "2026-05-01 10:30:00", BasePrice: 110.00, Status: "Delayed"),
            (Id: 3, FlightNumber: "AF103", AircraftId: 1, DepartureAirportId: 3, DestinationAirportId: 4, DepartureTime: "2026-05-01 11:00:00", ArrivalTime: "2026-05-01 13:00:00", BasePrice: 130.00, Status: "Completed"),
            (Id: 4, FlightNumber: "AZ104", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 5, DepartureTime: "2026-05-01 12:30:00", ArrivalTime: "2026-05-01 15:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 5, FlightNumber: "SK105", AircraftId: 1, DepartureAirportId: 5, DestinationAirportId: 6, DepartureTime: "2026-05-01 14:00:00", ArrivalTime: "2026-05-01 16:30:00", BasePrice: 140.00, Status: "Cancelled"),
        };

        foreach (var flight in flights)
        {
            var checkCmd = new SqliteCommand("SELECT COUNT(*) FROM Flights WHERE id = @id", connection);
            checkCmd.Parameters.AddWithValue("@id", flight.Id);
            long count = (long)checkCmd.ExecuteScalar()!;

            if (count > 0)
            {
                Console.WriteLine($"⏩ Vlucht {flight.FlightNumber} bestaat al, wordt overgeslagen.");
                continue;
            }

            string insertQuery = @"
            INSERT INTO Flights 
            (id, flightNumber, aircraftId, departureAirportId, destinationAirportId, departureTime, arrivalTime, basePrice, status)
            VALUES
            (@id, @flightNumber, @aircraftId, @departureAirportId, @destinationAirportId, @departureTime, @arrivalTime, @basePrice, @status);";

            using var cmd = new SqliteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@id", flight.Id);
            cmd.Parameters.AddWithValue("@flightNumber", flight.FlightNumber);
            cmd.Parameters.AddWithValue("@aircraftId", flight.AircraftId);
            cmd.Parameters.AddWithValue("@departureAirportId", flight.DepartureAirportId);
            cmd.Parameters.AddWithValue("@destinationAirportId", flight.DestinationAirportId);
            cmd.Parameters.AddWithValue("@departureTime", flight.DepartureTime);
            cmd.Parameters.AddWithValue("@arrivalTime", flight.ArrivalTime);
            cmd.Parameters.AddWithValue("@basePrice", flight.BasePrice);
            cmd.Parameters.AddWithValue("@status", flight.Status);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"✅ Vlucht {flight.FlightNumber} toegevoegd.");
        }
    }
}