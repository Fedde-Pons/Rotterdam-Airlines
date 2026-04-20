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
                    var departures = flights.Where(f => f.DepartureCity == "Rotterdam").ToList();
                    var arrivals = flights.Where(f => f.DestinationCity == "Rotterdam").ToList();

                    var leftBoard = BuildBoardLines("DEPARTURES", departures, isDeparture: true);
                    var rightBoard = BuildBoardLines("ARRIVALS", arrivals, isDeparture: false);
                    PrintBoardsSideBySide(leftBoard, rightBoard);
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

    private static string FormatShortTime(string? dateTime)
    {
        if (DateTime.TryParse(dateTime, out var dt))
            return dt.ToString("dd-MM-yyyy HH:mm:ss");
        return dateTime ?? "";
    }

    private static List<(string Text, string? Status, ConsoleColor Color, int BoardWidth)> BuildBoardLines(
        string title, List<FlightModel> flights, bool isDeparture)
    {
        var lines = new List<(string Text, string? Status, ConsoleColor Color, int BoardWidth)>();

        if (flights.Count == 0)
        {
            string t1 = $"  {title} - Rotterdam Airlines";
            string t2 = "  No flights available.";
            int w = Math.Max(t1.Length, t2.Length);
            lines.Add((t1.PadRight(w), null, ConsoleColor.White, w));
            lines.Add((t2.PadRight(w), null, ConsoleColor.White, w));
            return lines;
        }

        var rows = new List<string[]>();
        foreach (var f in flights)
        {
            string city = isDeparture
                ? $"{f.DestinationCity} ({f.DestinationCountry})"
                : $"{f.DepartureCity} ({f.DepartureCountry})";
            string time = isDeparture ? FormatShortTime(f.DepartureTime) : FormatShortTime(f.ArrivalTime);

            rows.Add(new[]
            {
                f.FlightNumber ?? "",
                city,
                time,
                f.Status ?? ""
            });
        }

        string directionHeader = isDeparture ? "To" : "From";
        string timeHeader = isDeparture ? "Departure" : "Arrival";
        string[] headers = { "Flight", directionHeader, timeHeader, "Status" };
        int[] widths = new int[headers.Length];
        for (int c = 0; c < headers.Length; c++)
        {
            widths[c] = headers[c].Length;
            foreach (var row in rows)
                if (row[c].Length > widths[c])
                    widths[c] = row[c].Length;
        }

        string fmt = " " + string.Join("  ", widths.Select((w, i) => $"{{{i},-{w}}}"));
        string headerLine = string.Format(fmt, headers);
        int boardWidth = headerLine.Length;
        string separator = new string('=', boardWidth);

        lines.Add((separator, null, ConsoleColor.White, boardWidth));
        lines.Add(($"  {title} - Rotterdam Airlines".PadRight(boardWidth), null, ConsoleColor.White, boardWidth));
        lines.Add((separator, null, ConsoleColor.White, boardWidth));
        lines.Add((headerLine, null, ConsoleColor.White, boardWidth));
        lines.Add((new string('-', boardWidth), null, ConsoleColor.White, boardWidth));

        string fmtNoStatus = " " + string.Join("  ", widths.Take(widths.Length - 1).Select((w, i) => $"{{{i},-{w}}}"));

        foreach (var row in rows)
        {
            string status = row[^1];
            string[] rowWithoutStatus = row.Take(row.Length - 1).ToArray();
            string text = string.Format(fmtNoStatus, rowWithoutStatus) + "  ";
            ConsoleColor color = status switch
            {
                "Scheduled" => ConsoleColor.Green,
                "Delayed" => ConsoleColor.Yellow,
                "Cancelled" => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
            lines.Add((text, status.PadRight(widths[^1]), color, boardWidth));
        }

        lines.Add((separator, null, ConsoleColor.White, boardWidth));
        lines.Add(($"  Total: {flights.Count}".PadRight(boardWidth), null, ConsoleColor.White, boardWidth));

        return lines;
    }

    private static void PrintBoardsSideBySide(
        List<(string Text, string? Status, ConsoleColor Color, int BoardWidth)> left,
        List<(string Text, string? Status, ConsoleColor Color, int BoardWidth)> right)
    {
        int maxRows = Math.Max(left.Count, right.Count);
        int leftWidth = left.Count > 0 ? left[0].BoardWidth : 0;
        int gap = 4;

        Console.WriteLine();

        for (int i = 0; i < maxRows; i++)
        {
            if (i < left.Count)
            {
                var l = left[i];
                Console.Write(l.Text);
                if (l.Status != null)
                {
                    Console.ForegroundColor = l.Color;
                    Console.Write(l.Status);
                    Console.ResetColor();
                }
                int written = l.Text.Length + (l.Status?.Length ?? 0);
                if (written < leftWidth)
                    Console.Write(new string(' ', leftWidth - written));
            }
            else
            {
                Console.Write(new string(' ', leftWidth));
            }

            Console.Write(new string(' ', gap));

            if (i < right.Count)
            {
                var r = right[i];
                Console.Write(r.Text);
                if (r.Status != null)
                {
                    Console.ForegroundColor = r.Color;
                    Console.Write(r.Status);
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
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