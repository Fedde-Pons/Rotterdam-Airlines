static class FlightList
{
    static public void ShowAllFlights()
    {
        FlightLogic flightLogic = new FlightLogic();
        List<FlightModel> flights = flightLogic.GetAllFlights();

        if (flights == null || flights.Count == 0)
        {
            Console.WriteLine("\nThere are currently no available flights.\n");
            Menu.Start();
            return;
        }

        // Sort flights by departure time
        flights = flights.OrderBy(f => f.DepartureTime).ToList();

        Console.WriteLine("\n========================================");
        Console.WriteLine("        Available Flights");
        Console.WriteLine("========================================\n");

        for (int i = 0; i < flights.Count; i++)
        {
            var flight = flights[i];
            Console.WriteLine($"  [{i + 1}] Flight {flight.FlightNumber}");
            Console.WriteLine($"      From:        {flight.DepartureAirportName}, {flight.DepartureCity} ({flight.DepartureCountry})");
            Console.WriteLine($"      To:          {flight.DestinationAirportName}, {flight.DestinationCity} ({flight.DestinationCountry})");
            Console.WriteLine($"      Time:        {flight.DepartureTime} - {flight.ArrivalTime}");
            Console.WriteLine($"      Aircraft:    {flight.AircraftManufacturer} {flight.AircraftModel}");
            Console.WriteLine($"      Price:       €{flight.BasePrice:F2}");
            Console.WriteLine($"      Status:      {flight.Status}");
            Console.WriteLine("      ----------------------------------------");
        }

        Console.WriteLine($"\n  Total flights found: {flights.Count}\n");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
        Menu.Start();
    }
}