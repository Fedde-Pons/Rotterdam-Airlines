public class AdminFlightMenu
{
    public static void Start()
    {
        FlightLogic flightLogic = new FlightLogic();
        var aircrafts = flightLogic.GetAllAircrafts();
        var airports = flightLogic.GetAllAirports();

        try
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW FLIGHT ===");
            Console.WriteLine("Press Esc at any time to cancel.\n");

            string flightNumber = ReadFlightNumber(flightLogic);
            int aircraftIndex = ReadAircraftChoice(aircrafts);
            int departureIndex = ReadDepartureAirport(airports);
            string departureTime = ReadDepartureDateTime();
            int arrivalIndex = ReadArrivalAirport(airports, departureIndex);
            string arrivalTime = ReadArrivalDateTime(departureTime);
            string basePrice = ReadBasePrice();

            PrintOverview(flightNumber, aircrafts, aircraftIndex, airports, departureIndex, departureTime, arrivalIndex, arrivalTime, basePrice);

            Console.Write("Do you want to confirm this flight? (Y/N): ");
            string confirm = Console.ReadLine()?.Trim().ToLower();

            if (confirm != "y")
            {
                Console.Clear();
                Console.WriteLine("Flight not added.");
                Console.WriteLine("Press any key to return to Flight Management...");
                Console.ReadKey();
                return;
            }

            var result = flightLogic.AddFlight(
                flightNumber,
                aircrafts[aircraftIndex].Id.ToString(),
                airports[departureIndex].Id.ToString(),
                airports[arrivalIndex].Id.ToString(),
                departureTime,
                arrivalTime,
                basePrice
            );

            Console.WriteLine();
            if (result.Success)
            {
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                Console.WriteLine("ERROR: " + result.ErrorMessage);
            }

            Console.WriteLine("Press any key to return to Flight Management...");
            Console.ReadKey();
        }
        catch (OperationCanceledException)
        {
            Console.Write("\x1b[3J");
            Console.Clear();
            Console.WriteLine("Flight creation cancelled.");
            Console.WriteLine("Press any key to return to Flight Management...");
            Console.ReadKey();
        }
    }

    private static void PrintOverview(string flightNumber, List<AircraftModel> aircrafts, int aircraftIndex, List<AirportModel> airports, int departureIndex, string departureTime, int arrivalIndex, string arrivalTime, string basePrice)
    {
        var ac = aircrafts[aircraftIndex];
        var dep = airports[departureIndex];
        var arr = airports[arrivalIndex];

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("=== FLIGHT OVERVIEW ===");
        Console.WriteLine($"  1. Flight Number : {flightNumber}");
        Console.WriteLine($"  2. Aircraft      : {ac.Manufacturer} {ac.Model}  (Business: {ac.BusinessSeats} | Economy: {ac.EconomySeats})");
        Console.WriteLine($"  3. Departure     : {dep.Name} — {dep.City}, {dep.Country}");
        Console.WriteLine($"  4. Departure Time: {departureTime}");
        Console.WriteLine($"  5. Arrival       : {arr.Name} — {arr.City}, {arr.Country}");
        Console.WriteLine($"  6. Arrival Time  : {arrivalTime}");
        Console.WriteLine($"  7. Base Price    : {basePrice}");
        Console.WriteLine();
    }

    private static string ReadFlightNumber(FlightLogic flightLogic)
    {
        string? error = null;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW FLIGHT ===");
            Console.WriteLine("Press Esc at any time to cancel.\n");
            if (error != null)
                Console.WriteLine(error);
            Console.Write("Flight Number: ");
            string? input = ReadLineWithEscapeSupport();
            if (input == null) throw new OperationCanceledException();
            if (string.IsNullOrWhiteSpace(input))
            {
                error = "Flight number cannot be empty.";
                continue;
            }
            if (flightLogic.FlightNumberExists(input))
            {
                error = $"Flight number '{input}' already exists. Please enter a different number.";
                continue;
            }
            return input;
        }
    }

    private static int ReadAircraftChoice(List<AircraftModel> aircrafts)
    {
        Console.Clear();
        Console.WriteLine("=== ADD NEW FLIGHT ===");
        Console.WriteLine("Press Esc at any time to cancel.\n");
        Console.WriteLine("Select Aircraft Type:");
        for (int i = 0; i < aircrafts.Count; i++)
        {
            var a = aircrafts[i];
            Console.WriteLine($"  {i + 1}. {a.Manufacturer} {a.Model}  (Business: {a.BusinessSeats} | Economy: {a.EconomySeats})");
        }
        return ReadMenuChoice(aircrafts.Count);
    }

    private static int ReadDepartureAirport(List<AirportModel> airports)
    {
        Console.Clear();
        Console.WriteLine("=== ADD NEW FLIGHT ===");
        Console.WriteLine("Press Esc at any time to cancel.\n");
        Console.WriteLine("Select Departure Airport:");
        for (int i = 0; i < airports.Count; i++)
        {
            var ap = airports[i];
            Console.WriteLine($"  {i + 1}. {ap.Name} — {ap.City}, {ap.Country}");
        }
        return ReadMenuChoice(airports.Count);
    }

    private static string ReadDepartureDateTime()
    {
        string? error = null;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW FLIGHT ===");
            Console.WriteLine("Press Esc at any time to cancel.\n");
            if (error != null)
                Console.WriteLine(error);
            string date = ReadDate("Departure Date (yyyy-MM-dd): ");
            string time = ReadTime("\nDeparture Time (HH:mm): ");
            string combined = $"{date} {time}";
            if (DateTime.Parse(combined) > DateTime.Now)
                return combined;
            error = "\nDeparture must be in the future.";
        }
    }

    private static int ReadArrivalAirport(List<AirportModel> airports, int departureIndex)
    {
        var arrivalOptions = airports[departureIndex].Id == 7
            ? airports.Where((ap, i) => i != departureIndex).ToList()
            : airports.Where(ap => ap.Id == 7).ToList();

        Console.Clear();
        Console.WriteLine("=== ADD NEW FLIGHT ===");
        Console.WriteLine("Press Esc at any time to cancel.\n");
        Console.WriteLine("Select Arrival Airport:");
        for (int i = 0; i < arrivalOptions.Count; i++)
        {
            var ap = arrivalOptions[i];
            Console.WriteLine($"  {i + 1}. {ap.Name} — {ap.City}, {ap.Country}");
        }
        int optionIndex = ReadMenuChoice(arrivalOptions.Count);
        return airports.IndexOf(arrivalOptions[optionIndex]);
    }

    private static string ReadArrivalDateTime(string departureTime)
    {
        string? error = null;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW FLIGHT ===");
            Console.WriteLine("Press Esc at any time to cancel.\n");
            if (error != null)
                Console.WriteLine("\n" + error);
            string date = ReadDate("Arrival Date (yyyy-MM-dd): ");
            string time = ReadTime("\nArrival Time (HH:mm): ");
            string combined = $"{date} {time}";
            if (DateTime.Parse(combined) > DateTime.Parse(departureTime))
                return combined;
            error = "The Arrival must be after the Departure, please try again.";
        }
    }

    private static string ReadBasePrice()
    {
        Console.Clear();
        Console.WriteLine("=== ADD NEW FLIGHT ===");
        Console.WriteLine("Press Esc at any time to cancel.\n");
        Console.Write("Base Price: ");
        string? input = ReadLineWithEscapeSupport();
        if (input == null) throw new OperationCanceledException();
        return input;
    }

    private static int ReadMenuChoice(int max)
    {
        while (true)
        {
            Console.Write($"Enter choice (1-{max}): ");
            string? input = ReadLineWithEscapeSupport();
            if (input == null) throw new OperationCanceledException();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= max)
                return choice - 1;
            Console.WriteLine($"\nInvalid choice. Please enter a number between 1 and {max}.");
        }
    }

    private static string ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = ReadLineWithEscapeSupport();
            if (input == null) throw new OperationCanceledException();
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
                return input;
            Console.WriteLine("\nInvalid format. Please use yyyy-MM-dd (e.g. 2026-07-15).");
        }
    }

    private static string ReadTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = ReadLineWithEscapeSupport();
            if (input == null) throw new OperationCanceledException();
            if (DateTime.TryParseExact(input, new[] { "HH:mm", "HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None, out DateTime parsed))
                return parsed.ToString("HH:mm");
            Console.WriteLine("\nInvalid format. Please use HH:mm or HH:mm:ss (e.g. 14:30 or 14:30:00).");
        }
    }

    private static string? ReadLineWithEscapeSupport()
    {
        string input = "";
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
                return null;
            if (key.Key == ConsoleKey.Enter)
                return input;
            if (key.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                input += key.KeyChar;
                Console.Write(key.KeyChar);
            }
        }
    }
}
