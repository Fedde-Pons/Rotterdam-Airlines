static class AdminFlightEdit
{
    public static void ShowflightWithEditValues(FlightModel flight)
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine($"flight number: {flight.FlightNumber}");
            Console.WriteLine($"departure time: {flight.DepartureTime}");
            Console.WriteLine($"arrival time: {flight.ArrivalTime}");
            Console.WriteLine($"Status: {flight.Status}");
            Console.WriteLine("1 to edit departure and arival time");
            Console.WriteLine("2 to edit status");
            Console.WriteLine("3 to cancel the flight (also cancels the bookings for said flight)");
            Console.WriteLine("0 to go back to the previous menu");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    EditArrivalTime(flight);
                    return;
                case "2":
                    EditArrivalTime(flight);
                    return;
                case "3":
                    Cancelflight(flight);
                    return;
                case "0":
                    return;
                default:
                    Console.WriteLine("please pick of the selected menu options");
                    break;
            }
        }
    }

    private static void EditArrivalTime(FlightModel flight)
    {
        Console.WriteLine("not implemented yet");
        // FlightLogic.EditFlight(flight);
    }
    private static void EditStatusTime(FlightModel flight)
    {
        Console.WriteLine("not implemented yet");
        // FlightLogic.EditFlight(flight);
    }
    private static void Cancelflight(FlightModel flight)
    {
        Console.WriteLine("not implemented yet");
        // FlightLogic.CancelFlight(flight);
    }
}