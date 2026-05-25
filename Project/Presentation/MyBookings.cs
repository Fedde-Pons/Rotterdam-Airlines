public static class MyBookings
{
    private static readonly FlightLogic _flightLogic = new();

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

            List<BookingModel> bookings = BookingLogic.GetBookingsForAccount(AccountsLogic.CurrentAccount.Id);

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

            List<TicketModel> tickets = TicketLogic.GetTicketsForBooking(booking.Id);

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
                    PassangerModel? passanger = PassangerLogic.GetById(t.PassengerId);
                    SeatModel? seat = _flightLogic.GetSeatById(t.SeatId);
                    FlightModel? flight = _flightLogic.GetFlightById(t.FlightId);

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
                    Console.WriteLine($"    Status:    {(t.IsCheckedIn ? "\x1b[32mChecked In\x1b[0m" : "\x1b[31mNot Checked In\x1b[0m")}");
                    Console.WriteLine();
                }
            }

            bool isCancelled = BookingLogic.IsCancelled(booking);
            
            if (!isCancelled)
                {
                Console.WriteLine("1: Cancel this booking");
                Console.WriteLine("2: Check in online");
                Console.WriteLine("3: Back to my bookings");
                }
            else
                {
                Console.WriteLine("1: Back to my bookings");
                }
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
                        if (ConfirmCancellation(booking))
                            return;
                        break;
                    case "2":
                        PerformCheckIn(booking, tickets);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("\nInvalid input. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    private static bool ConfirmCancellation(BookingModel booking)
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

        BookingLogic.CancelBooking(booking.Id);

        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("         BOOKING CANCELLED            ");
        Console.WriteLine("======================================\n");
        Console.WriteLine($"Booking #{booking.Id} has been cancelled.");
        Console.WriteLine("\nPress any key to return to your bookings...");
        Console.ReadKey();
        return true;
    }

private static void PerformCheckIn(BookingModel booking, List<TicketModel> tickets)
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine($"         ONLINE CHECK-IN #{booking.Id}");
        Console.WriteLine("======================================\n");

        List<TicketModel> pendingTickets = new List<TicketModel>();
        foreach (var t in tickets)
        {
            if (!t.IsCheckedIn) pendingTickets.Add(t);
        }

        if (pendingTickets.Count == 0)
        {
            Console.WriteLine("All passengers for this booking are already checked in!");
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"You are about to check in the following {pendingTickets.Count} passenger(s):\n");
        Console.WriteLine("------------------------------------------------------------");


        foreach (var t in pendingTickets)
        {
            PassangerModel? passanger = PassangerLogic.GetById(t.PassengerId);
            SeatModel? seat = _flightLogic.GetSeatById(t.SeatId);
            FlightModel? flight = _flightLogic.GetFlightById(t.FlightId);

            string passangerName = passanger != null ? $"{passanger.FirstName} {passanger.LastName}" : "(unknown passenger)";
            string seatLabel = seat != null ? seat.SeatNumber : "(unknown)";
            string flightLabel = flight != null ? flight.FlightNumber : $"(Flight #{t.FlightId})";

            
            Console.WriteLine($" * {passangerName,-20} | Flight: {flightLabel,-8} | Seat: {seatLabel}");
        }
        Console.WriteLine("------------------------------------------------------------\n");

        Console.WriteLine("Do you want to confirm check-in for these passengers? (Y/N):");
        
        string? input = Console.ReadLine()?.ToUpper();

        if (input == "Y")
        {
            TicketAccess db = new TicketAccess();
            foreach (var t in pendingTickets)
            {
                db.UpdateCheckInStatus(t.Id); 
                t.IsCheckedIn = true;         
            }

            Console.WriteLine("\nSuccess! Online check-in is confirmed.");
            Console.WriteLine("We have sent the boarding pass(es) to your registered email address.");
        }
        else
        {
            Console.WriteLine("\nCheck-in cancelled.");
        }

        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }
    }
