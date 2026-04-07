using Microsoft.Data.Sqlite;
using Dapper;

public class DatabaseInitializer
{
    private readonly string _connectionString = "Data Source=DataSources/project.db";

    public void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);

        // ACCOUNTS (heb je al maar voor zekerheid)
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Accounts (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            emailAddress TEXT NOT NULL UNIQUE,
            phoneNumber TEXT,
            firstName TEXT,
            lastName TEXT,
            dateOfBirth TEXT,
            password TEXT,
            createdAt TEXT
        );");

        // BOOKINGS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Bookings (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            accountId INTEGER,
            date TEXT,
            totalPrice REAL,
            status TEXT,
            FOREIGN KEY (accountId) REFERENCES Accounts(id)
        );");

        // PASSENGERS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Passengers (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            firstName TEXT,
            lastName TEXT,
            dateOfBirth TEXT,
            passportNumber TEXT
        );");

        // AIRPORTS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Airports (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT,
            address TEXT,
            city TEXT,
            country TEXT
        );");

        // AIRCRAFTS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Aircrafts (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            manufacturer TEXT,
            model TEXT,
            totalSeats INTEGER,
            businessSeats INTEGER,
            economySeats INTEGER
        );");

        // FLIGHTS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Flights (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            flightNumber TEXT,
            aircraftId INTEGER,
            departureAirportId INTEGER,
            destinationAirportId INTEGER,
            departureTime TEXT,
            arrivalTime TEXT,
            basePrice REAL,
            status TEXT,
            FOREIGN KEY (aircraftId) REFERENCES Aircrafts(id),
            FOREIGN KEY (departureAirportId) REFERENCES Airports(id),
            FOREIGN KEY (destinationAirportId) REFERENCES Airports(id)
        );");

        // SEATS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Seats (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            aircraftId INTEGER,
            seatNumber TEXT,
            rowNumber INTEGER,
            seatClass TEXT,
            isWindow INTEGER,
            isExitRow INTEGER,
            isFirstRow INTEGER,
            isLastRow INTEGER,
            FOREIGN KEY (aircraftId) REFERENCES Aircrafts(id)
        );");

        // TICKETS
        connection.Execute(@"
        CREATE TABLE IF NOT EXISTS Tickets (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            bookingId INTEGER,
            flightId INTEGER,
            seatId INTEGER,
            passengerId INTEGER,
            price REAL,
            extraBaggageKg INTEGER,
            FOREIGN KEY (bookingId) REFERENCES Bookings(id),
            FOREIGN KEY (flightId) REFERENCES Flights(id),
            FOREIGN KEY (seatId) REFERENCES Seats(id),
            FOREIGN KEY (passengerId) REFERENCES Passengers(id)
        );");

        FlightSeeder.Run(_connectionString);
    }
}