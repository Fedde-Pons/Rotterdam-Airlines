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

        //Console.WriteLine("✅ Database gevuld met mock data");
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
        (6, 'Glasgow Airport', 'Paisley PA3', 'Glasgow', 'United Kingdom'),
        (7, 'Rotterdam The Hague Airport', 'Rotterdam Airportplein 60', 'Rotterdam', 'Netherlands');";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertAircrafts(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Aircrafts (id, manufacturer, model, totalSeats, businessSeats, economySeats) VALUES
        (1, 'Boeing', '737', 150, 20, 130),
        (2, 'Airbus', 'A330', 225, 16, 209),
        (3, 'Boeing', '787', 300, 16, 284);";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertSeats(SqliteConnection connection)
    {
        var checkCmd = new SqliteCommand("SELECT COUNT(*) FROM Seats", connection);
        long count = (long)checkCmd.ExecuteScalar()!;
        if (count > 0)
        {
            //Console.WriteLine("⏩ Seats bestaan al, worden overgeslagen.");
            return;
        }

        for (int aircraftId = 1; aircraftId <= 3; aircraftId++)
        {
            int totalRows = aircraftId == 1 ? 30 : aircraftId == 2 ? 35 : 40;
            for (int row = 1; row <= totalRows; row++)
            {
                foreach (char seatLetter in "ABCDEF")
                {
                    string seatNumber = $"{row}{seatLetter}";
                    string seatClass = row <= 3 ? "Business" : "Economy";
                    bool isWindow = seatLetter == 'A' || seatLetter == 'F';
                    bool isExit = row == 10 || row == 20;
                    bool isFirst = row == 1;
                    bool isLast = row == totalRows;

                    string query = @"
                    INSERT INTO Seats 
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
            // 9 april 2026
            (Id: 1, FlightNumber: "RA101", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-09 08:00:00", ArrivalTime: "2026-04-09 10:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 2, FlightNumber: "RA102", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-09 09:00:00", ArrivalTime: "2026-04-09 10:30:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 3, FlightNumber: "RA103", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-09 11:00:00", ArrivalTime: "2026-04-09 13:00:00", BasePrice: 130.00, Status: "Delayed"),
            (Id: 4, FlightNumber: "RA104", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-09 12:30:00", ArrivalTime: "2026-04-09 15:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 5, FlightNumber: "RA105", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-09 14:00:00", ArrivalTime: "2026-04-09 16:30:00", BasePrice: 140.00, Status: "Scheduled"),
            (Id: 6, FlightNumber: "RA106", AircraftId: 3, DepartureAirportId: 1, DestinationAirportId: 7, DepartureTime: "2026-04-09 16:00:00", ArrivalTime: "2026-04-09 19:00:00", BasePrice: 175.00, Status: "Delayed"),
            (Id: 7, FlightNumber: "RA107", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-09 17:30:00", ArrivalTime: "2026-04-09 20:00:00", BasePrice: 160.00, Status: "Cancelled"),
            (Id: 8, FlightNumber: "RA108", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-04-09 18:00:00", ArrivalTime: "2026-04-09 20:30:00", BasePrice: 145.00, Status: "Scheduled"),

            // 10 april 2026
            (Id: 9, FlightNumber: "RA201", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-04-10 06:30:00", ArrivalTime: "2026-04-10 08:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 10, FlightNumber: "RA202", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-10 07:00:00", ArrivalTime: "2026-04-10 09:15:00", BasePrice: 125.00, Status: "Delayed"),
            (Id: 11, FlightNumber: "RA203", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-10 09:00:00", ArrivalTime: "2026-04-10 10:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 12, FlightNumber: "RA204", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-10 10:00:00", ArrivalTime: "2026-04-10 12:15:00", BasePrice: 135.00, Status: "Scheduled"),
            (Id: 13, FlightNumber: "RA205", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-10 11:30:00", ArrivalTime: "2026-04-10 14:00:00", BasePrice: 170.00, Status: "Scheduled"),
            (Id: 14, FlightNumber: "RA206", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-10 13:00:00", ArrivalTime: "2026-04-10 15:30:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 15, FlightNumber: "RA207", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-10 15:00:00", ArrivalTime: "2026-04-10 18:00:00", BasePrice: 185.00, Status: "Delayed"),
            (Id: 16, FlightNumber: "RA208", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-10 16:30:00", ArrivalTime: "2026-04-10 19:00:00", BasePrice: 165.00, Status: "Scheduled"),

            // 11 april 2026
            (Id: 17, FlightNumber: "RA301", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-11 06:00:00", ArrivalTime: "2026-04-11 09:00:00", BasePrice: 190.00, Status: "Scheduled"),
            (Id: 18, FlightNumber: "RA302", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-11 07:30:00", ArrivalTime: "2026-04-11 10:00:00", BasePrice: 175.00, Status: "Cancelled"),
            (Id: 19, FlightNumber: "RA303", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-04-11 08:00:00", ArrivalTime: "2026-04-11 11:00:00", BasePrice: 180.00, Status: "Scheduled"),
            (Id: 20, FlightNumber: "RA304", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-11 09:30:00", ArrivalTime: "2026-04-11 11:30:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 21, FlightNumber: "RA305", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-11 11:00:00", ArrivalTime: "2026-04-11 12:30:00", BasePrice: 95.00, Status: "Scheduled"),
            (Id: 22, FlightNumber: "RA306", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-11 12:00:00", ArrivalTime: "2026-04-11 13:30:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 23, FlightNumber: "RA307", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-11 14:00:00", ArrivalTime: "2026-04-11 15:30:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 24, FlightNumber: "RA308", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-11 16:00:00", ArrivalTime: "2026-04-11 19:00:00", BasePrice: 200.00, Status: "Scheduled"),

            // 12 april 2026
            (Id: 25, FlightNumber: "RA401", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-12 07:00:00", ArrivalTime: "2026-04-12 09:30:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 26, FlightNumber: "RA402", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-12 08:30:00", ArrivalTime: "2026-04-12 11:30:00", BasePrice: 195.00, Status: "Scheduled"),
            (Id: 27, FlightNumber: "RA403", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-12 09:00:00", ArrivalTime: "2026-04-12 12:00:00", BasePrice: 170.00, Status: "Scheduled"),
            (Id: 28, FlightNumber: "RA404", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-12 10:30:00", ArrivalTime: "2026-04-12 13:00:00", BasePrice: 160.00, Status: "Scheduled"),
            (Id: 29, FlightNumber: "RA405", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-12 12:00:00", ArrivalTime: "2026-04-12 14:30:00", BasePrice: 145.00, Status: "Scheduled"),
            (Id: 30, FlightNumber: "RA406", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-12 13:30:00", ArrivalTime: "2026-04-12 15:30:00", BasePrice: 130.00, Status: "Scheduled"),
            (Id: 31, FlightNumber: "RA407", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-12 15:00:00", ArrivalTime: "2026-04-12 18:00:00", BasePrice: 185.00, Status: "Scheduled"),
            (Id: 32, FlightNumber: "RA408", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-12 17:00:00", ArrivalTime: "2026-04-12 18:30:00", BasePrice: 90.00, Status: "Scheduled"),

            // 13 april 2026
            (Id: 33, FlightNumber: "RA501", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-13 06:00:00", ArrivalTime: "2026-04-13 08:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 34, FlightNumber: "RA502", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-13 07:30:00", ArrivalTime: "2026-04-13 09:45:00", BasePrice: 130.00, Status: "Scheduled"),
            (Id: 35, FlightNumber: "RA503", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-13 08:00:00", ArrivalTime: "2026-04-13 09:30:00", BasePrice: 100.00, Status: "Cancelled"),
            (Id: 36, FlightNumber: "RA504", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-13 10:00:00", ArrivalTime: "2026-04-13 13:00:00", BasePrice: 205.00, Status: "Scheduled"),
            (Id: 37, FlightNumber: "RA505", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-13 11:00:00", ArrivalTime: "2026-04-13 13:30:00", BasePrice: 165.00, Status: "Scheduled"),
            (Id: 38, FlightNumber: "RA506", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-13 13:00:00", ArrivalTime: "2026-04-13 15:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 39, FlightNumber: "RA507", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-04-13 15:30:00", ArrivalTime: "2026-04-13 17:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 40, FlightNumber: "RA508", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-13 18:00:00", ArrivalTime: "2026-04-13 20:30:00", BasePrice: 170.00, Status: "Scheduled"),

            // 14 april 2026
            (Id: 41, FlightNumber: "RA601", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-14 06:30:00", ArrivalTime: "2026-04-14 09:30:00", BasePrice: 195.00, Status: "Scheduled"),
            (Id: 42, FlightNumber: "RA602", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-14 07:00:00", ArrivalTime: "2026-04-14 09:15:00", BasePrice: 135.00, Status: "Scheduled"),
            (Id: 43, FlightNumber: "RA603", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-14 08:30:00", ArrivalTime: "2026-04-14 11:00:00", BasePrice: 145.00, Status: "Scheduled"),
            (Id: 44, FlightNumber: "RA604", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-14 10:00:00", ArrivalTime: "2026-04-14 12:30:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 45, FlightNumber: "RA605", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-14 12:00:00", ArrivalTime: "2026-04-14 15:00:00", BasePrice: 190.00, Status: "Scheduled"),
            (Id: 46, FlightNumber: "RA606", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-14 13:30:00", ArrivalTime: "2026-04-14 15:00:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 47, FlightNumber: "RA607", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-14 16:00:00", ArrivalTime: "2026-04-14 18:00:00", BasePrice: 130.00, Status: "Scheduled"),
            (Id: 48, FlightNumber: "RA608", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-14 18:30:00", ArrivalTime: "2026-04-14 21:00:00", BasePrice: 180.00, Status: "Scheduled"),

            // 15 april 2026
            (Id: 49, FlightNumber: "RA701", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-04-15 06:00:00", ArrivalTime: "2026-04-15 09:00:00", BasePrice: 185.00, Status: "Scheduled"),
            (Id: 50, FlightNumber: "RA702", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-15 07:30:00", ArrivalTime: "2026-04-15 09:30:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 51, FlightNumber: "RA703", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-15 08:00:00", ArrivalTime: "2026-04-15 09:30:00", BasePrice: 90.00, Status: "Scheduled"),
            (Id: 52, FlightNumber: "RA704", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-15 10:00:00", ArrivalTime: "2026-04-15 11:30:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 53, FlightNumber: "RA705", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-15 11:30:00", ArrivalTime: "2026-04-15 13:00:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 54, FlightNumber: "RA706", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-15 13:00:00", ArrivalTime: "2026-04-15 16:00:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 55, FlightNumber: "RA707", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-15 15:00:00", ArrivalTime: "2026-04-15 18:00:00", BasePrice: 180.00, Status: "Scheduled"),
            (Id: 56, FlightNumber: "RA708", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-15 17:30:00", ArrivalTime: "2026-04-15 19:00:00", BasePrice: 95.00, Status: "Scheduled"),

            // 16 april 2026 - retourvluchten
            (Id: 57, FlightNumber: "RA801", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-16 06:00:00", ArrivalTime: "2026-04-16 07:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 58, FlightNumber: "RA802", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-04-16 07:30:00", ArrivalTime: "2026-04-16 09:00:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 59, FlightNumber: "RA803", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-04-16 09:00:00", ArrivalTime: "2026-04-16 11:00:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 60, FlightNumber: "RA804", AircraftId: 3, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-04-16 10:30:00", ArrivalTime: "2026-04-16 13:30:00", BasePrice: 160.00, Status: "Scheduled"),
            (Id: 61, FlightNumber: "RA805", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-16 12:00:00", ArrivalTime: "2026-04-16 14:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 62, FlightNumber: "RA806", AircraftId: 2, DepartureAirportId: 1, DestinationAirportId: 7, DepartureTime: "2026-04-16 14:00:00", ArrivalTime: "2026-04-16 15:30:00", BasePrice: 85.00, Status: "Scheduled"),
            (Id: 63, FlightNumber: "RA807", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-16 16:00:00", ArrivalTime: "2026-04-16 18:30:00", BasePrice: 175.00, Status: "Scheduled"),
            (Id: 64, FlightNumber: "RA808", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-16 18:00:00", ArrivalTime: "2026-04-16 21:00:00", BasePrice: 200.00, Status: "Cancelled"),

            // 17 april 2026
            (Id: 65, FlightNumber: "RA901", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-04-17 06:30:00", ArrivalTime: "2026-04-17 08:00:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 66, FlightNumber: "RA902", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-17 08:00:00", ArrivalTime: "2026-04-17 11:00:00", BasePrice: 210.00, Status: "Scheduled"),
            (Id: 67, FlightNumber: "RA903", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-17 09:00:00", ArrivalTime: "2026-04-17 11:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 68, FlightNumber: "RA904", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-04-17 10:30:00", ArrivalTime: "2026-04-17 13:00:00", BasePrice: 160.00, Status: "Scheduled"),
            (Id: 69, FlightNumber: "RA905", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-17 12:00:00", ArrivalTime: "2026-04-17 15:00:00", BasePrice: 175.00, Status: "Scheduled"),
            (Id: 70, FlightNumber: "RA906", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-17 14:00:00", ArrivalTime: "2026-04-17 16:30:00", BasePrice: 165.00, Status: "Scheduled"),

            // 18 april 2026
            (Id: 71, FlightNumber: "RA1001", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-04-18 07:00:00", ArrivalTime: "2026-04-18 09:30:00", BasePrice: 140.00, Status: "Scheduled"),
            (Id: 72, FlightNumber: "RA1002", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-18 08:30:00", ArrivalTime: "2026-04-18 10:30:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 73, FlightNumber: "RA1003", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-04-18 10:00:00", ArrivalTime: "2026-04-18 12:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 74, FlightNumber: "RA1004", AircraftId: 3, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-04-18 12:00:00", ArrivalTime: "2026-04-18 14:30:00", BasePrice: 170.00, Status: "Scheduled"),
            (Id: 75, FlightNumber: "RA1005", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-04-18 14:30:00", ArrivalTime: "2026-04-18 17:30:00", BasePrice: 195.00, Status: "Scheduled"),
            (Id: 76, FlightNumber: "RA1006", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-04-18 16:00:00", ArrivalTime: "2026-04-18 17:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 77, FlightNumber: "RA1007", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-04-18 18:30:00", ArrivalTime: "2026-04-18 21:30:00", BasePrice: 190.00, Status: "Scheduled"),
            (Id: 78, FlightNumber: "RA1008", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-04-18 20:00:00", ArrivalTime: "2026-04-18 22:15:00", BasePrice: 140.00, Status: "Scheduled"),
        };

        foreach (var flight in flights)
        {
            var checkCmd = new SqliteCommand("SELECT COUNT(*) FROM Flights WHERE id = @id", connection);
            checkCmd.Parameters.AddWithValue("@id", flight.Id);
            long count = (long)checkCmd.ExecuteScalar()!;

            if (count > 0)
            {
                //Console.WriteLine($"⏩ Vlucht {flight.FlightNumber} bestaat al, wordt overgeslagen.");
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

            //Console.WriteLine($"✅ Vlucht {flight.FlightNumber} toegevoegd.");
        }
    }
}