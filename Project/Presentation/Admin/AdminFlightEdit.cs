static class AdminFlight
{
    public static void ShowflightWithEditValues(FlightModel flight)
    {
        while(true)
        {
            Console.WriteLine($"flight number: {flight.FlightNumber}");
            Console.WriteLine($"departure time: {flight.DepartureTime}");
            Console.WriteLine($"arrival time: {flight.ArrivalTime}");
            Console.WriteLine($"Status: {flight.Status}");
            Console.WriteLine("1 to edit departure and arival time");
            Console.WriteLine("2 to edit status");
`           Console.WriteLine("3 to cancel the flight (also cancels the bookings)");
            string? input = Console.ReadLine();
        }
    }
}