public class FlightLogic
{
    private FlightAccess flightAccess = new FlightAccess();

    public List<FlightModel> GetAllFlights()
    {
        List<FlightModel> FlightList = flightAccess.GetAllFlights();
        return FlightList;

    }

    public List<FlightModel> GetAllAvailableFlightsSorted()
    {
        List<FlightModel> flights = flightAccess.GetAllAvailableFlights();
        return flights.OrderBy(f => f.DepartureTime).ToList();
    }

    public static string CreateFlightsSummary(List<FlightModel>? flights)
    {
        if (flights == null || flights.Count == 0) return "";
        var sb = new System.Text.StringBuilder();
        foreach (var f in flights.OrderBy(f => f.FlightNumber))
            sb.Append($"{f.FlightNumber}|{f.DepartureTime}|{f.ArrivalTime}|{f.BasePrice}|{f.Status}|{f.DepartureCity}|{f.DestinationCity};");
        return sb.ToString();
    }
}