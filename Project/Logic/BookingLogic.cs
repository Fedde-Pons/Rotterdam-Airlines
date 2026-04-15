/// <summary>
/// this class is for handling the ticket logic within the project
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

    // private bool IsValidateSeatAvailable(Plane plane)
    // {
    //     return false;
    // }
}