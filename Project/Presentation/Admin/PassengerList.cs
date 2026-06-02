// Usage: PassengerList.Show(flightnumber);
static class PassengerList
{
    private const string RESET = "\x1b[0m";
    private const string GREEN = "\x1b[92m";
    private const string YELLOW = "\x1b[93m";

    public static void Show(FlightModel flight)
    {
        Console.Clear();
        Console.Write("\x1b[3J");
        Console.Out.Flush();

        List<PassangerListEntry> passengers = PassangerLogic.GetPassengerListForFlight(flight.Id);

        Console.WriteLine("+------------------------------------------------------------------------------+");
        Console.WriteLine($"  PASSENGER LIST  -  Flight {flight.FlightNumber}");
        Console.WriteLine($"  {flight.DepartureCity} -> {flight.DestinationCity}   |   Departs: {flight.DepartureTime}");
        Console.WriteLine("+------------------------------------------------------------------------------+\n");

        if (passengers.Count == 0)
        {
            Console.WriteLine("  No passengers have been booked on this flight yet.\n");
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"{"#",-4}{"Name",-28}{"Passport",-12}{"Seat",-8}{"Type",-12}{"Extra KG"}");
        Console.WriteLine(new string('-', 78));

        int index = 1;
        foreach (PassangerListEntry p in passengers)
        {
            bool isBusiness = p.SeatClass.Equals("Business", StringComparison.OrdinalIgnoreCase);
            string color = isBusiness ? YELLOW : GREEN;
            string seatClass = $"{color}{p.SeatClass,-12}{RESET}";

            Console.WriteLine(
                $"{index,-4}{p.FullName,-28}{p.PassportNumber,-12}{p.SeatNumber,-8}{seatClass}{p.ExtraBaggageKg + " kg"}");
            index++;
        }

        Console.WriteLine(new string('-', 78));
        Console.WriteLine($"  Total passengers: {passengers.Count}\n");

        Console.WriteLine("Press any key to go back...");
        Console.ReadKey();
    }
}
