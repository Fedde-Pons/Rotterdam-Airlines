using System;
using Microsoft.Data.Sqlite;

public static class DatabaseSeeder
{
    public static void Run(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        InsertAirports(connection);
        InsertAircrafts(connection);
        InsertSeats(connection);
        InsertFlights(connection);
        InsertAdminAccount(connection);
        InsertAccounts(connection);
        InsertPassengers(connection);
        InsertBookings(connection);
        InsertTickets(connection);
    }

    private static void InsertAccounts(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Accounts (id, emailAddress, phoneNumber, firstName, lastName, dateOfBirth, password, createdAt) VALUES
        (2, 'jan.jansen@example.com', '0612345678', 'Jan', 'Jansen', '1990-05-12', 'password123', '2026-01-01 10:00:00'),
        (3, 'emma.devries@example.com', '0698765432', 'Emma', 'de Vries', '1985-09-23', 'password123', '2026-01-02 11:15:00'),
        (4, 'lucas.bakker@example.com', '0611223344', 'Lucas', 'Bakker', '1995-03-08', 'password123', '2026-01-03 09:30:00'),
        (5, 'noor.vandenberg@example.com', '0644556677', 'Noor', 'van den Berg', '1993-02-14', 'password123', '2026-01-05 08:00:00'),
        (6, 'thomas.smit@example.com', '0633445566', 'Thomas', 'Smit', '1988-07-30', 'password123', '2026-01-06 13:45:00'),
        (7, 'lotte.visser@example.com', '0655667788', 'Lotte', 'Visser', '1997-11-18', 'password123', '2026-01-07 17:20:00'),
        (8, 'daan.meijer@example.com', '0677889900', 'Daan', 'Meijer', '1982-04-05', 'password123', '2026-01-08 09:10:00'),
        (9, 'sara.koning@example.com', '0622334455', 'Sara', 'Koning', '2001-08-22', 'password123', '2026-01-09 12:30:00'),
        (10, 'finn.peters@example.com', '0611998877', 'Finn', 'Peters', '1979-12-01', 'password123', '2026-01-10 15:00:00');";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertPassengers(SqliteConnection connection)
    {
        var passengerCountCmd = new SqliteCommand("SELECT COUNT(*) FROM Passengers", connection);
        long count = (long)passengerCountCmd.ExecuteScalar()!;
        if (count > 0) return;

        string query = @"
        INSERT INTO Passengers (id, firstName, lastName, dateOfBirth, passportNumber) VALUES
        (1, 'Jan', 'Jansen', '1990-05-12', '1234567'),
        (2, 'Sophie', 'Jansen', '1992-07-21', '2345678'),
        (3, 'Emma', 'de Vries', '1985-09-23', '3456789'),
        (4, 'Lucas', 'Bakker', '1995-03-08', '4567890'),
        (5, 'Mila', 'Bakker', '2018-11-04', '5678901'),
        (6, 'Noor', 'van den Berg', '1993-02-14', '6789012'),
        (7, 'Thomas', 'Smit', '1988-07-30', '7890123'),
        (8, 'Lotte', 'Visser', '1997-11-18', '8901234'),
        (9, 'Daan', 'Meijer', '1982-04-05', '9012345'),
        (10, 'Sara', 'Koning', '2001-08-22', '0123456'),
        (11, 'Finn', 'Peters', '1979-12-01', '1122334'),
        (12, 'Anna', 'Peters', '2010-03-15', '2233445'),
        (13, 'Bas', 'van den Berg', '1991-06-27', '3344556'),
        (14, 'Roos', 'Smit', '1986-09-09', '4455667');";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertBookings(SqliteConnection connection)
    {
        var bookings = new[]
        {
            (Id: 1,  AccountId: 2, Date: "2026-03-15 14:20:00", TotalPrice: 260.00, Status: "Confirmed"),
            (Id: 2,  AccountId: 2, Date: "2026-04-01 09:45:00", TotalPrice: 130.00, Status: "Confirmed"),
            (Id: 3,  AccountId: 3, Date: "2026-03-20 16:10:00", TotalPrice: 150.00, Status: "Confirmed"),
            (Id: 4,  AccountId: 3, Date: "2026-04-05 12:00:00", TotalPrice: 175.00, Status: "Cancelled"),
            (Id: 5,  AccountId: 4, Date: "2026-04-10 18:30:00", TotalPrice: 390.00, Status: "Confirmed"),
            (Id: 6,  AccountId: 5, Date: "2026-04-12 10:00:00", TotalPrice: 240.00, Status: "Confirmed"),
            (Id: 7,  AccountId: 5, Date: "2026-04-20 08:30:00", TotalPrice: 115.00, Status: "Confirmed"),
            (Id: 8,  AccountId: 6, Date: "2026-04-15 14:00:00", TotalPrice: 330.00, Status: "Confirmed"),
            (Id: 9,  AccountId: 6, Date: "2026-05-01 09:00:00", TotalPrice: 160.00, Status: "Cancelled"),
            (Id: 10, AccountId: 7, Date: "2026-04-18 17:45:00", TotalPrice: 120.00, Status: "Confirmed"),
            (Id: 11, AccountId: 7, Date: "2026-05-05 11:15:00", TotalPrice: 350.00, Status: "Confirmed"),
            (Id: 12, AccountId: 8, Date: "2026-04-22 13:30:00", TotalPrice: 190.00, Status: "Confirmed"),
            (Id: 13, AccountId: 9, Date: "2026-05-10 16:00:00", TotalPrice: 115.00, Status: "Confirmed"),
            (Id: 14, AccountId: 10, Date: "2026-05-12 07:00:00", TotalPrice: 480.00, Status: "Confirmed"),
            (Id: 15, AccountId: 2, Date: "2026-05-20 10:30:00", TotalPrice: 175.00, Status: "Confirmed"),
        };

        foreach (var b in bookings)
        {
            var bookingExistsCmd = new SqliteCommand("SELECT COUNT(*) FROM Bookings WHERE id = @id", connection);
            bookingExistsCmd.Parameters.AddWithValue("@id", b.Id);
            long exists = (long)bookingExistsCmd.ExecuteScalar()!;
            if (exists > 0) continue;

            string insertQuery = @"
            INSERT INTO Bookings (id, accountId, date, totalPrice, status)
            VALUES (@id, @accountId, @date, @totalPrice, @status);";

            using var cmd = new SqliteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@id", b.Id);
            cmd.Parameters.AddWithValue("@accountId", b.AccountId);
            cmd.Parameters.AddWithValue("@date", b.Date);
            cmd.Parameters.AddWithValue("@totalPrice", b.TotalPrice);
            cmd.Parameters.AddWithValue("@status", b.Status);
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertTickets(SqliteConnection connection)
    {
        var ticketCountCmd = new SqliteCommand("SELECT COUNT(*) FROM Tickets", connection);
        long count = (long)ticketCountCmd.ExecuteScalar()!;
        if (count > 0) return;

        var tickets = new[]
        {
            (BookingId: 1,  FlightId: 1,  SeatId: 1,   PassengerId: 1,  Price: 130.00, ExtraBaggageKg: 0),
            (BookingId: 1,  FlightId: 1,  SeatId: 2,   PassengerId: 2,  Price: 130.00, ExtraBaggageKg: 0),

            (BookingId: 2,  FlightId: 3,  SeatId: 675, PassengerId: 1,  Price: 130.00, ExtraBaggageKg: 0),

            (BookingId: 3,  FlightId: 9,  SeatId: 676, PassengerId: 3,  Price: 150.00, ExtraBaggageKg: 0),

            (BookingId: 4,  FlightId: 4,  SeatId: 181, PassengerId: 3,  Price: 175.00, ExtraBaggageKg: 0),

            (BookingId: 5,  FlightId: 17, SeatId: 681, PassengerId: 4,  Price: 195.00, ExtraBaggageKg: 0),
            (BookingId: 5,  FlightId: 17, SeatId: 682, PassengerId: 5,  Price: 195.00, ExtraBaggageKg: 0),

            // Noor van den Berg: retourvlucht Rotterdam-Londen
            (BookingId: 6,  FlightId: 11, SeatId: 10,  PassengerId: 6,  Price: 120.00, ExtraBaggageKg: 23),
            (BookingId: 6,  FlightId: 11, SeatId: 11,  PassengerId: 13, Price: 120.00, ExtraBaggageKg: 0),

            // Noor: enkelvlucht Rotterdam-Parijs
            (BookingId: 7,  FlightId: 19, SeatId: 20,  PassengerId: 6,  Price: 115.00, ExtraBaggageKg: 0),

            // Thomas Smit: gezin Rotterdam-Stockholm
            (BookingId: 8,  FlightId: 13, SeatId: 50,  PassengerId: 7,  Price: 165.00, ExtraBaggageKg: 23),
            (BookingId: 8,  FlightId: 13, SeatId: 51,  PassengerId: 14, Price: 165.00, ExtraBaggageKg: 0),

            // Thomas: geannuleerd Rotterdam-Rome
            (BookingId: 9,  FlightId: 27, SeatId: 60,  PassengerId: 7,  Price: 160.00, ExtraBaggageKg: 0),

            // Lotte Visser: enkelvlucht Rotterdam-Glasgow
            (BookingId: 10, FlightId: 21, SeatId: 30,  PassengerId: 8,  Price: 120.00, ExtraBaggageKg: 0),

            // Lotte: Rotterdam-Stockholm met extra bagage
            (BookingId: 11, FlightId: 31, SeatId: 700, PassengerId: 8,  Price: 175.00, ExtraBaggageKg: 23),
            (BookingId: 11, FlightId: 31, SeatId: 701, PassengerId: 6,  Price: 175.00, ExtraBaggageKg: 0),

            // Daan Meijer: Rotterdam-Parijs
            (BookingId: 12, FlightId: 35, SeatId: 710, PassengerId: 9,  Price: 190.00, ExtraBaggageKg: 0),

            // Sara Koning: enkelvlucht Rotterdam-Frankfurt
            (BookingId: 13, FlightId: 25, SeatId: 40,  PassengerId: 10, Price: 115.00, ExtraBaggageKg: 0),

            // Finn Peters: gezin Rotterdam-Stockholm
            (BookingId: 14, FlightId: 37, SeatId: 720, PassengerId: 11, Price: 240.00, ExtraBaggageKg: 23),
            (BookingId: 14, FlightId: 37, SeatId: 721, PassengerId: 12, Price: 240.00, ExtraBaggageKg: 0),

            // Jan Jansen: Rotterdam-Rome
            (BookingId: 15, FlightId: 45, SeatId: 730, PassengerId: 1,  Price: 175.00, ExtraBaggageKg: 0),
        };

        foreach (var t in tickets)
        {
            string insertQuery = @"
            INSERT INTO Tickets (bookingId, flightId, seatId, passengerId, price, extraBaggageKg)
            VALUES (@bookingId, @flightId, @seatId, @passengerId, @price, @extraBaggageKg);";

            using var cmd = new SqliteCommand(insertQuery, connection);
            cmd.Parameters.AddWithValue("@bookingId", t.BookingId);
            cmd.Parameters.AddWithValue("@flightId", t.FlightId);
            cmd.Parameters.AddWithValue("@seatId", t.SeatId);
            cmd.Parameters.AddWithValue("@passengerId", t.PassengerId);
            cmd.Parameters.AddWithValue("@price", t.Price);
            cmd.Parameters.AddWithValue("@extraBaggageKg", t.ExtraBaggageKg);
            cmd.ExecuteNonQuery();
        }
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
        (7, 'Rotterdam Zuid', 'Wijnhaven 107', 'Rotterdam', 'Netherlands');";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertAircrafts(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Aircrafts (id, manufacturer, model, totalSeats, businessSeats, economySeats) VALUES
        (1, 'Boeing', '737', 198, 18, 180),
        (2, 'Airbus', 'A330', 344, 80, 264),
        (3, 'Boeing', '787', 228, 36, 192);";

        using var cmd = new SqliteCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static void InsertSeats(SqliteConnection connection)
    {
        var seatCountCmd = new SqliteCommand("SELECT COUNT(*) FROM Seats", connection);
        long count = (long)seatCountCmd.ExecuteScalar()!;
        if (count > 0)
        {
            return;
        }

        for (int aircraftId = 1; aircraftId <= 3; aircraftId++)
        {
            string seatLetters = aircraftId == 2 ? "ABCDEFGH" : "ABCDEF";
            int totalRows = aircraftId == 1 ? 33 : aircraftId == 2 ? 43 : 38;
            int businessRows = aircraftId == 1 ? 3 : aircraftId == 2 ? 10 : 6;
            (int first, int second) exitRows = aircraftId == 1 ? (11, 26) : aircraftId == 2 ? (18, 33) : (10, 20);
            char lastLetter = seatLetters[seatLetters.Length - 1];

            for (int row = 1; row <= totalRows; row++)
            {
                foreach (char seatLetter in seatLetters)
                {
                    string seatNumber = $"{row}{seatLetter}";
                    string seatClass = row <= businessRows ? "Business" : "Economy";
                    bool isWindow = seatLetter == 'A' || seatLetter == lastLetter;
                    bool isExit = row == exitRows.first || row == exitRows.second;
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

    private static void InsertAdminAccount(SqliteConnection connection)
    {
        string query = @"
        INSERT OR IGNORE INTO Accounts
        (id, emailAddress, phoneNumber, firstName, lastName, dateOfBirth, password, createdAt, isAdmin)
        VALUES
        (1, 'admin@ra.nl', '0000000000', 'Admin', 'RA', '01/01/1990', 'admin', @createdAt, 1);";

        using var cmd = new SqliteCommand(query, connection);
        cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.ExecuteNonQuery();
    }

    private static void InsertFlights(SqliteConnection connection)
    {
        var flights = new[]
        {
            // 11 juni 2026
            (Id: 1, FlightNumber: "RA101", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-06-11 06:30:00", ArrivalTime: "2026-06-11 07:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 2, FlightNumber: "RA102", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-11 07:15:00", ArrivalTime: "2026-06-11 08:45:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 3, FlightNumber: "RA103", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-11 09:00:00", ArrivalTime: "2026-06-11 11:00:00", BasePrice: 135.00, Status: "Delayed"),
            (Id: 4, FlightNumber: "RA104", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-11 11:30:00", ArrivalTime: "2026-06-11 14:00:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 5, FlightNumber: "RA105", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-11 13:00:00", ArrivalTime: "2026-06-11 14:30:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 6, FlightNumber: "RA106", AircraftId: 3, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-11 15:00:00", ArrivalTime: "2026-06-11 16:30:00", BasePrice: 130.00, Status: "Scheduled"),
            (Id: 7, FlightNumber: "RA107", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-11 16:30:00", ArrivalTime: "2026-06-11 19:00:00", BasePrice: 175.00, Status: "Cancelled"),
            (Id: 8, FlightNumber: "RA108", AircraftId: 2, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-11 18:00:00", ArrivalTime: "2026-06-11 20:30:00", BasePrice: 165.00, Status: "Scheduled"),

            // 12 juni 2026
            (Id: 9, FlightNumber: "RA201", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-12 06:00:00", ArrivalTime: "2026-06-12 07:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 10, FlightNumber: "RA202", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-12 07:30:00", ArrivalTime: "2026-06-12 09:00:00", BasePrice: 125.00, Status: "Delayed"),
            (Id: 11, FlightNumber: "RA203", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-06-12 09:00:00", ArrivalTime: "2026-06-12 10:00:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 12, FlightNumber: "RA204", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-12 10:30:00", ArrivalTime: "2026-06-12 12:45:00", BasePrice: 140.00, Status: "Scheduled"),
            (Id: 13, FlightNumber: "RA205", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-12 12:00:00", ArrivalTime: "2026-06-12 14:30:00", BasePrice: 170.00, Status: "Scheduled"),
            (Id: 14, FlightNumber: "RA206", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-12 14:00:00", ArrivalTime: "2026-06-12 15:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 15, FlightNumber: "RA207", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-12 15:30:00", ArrivalTime: "2026-06-12 17:45:00", BasePrice: 155.00, Status: "Delayed"),
            (Id: 16, FlightNumber: "RA208", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-12 17:30:00", ArrivalTime: "2026-06-12 19:00:00", BasePrice: 130.00, Status: "Scheduled"),

            // 13 juni 2026
            (Id: 17, FlightNumber: "RA301", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-13 06:00:00", ArrivalTime: "2026-06-13 08:30:00", BasePrice: 190.00, Status: "Scheduled"),
            (Id: 18, FlightNumber: "RA302", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-13 07:30:00", ArrivalTime: "2026-06-13 10:00:00", BasePrice: 175.00, Status: "Cancelled"),
            (Id: 19, FlightNumber: "RA303", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-13 08:00:00", ArrivalTime: "2026-06-13 09:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 20, FlightNumber: "RA304", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-13 09:30:00", ArrivalTime: "2026-06-13 11:00:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 21, FlightNumber: "RA305", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-13 11:00:00", ArrivalTime: "2026-06-13 12:30:00", BasePrice: 95.00, Status: "Scheduled"),
            (Id: 22, FlightNumber: "RA306", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-13 12:30:00", ArrivalTime: "2026-06-13 14:00:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 23, FlightNumber: "RA307", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-13 14:30:00", ArrivalTime: "2026-06-13 16:00:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 24, FlightNumber: "RA308", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-13 16:30:00", ArrivalTime: "2026-06-13 19:00:00", BasePrice: 200.00, Status: "Scheduled"),

            // 14 juni 2026
            (Id: 25, FlightNumber: "RA401", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-14 06:30:00", ArrivalTime: "2026-06-14 08:00:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 26, FlightNumber: "RA402", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-14 08:00:00", ArrivalTime: "2026-06-14 09:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 27, FlightNumber: "RA403", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-14 09:00:00", ArrivalTime: "2026-06-14 11:00:00", BasePrice: 145.00, Status: "Scheduled"),
            (Id: 28, FlightNumber: "RA404", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-14 10:30:00", ArrivalTime: "2026-06-14 12:00:00", BasePrice: 130.00, Status: "Scheduled"),
            (Id: 29, FlightNumber: "RA405", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-14 12:00:00", ArrivalTime: "2026-06-14 13:30:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 30, FlightNumber: "RA406", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-14 13:30:00", ArrivalTime: "2026-06-14 16:00:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 31, FlightNumber: "RA407", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-14 15:00:00", ArrivalTime: "2026-06-14 17:30:00", BasePrice: 185.00, Status: "Scheduled"),
            (Id: 32, FlightNumber: "RA408", AircraftId: 2, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-14 17:30:00", ArrivalTime: "2026-06-14 20:00:00", BasePrice: 170.00, Status: "Scheduled"),

            // 15 juni 2026
            (Id: 33, FlightNumber: "RA501", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-06-15 06:00:00", ArrivalTime: "2026-06-15 07:00:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 34, FlightNumber: "RA502", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-15 07:30:00", ArrivalTime: "2026-06-15 09:00:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 35, FlightNumber: "RA503", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-15 08:00:00", ArrivalTime: "2026-06-15 09:30:00", BasePrice: 115.00, Status: "Cancelled"),
            (Id: 36, FlightNumber: "RA504", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-15 10:00:00", ArrivalTime: "2026-06-15 12:30:00", BasePrice: 160.00, Status: "Scheduled"),
            (Id: 37, FlightNumber: "RA505", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-15 11:30:00", ArrivalTime: "2026-06-15 14:00:00", BasePrice: 180.00, Status: "Scheduled"),
            (Id: 38, FlightNumber: "RA506", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-15 13:00:00", ArrivalTime: "2026-06-15 14:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 39, FlightNumber: "RA507", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-15 15:00:00", ArrivalTime: "2026-06-15 16:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 40, FlightNumber: "RA508", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-15 17:30:00", ArrivalTime: "2026-06-15 19:00:00", BasePrice: 135.00, Status: "Scheduled"),

            // 16 juni 2026
            (Id: 41, FlightNumber: "RA601", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-16 06:30:00", ArrivalTime: "2026-06-16 09:00:00", BasePrice: 195.00, Status: "Scheduled"),
            (Id: 42, FlightNumber: "RA602", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-16 07:00:00", ArrivalTime: "2026-06-16 09:30:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 43, FlightNumber: "RA603", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-16 08:30:00", ArrivalTime: "2026-06-16 10:00:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 44, FlightNumber: "RA604", AircraftId: 3, DepartureAirportId: 1, DestinationAirportId: 7, DepartureTime: "2026-06-16 10:00:00", ArrivalTime: "2026-06-16 11:30:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 45, FlightNumber: "RA605", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-16 11:30:00", ArrivalTime: "2026-06-16 13:45:00", BasePrice: 160.00, Status: "Scheduled"),
            (Id: 46, FlightNumber: "RA606", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-16 13:30:00", ArrivalTime: "2026-06-16 15:00:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 47, FlightNumber: "RA607", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-16 15:30:00", ArrivalTime: "2026-06-16 17:00:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 48, FlightNumber: "RA608", AircraftId: 3, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-16 18:00:00", ArrivalTime: "2026-06-16 20:30:00", BasePrice: 185.00, Status: "Scheduled"),

            // 17 juni 2026
            (Id: 49, FlightNumber: "RA701", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-17 06:00:00", ArrivalTime: "2026-06-17 07:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 50, FlightNumber: "RA702", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-17 07:30:00", ArrivalTime: "2026-06-17 09:00:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 51, FlightNumber: "RA703", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-17 08:00:00", ArrivalTime: "2026-06-17 09:30:00", BasePrice: 95.00, Status: "Scheduled"),
            (Id: 52, FlightNumber: "RA704", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-17 10:00:00", ArrivalTime: "2026-06-17 11:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 53, FlightNumber: "RA705", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-17 11:30:00", ArrivalTime: "2026-06-17 13:00:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 54, FlightNumber: "RA706", AircraftId: 3, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-17 13:00:00", ArrivalTime: "2026-06-17 15:30:00", BasePrice: 165.00, Status: "Scheduled"),
            (Id: 55, FlightNumber: "RA707", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-17 15:00:00", ArrivalTime: "2026-06-17 17:30:00", BasePrice: 180.00, Status: "Scheduled"),
            (Id: 56, FlightNumber: "RA708", AircraftId: 2, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-17 18:00:00", ArrivalTime: "2026-06-17 20:30:00", BasePrice: 170.00, Status: "Scheduled"),

            // 18 juni 2026
            (Id: 57, FlightNumber: "RA801", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-06-18 06:00:00", ArrivalTime: "2026-06-18 07:00:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 58, FlightNumber: "RA802", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-18 07:30:00", ArrivalTime: "2026-06-18 09:00:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 59, FlightNumber: "RA803", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 3, DepartureTime: "2026-06-18 09:00:00", ArrivalTime: "2026-06-18 10:30:00", BasePrice: 125.00, Status: "Scheduled"),
            (Id: 60, FlightNumber: "RA804", AircraftId: 3, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-18 10:30:00", ArrivalTime: "2026-06-18 13:00:00", BasePrice: 165.00, Status: "Scheduled"),
            (Id: 61, FlightNumber: "RA805", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-18 12:00:00", ArrivalTime: "2026-06-18 14:30:00", BasePrice: 175.00, Status: "Scheduled"),
            (Id: 62, FlightNumber: "RA806", AircraftId: 2, DepartureAirportId: 1, DestinationAirportId: 7, DepartureTime: "2026-06-18 14:00:00", ArrivalTime: "2026-06-18 15:00:00", BasePrice: 90.00, Status: "Scheduled"),
            (Id: 63, FlightNumber: "RA807", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-18 16:00:00", ArrivalTime: "2026-06-18 17:30:00", BasePrice: 110.00, Status: "Scheduled"),
            (Id: 64, FlightNumber: "RA808", AircraftId: 2, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-18 18:00:00", ArrivalTime: "2026-06-18 19:30:00", BasePrice: 115.00, Status: "Cancelled"),

            // 19 juni 2026
            (Id: 65, FlightNumber: "RA901", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-19 06:30:00", ArrivalTime: "2026-06-19 08:30:00", BasePrice: 140.00, Status: "Scheduled"),
            (Id: 66, FlightNumber: "RA902", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-19 08:00:00", ArrivalTime: "2026-06-19 10:30:00", BasePrice: 155.00, Status: "Scheduled"),
            (Id: 67, FlightNumber: "RA903", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-19 09:00:00", ArrivalTime: "2026-06-19 10:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 68, FlightNumber: "RA904", AircraftId: 3, DepartureAirportId: 6, DestinationAirportId: 7, DepartureTime: "2026-06-19 10:30:00", ArrivalTime: "2026-06-19 12:00:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 69, FlightNumber: "RA905", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-19 12:00:00", ArrivalTime: "2026-06-19 14:30:00", BasePrice: 180.00, Status: "Scheduled"),
            (Id: 70, FlightNumber: "RA906", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-19 13:30:00", ArrivalTime: "2026-06-19 15:00:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 71, FlightNumber: "RA907", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 1, DepartureTime: "2026-06-19 15:30:00", ArrivalTime: "2026-06-19 16:30:00", BasePrice: 100.00, Status: "Scheduled"),
            (Id: 72, FlightNumber: "RA908", AircraftId: 3, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-19 17:30:00", ArrivalTime: "2026-06-19 19:00:00", BasePrice: 125.00, Status: "Scheduled"),

            // 20 juni 2026
            (Id: 73, FlightNumber: "RA1001", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 6, DepartureTime: "2026-06-20 07:00:00", ArrivalTime: "2026-06-20 08:30:00", BasePrice: 105.00, Status: "Scheduled"),
            (Id: 74, FlightNumber: "RA1002", AircraftId: 2, DepartureAirportId: 4, DestinationAirportId: 7, DepartureTime: "2026-06-20 08:30:00", ArrivalTime: "2026-06-20 11:00:00", BasePrice: 150.00, Status: "Scheduled"),
            (Id: 75, FlightNumber: "RA1003", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 2, DepartureTime: "2026-06-20 10:00:00", ArrivalTime: "2026-06-20 11:30:00", BasePrice: 120.00, Status: "Scheduled"),
            (Id: 76, FlightNumber: "RA1004", AircraftId: 3, DepartureAirportId: 5, DestinationAirportId: 7, DepartureTime: "2026-06-20 12:00:00", ArrivalTime: "2026-06-20 14:30:00", BasePrice: 175.00, Status: "Scheduled"),
            (Id: 77, FlightNumber: "RA1005", AircraftId: 1, DepartureAirportId: 7, DestinationAirportId: 4, DepartureTime: "2026-06-20 14:00:00", ArrivalTime: "2026-06-20 16:15:00", BasePrice: 160.00, Status: "Delayed"),
            (Id: 78, FlightNumber: "RA1006", AircraftId: 2, DepartureAirportId: 2, DestinationAirportId: 7, DepartureTime: "2026-06-20 16:00:00", ArrivalTime: "2026-06-20 17:30:00", BasePrice: 115.00, Status: "Scheduled"),
            (Id: 79, FlightNumber: "RA1007", AircraftId: 3, DepartureAirportId: 7, DestinationAirportId: 5, DepartureTime: "2026-06-20 17:30:00", ArrivalTime: "2026-06-20 20:00:00", BasePrice: 190.00, Status: "Scheduled"),
            (Id: 80, FlightNumber: "RA1008", AircraftId: 2, DepartureAirportId: 3, DestinationAirportId: 7, DepartureTime: "2026-06-20 19:00:00", ArrivalTime: "2026-06-20 20:30:00", BasePrice: 130.00, Status: "Scheduled"),
        };

        foreach (var flight in flights)
        {
            var flightExistsCmd = new SqliteCommand("SELECT COUNT(*) FROM Flights WHERE id = @id", connection);
            flightExistsCmd.Parameters.AddWithValue("@id", flight.Id);
            long count = (long)flightExistsCmd.ExecuteScalar()!;

            if (count > 0)
            {
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

        }
    }
} 

// wie ziet deze comment? :) groetjes  somaya araujo valdez 