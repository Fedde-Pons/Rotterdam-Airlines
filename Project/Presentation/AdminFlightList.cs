public class AdminFlightList : FlightList
{
    public override void ShowAllAvailableFlightsList()
    {
        FlightLogic flightLogic = new();
        string previousList = "";

        while (true)
        {
            List<FlightModel> flights = flightLogic.GetAllAvailableFlightsSorted();
            string currentList = FlightLogic.CreateFlightsSummary(flights);

            if (currentList != previousList)
            {
                previousList = currentList;

                Console.Clear();
                Console.Write("\x1b[3J");
                Console.Out.Flush();

                if (flights == null || flights.Count == 0)
                {
                    Console.WriteLine("\nThere are currently no available flights.\n");
                }
                else
                {
                    var departures = flights.Where(f => f.DepartureAirportId == 8).ToList();
                    var arrivals = flights.Where(f => f.DestinationAirportId == 8).ToList();

                    var leftBoard = BuildBoardLines("DEPARTURES", departures, isDeparture: true);
                    var rightBoard = BuildBoardLines("ARRIVALS", arrivals, isDeparture: false);
                    PrintBoardsSideBySide(leftBoard, rightBoard);
                }

                Console.WriteLine("Press any key to return to the menu...");
            }
            if (Console.KeyAvailable)
            {
                Console.ReadKey();
                break;
            }
        }
    }
}