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

            Console.WriteLine("\nPress any key to return to the Main Menu...");
            // Hier moet nog meer details over de vlucht komen , Nog in progress!!!
            Console.ReadKey(); 
            Menu.Start();
        }
    }

