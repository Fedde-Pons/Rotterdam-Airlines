/// <summary>
/// this class is for handling the ticket logic within the project
/// </summary>
public class BookingLogic
{   
    private FlightModel Flight;
    private AccountModel Account; 
    private PassangerModel Passanger;
    private string Date;
    public BookingLogic(FlightModel flight, AccountModel account, PassangerModel passanger, string date)
    {
        Flight = flight;
        Account = account;
        Passanger = passanger;
        Date = date;
    }   
    /// <summary>
    /// creates a ticket and stores it in the database if the data is valid.    
    /// </summary>  
    public (bool IsSuccesfull, string ErrorMessage) BookTicket()
    {
        //validates if the booking object can be made
        BookingsModel booking = CreateBooking(Account.Id, Date, Flight.BasePrice, 0);
        //validates if the ticket object can be made
        TicketModel ticket = CreateTicket(booking.Id, Flight.Id, 0,Passanger.Id, Flight.BasePrice, 0);
        return (false, "failed to create the booking");

        

    }

    private BookingsModel CreateBooking(long accountID, string date, double totalPrice, int status)
    {
        BookingsModel booking = new BookingsModel(accountID, date, totalPrice, status);
        return booking;
    }
    private TicketModel CreateTicket(long bookingId, long flightId, long seatId, long passengerId, double price, int extraBaggageKg)
    {
        TicketModel ticket = new TicketModel(bookingId, flightId, seatId, passengerId, price, extraBaggageKg);
        return ticket;
    }
    // private bool IsValidateSeatAvailable(Plane plane)
    // {
    //     return false;
    // }
}