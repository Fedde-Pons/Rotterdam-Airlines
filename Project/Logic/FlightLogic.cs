public class FlightLogic
{
    private FlightAccess flightAccess = new FlightAccess();

    public List<FlightModel> GetAllFlights()
    {
        List<FlightModel> FlightList = flightAccess.GetAllAvailableFlights();
        return FlightList;

    }
}