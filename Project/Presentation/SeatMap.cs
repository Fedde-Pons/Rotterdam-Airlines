using System.Text;

public static class SeatMap
{
    private const string RESET = "\x1b[0m";
    private const string DIM = "\x1b[90m";
    private const string GREEN = "\x1b[92m";
    private const string RED = "\x1b[31m";
    private const string YELLOW = "\x1b[93m";
    private const string BOLD_YELLOW = "\x1b[1;93m";
    private const string BOLD_CYAN = "\x1b[1;96m";
    private const string BOLD_GREEN = "\u001b[1;32m";
    private const string CURSOR = "\x1b[7m"; // reverse for cursor highlight

    public static void ShowSeatMap(FlightModel flight, List<SeatModel> availableSeats)
    {
        Console.Clear();
        AircraftLayoutModel layout = SeatingLogic.GetLayout(flight.AircraftId);
        RenderMap(layout, availableSeats, selectedCode: null);
    }

    // returns the selected seat code, null if cancelled.
    public static string? NavigateSeatMap(FlightModel flight, List<SeatModel> availableSeats, List<SeatModel> allSeats, double economyPrice, double businessPrice)
    {
        AircraftLayoutModel layout = SeatingLogic.GetLayout(flight.AircraftId);

        int curRow = 1;
        int curLetterIdx = 0;
        (int row, int letterIndex)? start = SeatingLogic.GetFirstAvailableSeat(availableSeats, layout);
        if (start != null)
        {
            curRow = start.Value.row;
            curLetterIdx = start.Value.letterIndex;
        }

        Console.CursorVisible = false;
        try
        {
            while (true)
            {
                string curCode = $"{curRow}{layout.Letters[curLetterIdx]}";
                Console.Clear();
                RenderMap(layout, availableSeats, curCode);

                SeatModel? curSeat = allSeats.FirstOrDefault(s => s.SeatNumber == curCode);
                bool isCursorAvailable = availableSeats.Any(s => s.SeatNumber == curCode);
                bool isBusiness = SeatingLogic.IsBusinessRow(curRow, layout);
                string statusColor = isCursorAvailable ? (isBusiness ? YELLOW : GREEN) : RED;
                double price = SeatingLogic.GetSeatPrice(curRow, layout, economyPrice, businessPrice);
                // zorgt ervoor dat de nieuwe legroom seats de juiste prijs tonen.
                if (curSeat != null && (curSeat.IsExitRow || curSeat.IsFirstRow) && curSeat.Seatclass.ToLower() == "economy")
                {
                    price += 15;
                }

                Console.WriteLine();
                Console.WriteLine($"   Flight: {flight.FlightNumber}    " +
                                  $"Business: {YELLOW}€{businessPrice:F2}{RESET}    " +
                                  $"Economy: {GREEN}€{economyPrice:F2}{RESET}");
                Console.WriteLine();

                if (curSeat != null)
                {
                    Console.WriteLine($"   Seat {statusColor}{curCode}{RESET}{(isCursorAvailable ? "" : $"  {RED}(taken){RESET}")}");
                    Console.WriteLine($"   Class:      {(isBusiness ? $"{YELLOW}Business{RESET}" : $"{GREEN}Economy{RESET}")}    Price: €{price:F2}");
                    Console.WriteLine($"   Window:     {(curSeat.IsWindow  ? $"{GREEN}Yes{RESET}" : $"{DIM}No{RESET}")}");
                    Console.WriteLine($"   Leg room:   {(curSeat.IsExitRow  ? $"{GREEN}Yes (+€15){RESET}" : $"{DIM}No{RESET}")}");
                    Console.WriteLine($"   Exit row:   {(curSeat.IsExitRow  ? $"{GREEN}Yes{RESET}" : $"{DIM}No{RESET}")}");
                    Console.WriteLine($"   First row:  {(curSeat.IsFirstRow ? $"{GREEN}Yes{RESET}" : $"{DIM}No{RESET}")}");
                    Console.WriteLine($"   Last row:   {(curSeat.IsLastRow  ? $"{GREEN}Yes{RESET}" : $"{DIM}No{RESET}")}");
                }

                Console.WriteLine();
                Console.WriteLine($"   Navigate: {DIM}arrow keys / WASD{RESET}    Confirm: {GREEN}Enter{RESET}    Cancel: {RED}Escape{RESET}");

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (curRow > 1) curRow--;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        if (curRow < layout.TotalRows) curRow++;
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (curLetterIdx > 0) curLetterIdx--;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (curLetterIdx < layout.Letters.Length - 1) curLetterIdx++;
                        break;
                    case ConsoleKey.Enter:
                        if (isCursorAvailable)
                            return curCode;
                        break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.X:
                        if (ConfirmCancel())
                            return null;
                        break;
                }
            }
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    // user cancel confirmation. returns true if cancellation is confirmed.
    private static bool ConfirmCancel()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine($"Are you sure you want to cancel the booking process?");
        Console.WriteLine($"All unsaved data will be lost.");
        Console.WriteLine();
        Console.Write($"Type {RED}Y{RESET} to cancel or {GREEN}N{RESET} to continue booking: \n");

        string input = Console.ReadLine() ?? "";

        if (input.ToLower() == "y")
            return true;

        return false;
    }

