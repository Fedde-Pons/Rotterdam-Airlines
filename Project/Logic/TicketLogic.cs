/// <summary>
/// this class handels the ticket logic within the project
/// </summary>
public static class TicketLogic
{    
    /// </summary>
    /// creates and returns a ticket model <br/>
    /// ticket only returns null if something went wrong
    /// </summary>  
    public static TicketModel CreateTicket(int bookingId, int flightId, int seatId, int price, int extraBaggageKg)
    {
        return new TicketModel(bookingId, flightId, seatId, price, extraBaggageKg);
    }

    // only function that can be tested in this file
    public static (bool IsSuccesfull, string message, TicketModel? ticket) CreateTicket(int bookingID, int flightId, int seatId, int passangerID, float price, int extraBaggadeKg)
    {
        try
        {
            TicketModel ticket = new TicketModel(bookingID, flightId, seatId, passangerID, price, extraBaggadeKg);
            return (true, "", ticket);
        }
        catch
        {
            return (false, "Could not create booking", null);
        }
    }

    // cant be tested directly access db cant mocked
    public static List<TicketModel> GetTicketsForBooking(int bookingId)
    {
        TicketAccess db = new();
        return db.GetByBookingId(bookingId);
    }

    // cant be tested directly access db cant mocked
    public static (int businessBooked, int economyBooked) GetSeatOccupancy(int flightId)
    {
        TicketAccess db = new();
        return db.GetSeatOccupancyByFlightId(flightId);
    }

    // cant be tested directly access db cant mocked
    public static void CheckIn(List<TicketModel> tickets)
    {
        TicketAccess db = new();
        foreach (var t in tickets)
        {
            db.UpdateCheckInStatus(t.Id);
            t.IsCheckedIn = true;
        }
    }
    
    // Cant be tested cant mock Datetime.now (will cause tests to randomly fail )
    public static (bool isOpen, string message) GetCheckInStatus(string departureTime)
    {
        if (!DateTime.TryParse(departureTime, out DateTime departure))
            return (false, "");

        TimeSpan timeUntilFlight = departure - DateTime.Now;

        if (timeUntilFlight.TotalHours > 24)
            return (false, "\n  * Online check-in opens 24 hours before departure.");

        if (timeUntilFlight.TotalHours >= 1)
            return (true, "");

        return (false, "\n  * Online check-in is now closed (closes 1 hour before departure).");
    }
}