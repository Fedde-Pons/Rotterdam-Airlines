public static class SeatMap
{
    public static void ShowSeatMap(FlightModel flight, List<SeatModel> availableSeats)
    {
        Console.Clear();

        switch (flight.AircraftId)
        {
            case 1:
                PrintAircraft("BOEING 737", 10, availableSeats, [["A","B","C"], ["D","E","F"]]);
                break;

            case 2:
                PrintAircraft("AIRBUS 330", 10, availableSeats, [["A","B"], ["C","D","E","F"], ["G","H"]]);
                break;

            case 3:
                PrintAircraft("BOEING 787", 10, availableSeats, [["A","B","C"], ["D","E","F"]]);
                break;

            default:
                Console.WriteLine("No seat layout found.");
                break;
        }
    }

    private static void PrintAircraft(string aircraftName, int rows, List<SeatModel> availableSeats, List<string[]> sections)
    {
        Console.WriteLine();
        Console.WriteLine("                              /^\\");
        Console.WriteLine("                             /   \\");
        Console.WriteLine("                            /_____\\");

        Console.WriteLine("                     _____________________");
        Console.WriteLine("                    /                     \\");
        Console.WriteLine($"                   /     {aircraftName,-15}\\");
        Console.WriteLine("                  /_________________________\\");

        for (int row = 1; row <= rows; row++)
        {
            if (row == 4)
            {
                Console.WriteLine("          ________________| EXIT |________________");
                Console.WriteLine("         /                                            \\");
                Console.WriteLine("========/                                              \\========");
            }

            string seatLine = BuildSeatLine(row, sections, availableSeats);

            Console.WriteLine($"        ||  {seatLine.PadRight(45)} ||");

            if (row == 8)
            {
                Console.WriteLine("========\\                                              /========");
                Console.WriteLine("         \\_______________| EXIT |____________________/");
            }
        }

        Console.WriteLine("                  \\_________________________/");
        Console.WriteLine("                   \\                       /");
        Console.WriteLine("                    \\_____________________/");
        Console.WriteLine("                            ||   ||");
        Console.WriteLine("                        ____||   ||____");
        Console.WriteLine("                       /_______________\\");
        Console.WriteLine();
        Console.WriteLine("Legend: spaces between seats = aisles");
    }

    private static string BuildSeatLine(int row, List<string[]> sections, List<SeatModel> availableSeats)
    {
        List<string> sectionLines = new();

        foreach (string[] section in sections)
        {
            List<string> seats = new();

            foreach (string seatLetter in section)
            {
                string seatCode = $"{row}{seatLetter}";

                bool isAvailable = availableSeats.Any(s => s.SeatNumber == seatCode);

                if (isAvailable)
                    seats.Add($"\x1b[32m{seatCode.PadRight(4)}\x1b[0m");
                else
                    seats.Add($"\x1b[31m{"XX".PadRight(4)}\x1b[0m");
            }

            sectionLines.Add(string.Join("", seats));
        }

        return string.Join("   ", sectionLines);
    }
}