public static class FlightSearch
{
    public static void StartSearch()
    {
        Console.Clear();

        FlightLogic flightLogic = new FlightLogic();
        List<FlightModel> flights = flightLogic.GetAllFlights();
        
        Console.Write("Enter your departure city: ");
        string searchDeparture = Console.ReadLine();

        Console.Write("Enter your destination city: ");
        string searchDestination = Console.ReadLine();

        Console.Write("Enter your travel date (YYYY-MM-DD): ");
        string dateInput = Console.ReadLine();
        DateTime searchDate = DateTime.Parse(dateInput);

        bool flightFound = false;

        foreach (FlightModel flight in flights)
        {
            if (flight.DepartureCity.Equals(searchDeparture, StringComparison.OrdinalIgnoreCase) &&
                flight.DestinationCity.Equals(searchDestination, StringComparison.OrdinalIgnoreCase) &&
                flight.DepartureTime.Date == searchDate.Date) 
            {
                Console.WriteLine($"Flight Number: {flight.FlightNumber}");
                Console.WriteLine($"Route: {flight.DepartureCity} to {flight.DestinationCity}");
                Console.WriteLine($"Departure: {flight.DepartureTime}");
                Console.WriteLine($"Price: €{flight.BasePrice}");
                
                flightFound = true;
            }
        }

        if (flightFound == false)
        {
            Console.WriteLine("Sorry, no flights are available for that route.");
        }

        Console.WriteLine("\nEnter the flight number for more details/booking. Or type X to return to main menu. ");

        string userChoice = Console.ReadLine();

        if (userChoice.Equals("X", StringComparison.OrdinalIgnoreCase))
        {
            Menu.Start();
        }
        else
        {
            ShowFlightDetails(userChoice, flights);
        }
    }

    public static void ShowFlightDetails(string userChoice, List<FlightModel> flights)
    {
        bool specificFlightFound = false;

        foreach (FlightModel specificFlight in flights)
        {
            if (specificFlight.FlightNumber.Equals(userChoice, StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine($"Flight Number: {specificFlight.FlightNumber}");
                Console.WriteLine($"Route: {specificFlight.DepartureCity} to {specificFlight.DestinationCity}");
                Console.WriteLine($"Departure: {specificFlight.DepartureTime}");
                Console.WriteLine($"Economy: €{specificFlight.BasePrice}");
                Console.WriteLine("Available Seats: 42");

                specificFlightFound = true;

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Proceed to Booking");
                Console.WriteLine("2. Return to Main Menu");
                
                while (true)
                {
                    Console.Write("\nEnter your choice (1 or 2): ");
                    string bookingChoice = Console.ReadLine();

                    if (bookingChoice == "1")
                    {
                        // hier komt dan de booking feature i think
                        break;
                    }
                    else if (bookingChoice == "2")
                    {
                        Menu.Start();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please type 1 or 2.");
                    }
                }

                break;
            }
        }

        if (specificFlightFound == false)
        {
            Console.WriteLine("\nSorry, we couldn't find a flight with that number.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            Menu.Start();
        }
    }
}