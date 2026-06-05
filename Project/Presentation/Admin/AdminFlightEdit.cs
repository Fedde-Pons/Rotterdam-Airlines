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
            Console.WriteLine("2 to cancel the flight (also cancels the bookings for said flight)");
            Console.WriteLine("3 to adjust the price");
            Console.WriteLine("4 to go back to the previous menu");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    EditArrivalTime(flight);
                    return;
                case "2":
                    Cancelflight(flight);
                    return;
                case "3":
                    EditPrice(flight);
                    return;
                case "4":
                    return;
                case "5":
                    return;
                default:
                    Console.WriteLine("please pick of the selected menu options");
                    break;
            }
        }
    }

    private static void EditArrivalTime(FlightModel flight)
    {
        Console.WriteLine("please enter the new departure time in yyyy-MM-dd HH:mm format");
        string? departure = Console.ReadLine();
        Console.WriteLine("please enter the new arival time in yyyy-MM-dd HH:mm format");
        string? arival = Console.ReadLine();
        (bool isSuccesfull, string ErrorMessage) edittedFlight = FlightLogic.EditFlightTime(flight, departure, arival);
        if (!edittedFlight.isSuccesfull)
        {
            Console.WriteLine("something went wrong");
            Console.WriteLine(edittedFlight.ErrorMessage);
            Console.ReadKey();
        }
        Console.WriteLine("flight has been sucessfully added");
        Console.ReadKey();
    }
    private static void EditPrice(FlightModel flight)
    {
        Console.WriteLine("please enter the new price");
        int price = 0;
        if(int.TryParse(Console.ReadLine(), out price))
        {
            FlightLogic.EditPrice(flight, price);
            Console.WriteLine("succesfully added the price");
            return;
        }
        Console.WriteLine("please put in a number");


    }
    private static void Cancelflight(FlightModel flight)
    {
        Console.Clear();
        FlightLogic.CancelFlight(flight);
        Console.WriteLine("flight got canceled along side the bookings");
        Console.ReadKey();
    }
}