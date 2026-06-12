static class AdminBookings
{
    private static string ColorStatus(string status)
    {
        if (status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            return $"\x1b[31m{status}\x1b[0m";
        if (status.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
            return $"\x1b[32m{status}\x1b[0m";
        if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            return $"\x1b[38;5;208m{status}\x1b[0m";
        return status;
    }

    public static void Show()
    {
        AccountsLogic accountsLogic = new();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Booking Management ===\n");

            List<BookingModel> bookings = BookingLogic.GetPastBookings();
            List<AccountModel> accounts = accountsLogic.GetAll();

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                Console.WriteLine("\nPress any key to return to the Admin Dashboard...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"{"#",-6} {"Booking Nr.",-12} {"Account Name",-24} {"Date",-22} {"Total Price",-14} {"Passengers",-12} {"Status"}");
            Console.WriteLine(new string('-', 100));
            for (int i = 0; i < bookings.Count; i++)
            {
                BookingModel booking = bookings[i];
                AccountModel? account = accounts.FirstOrDefault(a => a.Id == booking.AccountId);
                string accountName = account != null ? account.FullName : $"Account {booking.AccountId}";
                int passengerCount = TicketLogic.GetTicketsForBooking(booking.Id).Count;
                Console.WriteLine($"{i + 1,-6} {booking.Id,-12} {accountName,-24} {booking.Date,-22} {"€" + booking.TotalPrice.ToString("F2"),-14} {passengerCount,-12} {ColorStatus(booking.Status)}");
            }

            Console.WriteLine("\nEnter a number to view details, or press Enter to go back:");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                return;

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= bookings.Count)
            {
                ShowDetails(bookings[choice - 1], accounts);
            }
            else
            {
                Console.WriteLine($"Please enter a number between 1 and {bookings.Count}. Press any key to try again...");
                Console.ReadKey();
            }
        }
    }

    private static void ShowDetails(BookingModel booking, List<AccountModel> accounts)
    {
        FlightLogic flightLogic = new();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Booking Details ===\n");

            AccountModel? account = accounts.FirstOrDefault(a => a.Id == booking.AccountId);
            string accountName = account != null ? account.FullName : $"Account {booking.AccountId}";

            Console.WriteLine($"Booking Nr.: {booking.Id}");
            Console.WriteLine($"Account:     {accountName}");
            Console.WriteLine($"Date:        {booking.Date}");
            Console.WriteLine($"Total Price: €{booking.TotalPrice:F2}");
            Console.WriteLine($"Status:      {ColorStatus(booking.Status)}\n");

            List<TicketModel> tickets = TicketLogic.GetTicketsForBooking(booking.Id);

            if (tickets.Count == 0)
            {
                Console.WriteLine("This booking has no tickets.\n");
            }
            else
            {
                Console.WriteLine($"Tickets ({tickets.Count}):");
                Console.WriteLine(new string('-', 50));
                for (int i = 0; i < tickets.Count; i++)
                {
                    TicketModel t = tickets[i];
                    PassangerModel? passenger = PassangerLogic.GetById(t.PassengerId);
                    SeatModel? seat = flightLogic.GetSeatById(t.SeatId);
                    FlightModel? flight = flightLogic.GetFlightById(t.FlightId);

                    string passengerName = passenger != null
                        ? $"{passenger.FirstName} {passenger.LastName}"
                        : "(unknown passenger)";

                    Console.WriteLine($"  ── Ticket {i + 1} ──────────────────────");
                    Console.WriteLine($"  Passenger:   {passengerName}");
                    if (flight != null)
                    {
                        Console.WriteLine($"  Flight:      {flight.FlightNumber}");
                        Console.WriteLine($"  From:        {flight.DepartureAirportName} ({flight.DepartureCity})");
                        Console.WriteLine($"  Departure:   {flight.DepartureTime}");
                        Console.WriteLine($"  To:          {flight.DestinationAirportName} ({flight.DestinationCity})");
                        Console.WriteLine($"  Arrival:     {flight.ArrivalTime}");
                    }
                    else
                    {
                        Console.WriteLine($"  Flight:      #{t.FlightId}");
                    }
                    Console.WriteLine($"  Seat:        {(seat != null ? $"{seat.SeatNumber} ({seat.Seatclass})" : "(unknown)")}");
                    Console.WriteLine($"  Price:       €{t.Price:F2}");
                    string baggageLabel = t.ExtraBaggageKg > 0
                        ? $"25 kg (+{t.ExtraBaggageKg} kg extra)"
                        : "25 kg";
                    Console.WriteLine($"  Baggage:     {baggageLabel}");
                    Console.WriteLine($"  Check-in:    {(t.IsCheckedIn ? "\x1b[32mChecked In\x1b[0m" : "\x1b[31mNot Checked In\x1b[0m")}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("1: Edit status");
            Console.WriteLine("2: Back to bookings list");
            Console.WriteLine("\nPlease enter your choice:");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                EditStatus(booking);
                booking = BookingLogic.GetById(booking.Id) ?? booking;
            }
            else if (input == "2")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid input. Press any key to try again...");
                Console.ReadKey();
            }
        }
    }

    private static void EditStatus(BookingModel booking)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Edit Status — Booking #{booking.Id} ===\n");
            Console.WriteLine($"Current status:  {ColorStatus(booking.Status)}\n");
            Console.WriteLine("1  →  Confirmed");
            Console.WriteLine("2  →  Cancelled\n");
            Console.Write("Choose a status to change the booking to, or enter 'q' to cancel the edit process: ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    BookingLogic.EditBookingStatus(booking, "Confirmed");
                    booking.Status = "Confirmed";
                    Console.WriteLine($"\nStatus successfully changed to {ColorStatus("Confirmed")}. Press any key to continue...");
                    Console.ReadKey();
                    return;
                case "2":
                    BookingLogic.EditBookingStatus(booking, "Cancelled");
                    booking.Status = "Cancelled";
                    Console.WriteLine($"\nStatus successfully changed to {ColorStatus("Cancelled")}. Press any key to continue...");
                    Console.ReadKey();
                    return;
                case "q":
                    return;
                default:
                    Console.WriteLine("\nInvalid input. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
