/// <summary>
/// this class handels the ticket logic within the project
/// </summary>
public static class TicketLogic
{    
    /// </summary>
    /// creates and returns a ticket model <br/>
    /// ticket only returns null if something went wrong
    /// </summary>  
    public static (bool IsSuccesfull, string message, TicketModel? ticket) CreateTicket(int bookingID, int flightId, int seatId, int passangerID, float price, int extraBaggadeKg)
    {
        try
        {
            TicketModel ticket = new TicketModel(bookingID, flightId, seatId, passangerID, price, extraBaggadeKg);
            return (true, "", ticket);
        }
        catch 
        {
            return (false, "couldnt create a booking", null);
        }
    } 
}