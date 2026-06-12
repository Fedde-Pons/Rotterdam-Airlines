using System.Net;

/// <summary>
/// this class is for handling the booking logic within the project
/// </summary>
public static class BookingLogic
{
    /// <summary>
    /// creates and returns a booking data <br/>
    /// booking only returns null if something went wrong
    /// </summary>
    public static (bool IsSuccesfull, string Message, BookingModel? booking) CreateBooking(int accountId, string date, float totalPrice, string status)
    {
        try
        {
            BookingModel booking = new BookingModel(accountId, date, totalPrice, status);
            return (true, "", booking);
        }
        catch 
        {
            return (false, "couldnt create a booking", null);
        }
    }

    public static void EditBookingStatus(BookingModel booking, string changeInStatus)
    {
        BookingAccess db = new();
        db.UpdateBookingStatus(BookingModel.NormalizeStatus(changeInStatus), booking.Id);
    }

    public static List<BookingModel> GetPastBookings()
    {
        BookingAccess db = new();
        return db.GetAll()
            .Where(b => DateTime.TryParse(b.Date, out DateTime d) && d < DateTime.Now)
            .OrderByDescending(b => b.Date)
            .ToList();
    }

    public static BookingModel? GetById(int id)
    {
        BookingAccess db = new();
        return db.GetById(id);
    }

    public static List<BookingModel> GetBookingsForAccount(int accountId)
    {
        BookingAccess db = new();
        return db.GetByAccountId(accountId);
    }

    public static void CancelBooking(int bookingId)
    {
        BookingAccess db = new();
        db.UpdateBookingStatus("Cancelled", bookingId);

        TicketAccess ticketDb = new();
        ticketDb.ResetCheckInForBooking(bookingId);
    }

    public static bool IsCancelled(BookingModel booking)
    {
        return string.Equals(booking.Status, "Cancelled", StringComparison.OrdinalIgnoreCase);
    }

    public static int SaveBooking(BookingModel booking, List<(PassangerModel passanger, TicketModel ticket)> entries)
    {
        BookingAccess bookingAccess = new();
        PassangerAccess passangerAccess = new();
        TicketAccess ticketAccess = new();

        int bookingId = bookingAccess.Write(booking);

        foreach (var (passanger, ticket) in entries)
        {
            int passangerId = passangerAccess.Write(passanger);
            TicketModel dbTicket = new(bookingId, ticket.FlightId, ticket.SeatId, passangerId, ticket.Price, ticket.ExtraBaggageKg);
            ticketAccess.Write(dbTicket);
        }

        return bookingId;
    }
}