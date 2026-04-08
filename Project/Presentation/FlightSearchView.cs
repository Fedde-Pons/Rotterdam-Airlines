public static class FlightSearch
{
    public static void StartSearch()
    {
        FlightList.ShowAllAvailableFlightsShortList();

        Console.WriteLine("\nSEARCH FOR A FLIGHT");

        FlightLogic flightLogic = new FlightLogic();
        List<FlightModel> flights = flightLogic.GetAllFlights();
        
        Console.Write("Enter your departure city: ");
        string searchDeparture = Console.ReadLine();

        Console.Write("Enter your destination city: ");
        string searchDestination = Console.ReadLine();

        List<FlightModel> routeMatches = new List<FlightModel>();

        foreach (FlightModel flight in flights)
        {
            if (flight.DepartureCity.Equals(searchDeparture, StringComparison.OrdinalIgnoreCase) &&
                flight.DestinationCity.Equals(searchDestination, StringComparison.OrdinalIgnoreCase))
            {
                routeMatches.Add(flight);
            }
        }

        if (routeMatches.Count == 0)
        {
            Console.WriteLine("\nSorry, no flights are available for that route.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            Menu.Start();
            return;
        }

        Console.Clear();

        Console.WriteLine($"\nAVAILABLE FLIGHTS: {searchDeparture.ToUpper()} TO {searchDestination.ToUpper()}");
        foreach (FlightModel f in routeMatches)
        {
            Console.WriteLine($"- Flight {f.FlightNumber} departs at {f.DepartureTime}");
        }

        DateTime searchDate;

        while (true)
        {
            Console.Write("\nEnter your travel date (YYYY-MM-DD): ");
            string dateInput = Console.ReadLine();

            if (DateTime.TryParse(dateInput, out searchDate))
            {
                break;
            }
            else 
            {
                Console.WriteLine("Invalid Input, Please follow the exact travel date format!");
            }
        }

        List<FlightModel> finalMatches = new List<FlightModel>();

        foreach (FlightModel f in routeMatches)
        {
            if (DateTime.Parse(f.DepartureTime).Date == searchDate.Date)
            {
                finalMatches.Add(f);
            }
        }

        if (finalMatches.Count == 0)
        {
            Console.WriteLine("\nSorry, no flights found on that specific date.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            Menu.Start();
            return;
        }

        Console.Clear();
        Console.WriteLine($"\n FLIGHTS ON {searchDate.ToShortDateString()}");
        foreach (FlightModel f in finalMatches)
        {
            Console.WriteLine($"Flight Number: {f.FlightNumber} | Price: €{f.BasePrice}");
        }

        Console.WriteLine("\nEnter the flight number for more details/booking. Or type X to return to main menu.");
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
                        Console.WriteLine("\nThank you for booking your flight . (dev note: Booking feature isn't complete yet, coming soon.)");
                        Console.WriteLine("\nPress any key to return to the main menu.");
                        Console.ReadKey();
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