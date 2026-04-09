static class FlightList
{
    static public void ShowAllAvailableFlightsList()
    {
        FlightLogic flightLogic = new FlightLogic();
        string previousList = "";

        while (true)
        {
            List<FlightModel> flights = flightLogic.GetAllAvailableFlightsSorted();
            string currentList = FlightLogic.CreateFlightsSummary(flights);

            if (currentList != previousList)
            {
                previousList = currentList;

                Console.Clear();
                Console.Write("\x1b[3J");
                Console.Out.Flush();

                if (flights == null || flights.Count == 0)
                {
                    Console.WriteLine("\nThere are currently no available flights.\n");
                }
                else
                {
                    var rows = new List<string[]>();
                    for (int i = 0; i < flights.Count; i++)
                    {
                        var f = flights[i];
                        rows.Add(new[]
                        {
                            f.FlightNumber ?? "",
                            $"{f.DepartureCity} ({f.DepartureCountry})",
                            $"{f.DestinationCity} ({f.DestinationCountry})",
                            f.DepartureTime ?? "",
                            f.ArrivalTime ?? "",
                            $"{f.AircraftManufacturer} {f.AircraftModel}",
                            $"€{f.BasePrice:F2}",
                            f.Status ?? ""
                        });
                    }

                    string[] headers = { "Flight", "From", "To", "Departure", "Arrival", "Aircraft", "Price", "Status" };
                    int[] widths = new int[headers.Length];
                    for (int c = 0; c < headers.Length; c++)
                    {
                        widths[c] = headers[c].Length;
                        foreach (var row in rows)
                            if (row[c].Length > widths[c])
                                widths[c] = row[c].Length;
                    }

                    string fmt = " " + string.Join("  ", widths.Select((w, i) => $"{{{i},-{w}}}"));
                    string header = string.Format(fmt, headers);
                    string separator = new string('=', header.Length);

                    Console.WriteLine($"\n{separator}");
                    Console.WriteLine("  DEPARTURES - Rotterdam Airlines");
                    Console.WriteLine(separator);
                    Console.WriteLine(header);
                    Console.WriteLine(new string('-', header.Length));

                    string fmtNoStatus = " " + string.Join("  ", widths.Take(widths.Length - 1).Select((w, i) => $"{{{i},-{w}}}"));

                    foreach (var row in rows)
                    {
                        string status = row[^1];
                        string[] rowWithoutStatus = row.Take(row.Length - 1).ToArray();
                        Console.Write(string.Format(fmtNoStatus, rowWithoutStatus) + "  ");
                        WriteColoredStatus(status, widths[^1]);
                        Console.WriteLine();
                    }

                    Console.WriteLine(separator);
                    Console.WriteLine($"  Total flights: {flights.Count}\n");
                }

                Console.WriteLine("Press any key to return to the menu...");
            }

            DateTime waitUntil = DateTime.Now.AddSeconds(5);
            while (DateTime.Now < waitUntil)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Menu.Start();
                    return;
                }
                Thread.Sleep(100);
            }
        }
    }

    private static void WriteColoredStatus(string status, int width)
    {
        ConsoleColor color = status switch
        {
            "Scheduled" => ConsoleColor.Green,
            "Delayed" => ConsoleColor.Yellow,
            "Cancelled" => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
        Console.ForegroundColor = color;
        Console.Write(status.PadRight(width));
        Console.ResetColor();
    }

    static public void ShowAllAvailableFlightsShortList()
    {
        FlightLogic flightLogic = new FlightLogic();
        List<FlightModel> flights = flightLogic.GetAllAvailableFlightsSorted();

        Console.Clear();
        Console.Write("\x1b[3J");
        Console.Out.Flush();

        if (flights == null || flights.Count == 0)
        {
            Console.WriteLine("\nThere are currently no available flights.\n");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            Menu.Start();
            return;
        }

        Console.WriteLine("\nAvailable Flights:");
        foreach (var flight in flights)
        {
            Console.WriteLine($"- {flight.FlightNumber}: {flight.DepartureCity} to {flight.DestinationCity} at {flight.DepartureTime}");
        }
    }
}