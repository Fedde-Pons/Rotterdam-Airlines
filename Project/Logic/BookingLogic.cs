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
        db.UpdateBookingStatus(changeInStatus, booking.Id);
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
    }

    public static bool IsCancelled(BookingModel booking)
    {
        return booking.Status == "Cancelled";
    }
    // private bool IsValidateSeatAvailable(Plane plane)
    // {
    //     return false;
    // }
}