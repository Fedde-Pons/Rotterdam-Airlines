public class AdminFlightMenu
{
    public static void Start()
    {
        FlightLogic flightLogic = new FlightLogic();

        Console.Clear();

        Console.WriteLine("=== ADD NEW FLIGHT ===");

        Console.Write("Flight Number: ");
        string flightNumber = Console.ReadLine();

        Console.Write("Aircraft Id: ");
        string aircraftId = Console.ReadLine();

        Console.Write("Departure Airport Id: ");
        string departureAirportId = Console.ReadLine();

        Console.Write("Destination Airport Id: ");
        string destinationAirportId = Console.ReadLine();

        Console.Write("Departure Time (yyyy-MM-dd HH:mm): ");
        string departureTime = Console.ReadLine();

        Console.Write("Arrival Time (yyyy-MM-dd HH:mm): ");
        string arrivalTime = Console.ReadLine();

        Console.Write("Base Price: ");
        string basePrice = Console.ReadLine();

        Console.Write("Status: ");
        string status = Console.ReadLine();

        var result = flightLogic.AddFlight(
            flightNumber,
            aircraftId,
            departureAirportId,
            destinationAirportId,
            departureTime,
            arrivalTime,
            basePrice,
            status
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

        Console.ReadKey();
    }
}