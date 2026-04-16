/// <summary>
/// this class handles booking/ticket logic
/// </summary>
public class BookingLogic
{
    // basic data we need
    private FlightModel Flight;
    private AccountModel Account;
    private PassangerModel Passanger;
    private string Date;

    // constructor to set everything
    public BookingLogic(FlightModel flight, AccountModel account, PassangerModel passanger, string date)
    {
        Flight = flight;
        Account = account;
        Passanger = passanger;
        Date = date;
    }

    /// <summary>
    /// tries to create a booking + ticket
    /// </summary>
    public (bool IsSuccesfull, string ErrorMessage) BookTicket()
    {
        // check if everything is filled in
        if (Flight == null)
            return (false, "Flight is null");

        if (Account == null)
            return (false, "Account is null");

        if (Passanger == null)
            return (false, "Passenger is null");

        if (string.IsNullOrWhiteSpace(Date))
            return (false, "Date is required");

        // get price from flight
        double price = Flight.BasePrice;

        // price must be valid
        if (price <= 0)
            return (false, "Invalid price");

        // make booking
        BookingsModel booking = CreateBooking(Account.Id, Date, price, "ongoing");

        // make ticket for that booking
        TicketModel ticket = CreateTicket(
            booking.Id,
            Flight.Id,
            0, // no seat yet
            Passanger.Id,
            price,
            0 // no extra baggage
        );

        // if everything worked
        return (true, "Booking created successfully");
    }

    // makes a booking object
    private BookingsModel CreateBooking(long accountID, string date, double totalPrice, string status)
    {
        return new BookingsModel(accountID, date, totalPrice, status);
    }

    // makes a ticket object
    private TicketModel CreateTicket(long bookingId, long flightId, long seatId, long passengerId, double price, int extraBaggageKg)
    {
        return new TicketModel(bookingId, flightId, seatId, passengerId, price, extraBaggageKg);
    }
}