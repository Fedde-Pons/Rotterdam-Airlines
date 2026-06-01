using System.Linq.Expressions;

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

            Console.WriteLine("1 edit flight");
            Console.WriteLine("0 go back to previous menu");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    adminEditFlight();
                    break;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("please pick one of the options provided");
                    Console.ReadKey();
                    break;
            }
        }
    }
    private void adminEditFlight()

    {
        while (true)
        {
            FlightLogic flightLogic = new();
            List<FlightModel> flights = flightLogic.GetAllAvailableFlightsSorted();
            string currentList = FlightLogic.CreateFlightsSummary(flights);
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

            Console.WriteLine("put in the flight you want to edit \n press 0 to go back");
            string? input = Console.ReadLine();
            if (input == "0")
            {
                break;
            }
            else if (flights.Where(f => f.FlightNumber == input).ToList().Any()) 
            {
                Console.WriteLine("test");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("please pick one of the options provided");
                Console.ReadKey();
            }
        }
    }
}