    // picks the cabin renderer based on how many seat letters the layout has:
    // 8 letters is a 2-4-2 cabin, anything else a 3-3 cabin.
    private static void RenderMap(AircraftLayoutModel layout, List<SeatModel> availableSeats, string? selectedCode)
    {
        if (layout.Letters.Length == 8)
            PrintWideBody(layout, availableSeats, selectedCode);
        else if (layout.Letters.Length == 6)
            PrintNarrowBody(layout, availableSeats, selectedCode);
        else
            Console.WriteLine("No seat layout found.");
    }

    private const int CellWidth = 4;
    private const string SeatDivider = " |";
    private const string LeftMargin = "  ";
    private const int LineWidth = 134;

    // renders a 3-3 cabin (Boeing 737 / 787).
    private static void PrintNarrowBody(AircraftLayoutModel layout, List<SeatModel> availableSeats, string? selectedCode = null)
    {
        int businessRows = layout.BusinessRows;
        int totalRows = layout.TotalRows;

        int bizGridWidth = businessRows * CellWidth;
        int econGridWidth = (totalRows - businessRows) * CellWidth;

        // Title
        Console.WriteLine();
        Console.WriteLine(new string(' ', Math.Max(0, (LineWidth - layout.Title.Length) / 2)) + $"{BOLD_CYAN}{layout.Title}{RESET}");
        Console.WriteLine();

        // Class banner
        var sb = new StringBuilder();
        sb.Append(new string(' ', 10));
        sb.Append($"{BOLD_YELLOW}{Center("BUSINESS", bizGridWidth)}{RESET}");
        sb.Append("  ");
        sb.Append($"{BOLD_GREEN}{Center("ECONOMY", econGridWidth)}{RESET}");
        Console.WriteLine(sb);

        // Nose — body inner width = space between the │ borders of a seat row
        int bodyInner = totalRows * CellWidth + 2 + 8;
        Console.WriteLine(LeftMargin + new string(' ', 2) + new string('_', bodyInner - 4) + new string(' ', 2));
        Console.WriteLine(LeftMargin + "╱" + new string(' ', bodyInner) + "╲");

        // Cabin: seat rows
        WriteSeatRow("F", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("E", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("D", totalRows, businessRows, availableSeats, selectedCode);

        // Aisle row with row numbers
        WriteAisleRow(totalRows, businessRows, showRowNumbers: true);

        WriteSeatRow("C", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("B", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("A", totalRows, businessRows, availableSeats, selectedCode);

        // Tail
        Console.WriteLine(LeftMargin + "╲" + new string(' ', bodyInner) + "╱");
        Console.WriteLine(LeftMargin + new string(' ', 2) + new string('‾', bodyInner - 4) + new string(' ', 2));

        WriteExitMarkers(layout);
    }

    // Renders a 2-4-2 cabin (Airbus A330).
    private static void PrintWideBody(AircraftLayoutModel layout, List<SeatModel> availableSeats, string? selectedCode = null)
    {
        int businessRows = layout.BusinessRows;
        int totalRows = layout.TotalRows;

        int bizGridWidth = businessRows * CellWidth;
        int econGridWidth = (totalRows - businessRows) * CellWidth;
        int planeBodyWidth = totalRows * CellWidth;

        // Title
        int displayWidth = bizGridWidth + econGridWidth + 12;
        Console.WriteLine();
        Console.WriteLine(new string(' ', Math.Max(0, (displayWidth - layout.Title.Length) / 2)) + $"{BOLD_CYAN}{layout.Title}{RESET}");
        Console.WriteLine();

        // Class banner
        var sb = new StringBuilder();
        sb.Append(new string(' ', 10));
        sb.Append($"{BOLD_YELLOW}{Center("BUSINESS", bizGridWidth)}{RESET}");
        sb.Append("  ");
        sb.Append($"{BOLD_GREEN}{Center("ECONOMY", econGridWidth)}{RESET}");
        Console.WriteLine(sb);

        // Nose
        Console.WriteLine(LeftMargin + new string(' ', 2) + new string('_', planeBodyWidth + 8) + new string(' ', 2));
        Console.WriteLine(LeftMargin + "╱" + new string(' ', planeBodyWidth + 10) + "╲");

        // Cabin layout A330: 2-4-2
        WriteSeatRow("H", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("G", totalRows, businessRows, availableSeats, selectedCode);
        WriteAisleRow(totalRows, businessRows, showRowNumbers: true);
        WriteSeatRow("F", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("E", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("D", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("C", totalRows, businessRows, availableSeats, selectedCode);
        WriteAisleRow(totalRows, businessRows, showRowNumbers: true);
        WriteSeatRow("B", totalRows, businessRows, availableSeats, selectedCode);
        WriteSeatRow("A", totalRows, businessRows, availableSeats, selectedCode);

        // Tail
        Console.WriteLine(LeftMargin + "╲" + new string(' ', planeBodyWidth + 10) + "╱");
        Console.WriteLine(LeftMargin + new string(' ', 2) + new string('‾', planeBodyWidth + 8) + new string(' ', 2));

        WriteExitMarkers(layout);
    }

    private static void WriteSeatRow(string letter, int totalRows, int businessRows, List<SeatModel> availableSeats, string? selectedCode = null)
    {
        var sb = new StringBuilder();
        sb.Append(LeftMargin);
        sb.Append("│  ");
        sb.Append($"  {letter}  ");
        for (int row = 1; row <= totalRows; row++)
        {
            if (row == businessRows + 1) sb.Append(SeatDivider);
            string code = $"{row}{letter}";
            bool available = availableSeats.Any(s => s.SeatNumber == code);
            bool isBusiness = row <= businessRows;
            bool isCursor = code == selectedCode;
            string color = available ? (isBusiness ? YELLOW : GREEN) : RED;
            string content = available ? code : "XX";
            sb.Append(isCursor ? $"{CURSOR}{color}{content,4}{RESET}" : $"{color}{content,4}{RESET}");
        }
        sb.Append(" │");
        Console.WriteLine(sb);
    }

    private static void WriteAisleRow(int totalRows, int businessRows, bool showRowNumbers)
    {
        var sb = new StringBuilder();
        sb.Append(LeftMargin);
        sb.Append("│  ");
        sb.Append($"{DIM}aisle{RESET}");
        for (int row = 1; row <= totalRows; row++)
        {
            if (row == businessRows + 1) sb.Append(SeatDivider);
            sb.Append(showRowNumbers ? $"{DIM}{row,4}{RESET}" : "    ");
        }
        sb.Append(" │");
        Console.WriteLine(sb);
    }

    private static void WriteExitMarkers(AircraftLayoutModel layout)
    {
        var sb = new StringBuilder();
        sb.Append(new string(' ', 10));
        for (int row = 1; row <= layout.TotalRows; row++)
        {
            if (row == layout.BusinessRows + 1) sb.Append("  ");
            sb.Append((row == layout.FirstExitRow || row == layout.SecondExitRow) ? $"{YELLOW}   ^{RESET}" : "    ");
        }
        Console.WriteLine(sb);

        sb.Clear();
        sb.Append(new string(' ', 10));
        for (int row = 1; row <= layout.TotalRows; row++)
        {
            if (row == layout.BusinessRows + 1) sb.Append("  ");
            sb.Append((row == layout.FirstExitRow || row == layout.SecondExitRow) ? $"{YELLOW} EXIT{RESET}" : "    ");
        }
        Console.WriteLine(sb);
    }

    private static string Center(string text, int width)
    {
        if (text.Length >= width) return text;
        int totalPad = width - text.Length;
        int leftPad = totalPad / 2;
        int rightPad = totalPad - leftPad;
        return new string(' ', leftPad) + text + new string(' ', rightPad);
    }
}
