static class FlightList
{
    static public void ShowAllFlights()
    {
        FlightLogic flightLogic = new FlightLogic();

        while (true)
        {
            List<FlightModel> flights = flightLogic.GetAllFlights();

            Console.Clear();

            if (flights == null || flights.Count == 0)
            {
                Console.WriteLine("\nThere are currently no available flights.\n");
            }
            else
            {
                flights = flights.OrderBy(f => f.DepartureTime).ToList();

                string header = String.Format(
                    " {0,-4} {1,-10} {2,-20} {3,-20} {4,-18} {5,-18} {6,-22} {7,-10} {8,-10}",
                    "#", "Flight", "From", "To", "Departure", "Arrival", "Aircraft", "Price", "Status");
                string separator = new string('=', header.Length);

                Console.WriteLine($"\n{separator}");
                Console.WriteLine("  DEPARTURES - Rotterdam Airlines");
                Console.WriteLine(separator);
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));

                for (int i = 0; i < flights.Count; i++)
                {
                    var flight = flights[i];
                    string from = $"{flight.DepartureCity} ({flight.DepartureCountry})";
                    string to = $"{flight.DestinationCity} ({flight.DestinationCountry})";
                    string aircraft = $"{flight.AircraftManufacturer} {flight.AircraftModel}";
                    string price = $"€{flight.BasePrice:F2}";

                    Console.WriteLine(String.Format(
                        " {0,-4} {1,-10} {2,-20} {3,-20} {4,-18} {5,-18} {6,-22} {7,-10} {8,-10}",
                        i + 1, flight.FlightNumber, from, to, flight.DepartureTime, flight.ArrivalTime, aircraft, price, flight.Status));
                }

                Console.WriteLine(separator);
                Console.WriteLine($"  Total flights: {flights.Count}\n");
            }

            Console.WriteLine("Press any key to return to the menu... (auto-refresh every 5 seconds)");

            DateTime waitUntil = DateTime.Now.AddSeconds(5);
            while (DateTime.Now < waitUntil)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Menu.Start();
                    return;
                }
                Thread.Sleep(100);
            }
        }
    }
}