public static class MyBookings
{
    private static readonly BookingAccess _bookingAccess = new();
    private static readonly TicketAccess _ticketAccess = new();
    private static readonly PassangerAccess _passangerAccess = new();
    private static readonly FlightAccess _flightAccess = new();

    public static void Start()
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            Console.Clear();
            Console.WriteLine("You must be logged in to view your bookings.");
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("             MY BOOKINGS              ");
            Console.WriteLine("======================================\n");

            List<BookingModel> bookings = _bookingAccess.GetByAccountId(AccountsLogic.CurrentAccount.Id);

            if (bookings.Count == 0)
            {
                Console.WriteLine("You have no bookings yet.\n");
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < bookings.Count; i++)
            {
                var b = bookings[i];
                Console.WriteLine($"{i + 1}: Booking #{b.Id}  |  Date: {b.Date}  |  Status: {b.Status}  |  Total: €{b.TotalPrice}");
            }

            Console.WriteLine("\nEnter the number of a booking to view it, or q to return to the main menu:");
            string? input = Console.ReadLine();

            if (input.ToLower() == "q")
                return;

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > bookings.Count)
            {
                Console.WriteLine("\nInvalid input. Press any key to try again...");
                Console.ReadKey();
                continue;
            }

            ShowBookingDetails(bookings[choice - 1]);
        }
    }

    private static void ShowBookingDetails(BookingModel booking)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine($"          BOOKING #{booking.Id}");
            Console.WriteLine("======================================\n");
            Console.WriteLine($"Date:    {booking.Date}");
            Console.WriteLine($"Status:  {booking.Status}");
            Console.WriteLine($"Total:   €{booking.TotalPrice}\n");

            List<TicketModel> tickets = _ticketAccess.GetByBookingId(booking.Id);

            if (tickets.Count == 0)
            {
                Console.WriteLine("This booking has no tickets.\n");
            }
            else
            {
                Console.WriteLine($"Tickets ({tickets.Count}):");
                Console.WriteLine("--------------------------------------");
                for (int i = 0; i < tickets.Count; i++)
                {
                    var t = tickets[i];
                    PassangerModel? passanger = _passangerAccess.GetById(t.PassengerId);
                    SeatModel? seat = _flightAccess.RetrieveSeat(t.SeatId);
                    FlightModel? flight = _flightAccess.RetrieveFlight(t.FlightId);

                    string passangerName = passanger != null
                        ? $"{passanger.FirstName} {passanger.LastName}"
                        : "(unknown passenger)";
                    string seatLabel = seat != null ? seat.SeatNumber : "(unknown)";
                    string flightLabel = flight != null
                        ? $"{flight.FlightNumber} ({flight.DepartureCity} -> {flight.DestinationCity})"
                        : $"Flight #{t.FlightId}";

                    Console.WriteLine($"  Ticket {i + 1}");
                    Console.WriteLine($"    Flight:    {flightLabel}");
                    Console.WriteLine($"    Passenger: {passangerName}");
                    Console.WriteLine($"    Seat:      {seatLabel}");
                    Console.WriteLine($"    Price:     €{t.Price}");
                    Console.WriteLine($"    Baggage:   {t.ExtraBaggageKg} kg extra");
                    Console.WriteLine();
                }
            }

            bool isCancelled = booking.Status == "Cancelled";

            if (!isCancelled)
                Console.WriteLine("1: Cancel this booking");
            Console.WriteLine($"{(isCancelled ? "1" : "2")}: Back to my bookings");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

            string? input = Console.ReadLine();

            if (isCancelled)
            {
                if (input == "1")
                    return;
                Console.WriteLine("\nInvalid input. Press any key to try again...");
                Console.ReadKey();
            }
            else
            {
                switch (input)
                {
                    case "1":
                        if (ConfirmCancellation(booking, tickets))
                            return;
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("\nInvalid input. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    private static bool ConfirmCancellation(BookingModel booking, List<TicketModel> tickets)
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("         CANCEL BOOKING               ");
        Console.WriteLine("======================================\n");
        Console.WriteLine($"Are you sure you want to cancel booking #{booking.Id}?");
        Console.WriteLine("This action cannot be undone.\n");
        Console.WriteLine("1: Yes, cancel this booking");
        Console.WriteLine("2: No, keep this booking");
        Console.WriteLine("\nPlease enter the number of the option you would like to choose:");

        string? input = Console.ReadLine();
        if (input != "1")
        {
            Console.WriteLine("\nBooking was not cancelled. Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        _bookingAccess.UpdateBookingStatus("Cancelled", booking.Id);

        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("         BOOKING CANCELLED            ");
        Console.WriteLine("======================================\n");
        Console.WriteLine($"Booking #{booking.Id} has been cancelled.");
        Console.WriteLine("\nPress any key to return to your bookings...");
        Console.ReadKey();
        return true;
    }
}
