public class AdminFlightList : FlightList
{
    public override void ShowAllAvailableFlightsList()
    {
        while (true)

        {
            FlightLogic flightLogic = new();
            List<FlightModel> flights = flightLogic.GetAllAvailableFlightsSorted();
            string currentList = FlightLogic.CreateFlightsSummary(flights);

            Console.Clear();
            Console.Write("\x1b[3J");
            Console.Out.Flush();

            Console.Clear();
            Console.Write("\x1b[3J");
            Console.Out.Flush();

            if (flights == null || flights.Count == 0)
            {
                Console.WriteLine("\nThere are currently no available flights.\n");
                Console.WriteLine("Press any key to go back to Flight Management");
                Console.ReadKey();
                return;
            }
            else
            {
                var departures = flights.Where(f => f.DepartureAirportId == 7).ToList();
                var arrivals = flights.Where(f => f.DestinationAirportId == 7).ToList();

                var leftBoard = BuildBoardLines("DEPARTURES", departures, isDeparture: true);
                var rightBoard = BuildBoardLines("ARRIVALS", arrivals, isDeparture: false);
                PrintBoardsSideBySide(leftBoard, rightBoard);

                Console.WriteLine("\nEnter flight number to view/edit flight details:");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }

                var matchingFlight = flights.FirstOrDefault(f => f.FlightNumber == input);
                if (matchingFlight != null)
                {
                    AdminFlightEdit.ShowflightWithEditValues(matchingFlight);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Flight not found. Press any key to try again.");
                    Console.ReadKey();
                }
            }
        }
    }
}
