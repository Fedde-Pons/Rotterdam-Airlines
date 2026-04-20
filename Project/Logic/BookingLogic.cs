using System;
using Project.DataAccess;
using Project.DataModels;

namespace Project.Logic
{
    /// <summary>
    /// This class handles booking/ticket logic
    /// </summary>
    public class BookingLogic
    {
        private BookingsAccess _bookingsAccess = new();

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
        /// Tries to create a booking + ticket
        /// </summary>
        public (bool IsSuccesfull, string ErrorMessage, long BookingId) BookTicket()
        {
            Console.WriteLine("DEBUG: BookTicket is called");

            // check if everything is filled in
            if (Flight == null)
                return (false, "Flight is null", 0);

            if (Account == null)
                return (false, "Account is null", 0);

            if (Passanger == null)
                return (false, "Passenger is null", 0);

            if (string.IsNullOrWhiteSpace(Date))
                return (false, "Date is required", 0);

            // get price from flight
            double price = Flight.BasePrice;

            // price must be valid
            if (price <= 0)
                return (false, "Invalid price", 0);

            // make booking (status = ongoing)
            BookingsModel booking = CreateBooking(Account.Id, Date, price, "ongoing");

            // DB insert
            long bookingId = _bookingsAccess.Write(booking);
            booking.Id = bookingId;

            Console.WriteLine($"DEBUG: Booking saved with id {bookingId}");

            // make ticket for that booking
            TicketModel ticket = CreateTicket(
                booking.Id,
                Flight.Id,
                0, // no seat yet
                Passanger.Id,
                price,
                0 // no extra baggage
            );

            // confirm the booking (price locked)
            ConfirmBooking(booking);

            // if everything worked
            return (true, "Booking created successfully", booking.Id);        }

        /// <summary>
        /// Creates and returns a booking object
        /// </summary>
        private BookingsModel CreateBooking(long accountID, string date, double totalPrice, string status)
        {
            return new BookingsModel(accountID, date, totalPrice, status);
        }

        /// <summary>
        /// Creates and returns a ticket object
        /// </summary>
        private TicketModel CreateTicket(long bookingId, long flightId, long seatId, long passengerId, double price, int extraBaggageKg)
        {
            return new TicketModel(bookingId, flightId, seatId, passengerId, price, extraBaggageKg);
        }

        /// <summary>
        /// Confirms the booking — locks the price so it cannot change afterwards
        /// </summary>
        public void ConfirmBooking(BookingsModel booking)
        {
            booking.Status = "confirmed";
            _bookingsAccess.Update(booking);
            Console.WriteLine($"DEBUG: Booking {booking.Id} confirmed with locked price: {booking.TotalPrice}");
        }

        /// <summary>
        /// Attempts to update a booking price — disallowed when confirmed
        /// </summary>
        public void UpdateBookingPrice(BookingsModel booking, double newPrice)
        {
            if (booking.Status == "confirmed")
            {
                Console.WriteLine("Price cannot be changed after confirmation.");
                return;
            }

            booking.TotalPrice = newPrice;
            _bookingsAccess.Update(booking);
            Console.WriteLine($"DEBUG: Booking {booking.Id} price updated to {newPrice}");
        }
    }
}
