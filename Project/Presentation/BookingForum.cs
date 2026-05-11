using System.Runtime.CompilerServices;

public static class BookingForums
{
    /// <summary>
    /// starts the ui for the booking forum <br/>
    /// this also starts the ui for the passanger and ticket forums
    /// </summary>
    /// <param name="flight"></param>
    /// <param name="date"></param>
   public static void Start(FlightModel flight, string date)
    {
        int accountID = AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : 1; // edited this so we dont need to be logged in atm  
        BookingModel booking = new BookingModel(accountID, date, "ongoing");
        int numberOfTickets = NumberOfTickets();
        List<(PassangerModel passanger, TicketModel ticket, SeatModel seat)> bookingValues = [];

        FlightAccess dbAccess = new FlightAccess();
        var seatData = dbAccess.GetLiveSeatData(flight.Id, flight.AircraftId);
        
        List<SeatModel> availableSeats = seatData.availableSeats;
        int totalSeats = seatData.allSeats.Count;
        int bookedSeats = seatData.bookedSeats;


        for(int i = 0; i < numberOfTickets; i++)
        {
            PassangerModel passanger = CreatePassanger(i + 1, numberOfTickets);
            // seat and price logic goes here
            var seatingResult = SeatingLogic.StartSeatSelection(flight, availableSeats, totalSeats, bookedSeats);

            if (seatingResult == null)
            {
                return;
            }

            SeatModel pickedSeat = seatingResult.Value.seat;
            double finalPrice = seatingResult.Value.price;

            availableSeats.Remove(pickedSeat);
            bookedSeats++;
            
            //TODO: the 0's need to be adjusted based on pricing
            TicketModel ticket = CreateTicket(booking.Id, flight.Id, pickedSeat.Id, (int)finalPrice);
            bookingValues.Add((passanger, ticket, pickedSeat));
        }

        // ── CONFIRMATION STEP ──────────────────────────────────────────
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("        BOOKING CONFIRMATION          ");
        Console.WriteLine("======================================\n");
        Console.WriteLine($"Flight:     {flight.FlightNumber}");
        Console.WriteLine($"From:       {flight.DepartureAirportName} ({flight.DepartureCity}) at {flight.DepartureTime}");
        Console.WriteLine($"To:         {flight.DestinationAirportName} ({flight.DestinationCity}) at {flight.ArrivalTime}");
        Console.WriteLine($"Date:       {date}\n");
        Console.WriteLine($"  #  {"Passenger",-28} {"Seat",-8} {"Class",-12} Price");
        Console.WriteLine($"  -  {new string('-', 28)} {"----",-8} {"-----",-12} -----");

        for (int i = 0; i < bookingValues.Count; i++)
        {
            var (passanger, ticket, seat) = bookingValues[i];
            string name = $"{passanger.FirstName} {passanger.LastName}";
            Console.WriteLine($"  {i + 1}  {name,-28} {seat.SeatNumber,-8} {seat.Seatclass,-12} €{ticket.Price}");
        }

        double totalPrice = bookingValues.Sum(bv => bv.ticket.Price);
        Console.WriteLine($"\n  Total: €{totalPrice}\n");
        Console.WriteLine("Confirm this booking? [Y] Yes  [N] No (cancel)");

        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Y) break;
            if (key.Key == ConsoleKey.N)
            {
                Console.WriteLine("\n\nBooking cancelled. Returning to main menu...");
                Console.ReadKey();
                return;
            }
        }

        // ── SAVE TO DATABASE ───────────────────────────────────────────
        booking.TotalPrice = totalPrice;

        BookingAccess bookingAccess = new();
        PassangerAccess passangerAccess = new();
        TicketAccess ticketAccess = new();

        int bookingId = bookingAccess.Write(booking);

        foreach (var (passanger, ticket, _) in bookingValues)
        {
            int passangerId = passangerAccess.Write(passanger);
            TicketModel dbTicket = new(bookingId, ticket.FlightId, ticket.SeatId, passangerId, ticket.Price, ticket.ExtraBaggageKg);
            ticketAccess.Write(dbTicket);
        }

        // ── TICKET DISPLAY ─────────────────────────────────────────────
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("          BOOKING CONFIRMED!          ");
        Console.WriteLine("======================================\n");
        Console.WriteLine($"Booking reference: #{bookingId}\n");

        for (int i = 0; i < bookingValues.Count; i++)
        {
            var (passanger, ticket, seat) = bookingValues[i];
            Console.WriteLine($"  ── Ticket {i + 1} ──────────────────────");
            Console.WriteLine($"  Passenger:   {passanger.FirstName} {passanger.LastName}");
            Console.WriteLine($"  Flight:      {flight.FlightNumber}");
            Console.WriteLine($"  From:        {flight.DepartureAirportName} ({flight.DepartureCity})");
            Console.WriteLine($"  Departure:   {flight.DepartureTime}");
            Console.WriteLine($"  To:          {flight.DestinationAirportName} ({flight.DestinationCity})");
            Console.WriteLine($"  Arrival:     {flight.ArrivalTime}");
            Console.WriteLine($"  Seat:        {seat.SeatNumber}  ({seat.Seatclass})");
            Console.WriteLine($"  Price:       €{ticket.Price}\n");
        }

        Console.WriteLine($"  Total paid: €{totalPrice}");
    }
    private static int NumberOfTickets()
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("             BOOK A FLIGHT            ");
        Console.WriteLine("======================================\n");
        while (true)
        {
            Console.WriteLine("How many tickets would you like to buy? ");
            string? UserInput = Console.ReadLine();
            if (int.TryParse(UserInput, out int userInput) && userInput > 0)
            {
                return userInput;
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid number greater than 0.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("             BOOK A FLIGHT            ");
                Console.WriteLine("======================================\n");
            }
        }
    }
    private static TicketModel CreateTicket(int bookingID ,int flightId,int seatID,int price)
    {
        // replace the 0 with extra baggage later
        TicketModel ticket = new(bookingID, flightId, seatID ,price, 0);
        return ticket;
    }

    private static PassangerModel CreatePassanger(int current, int total)
    {
        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine($"      PASSENGER DETAILS ({current}/{total})       ");
        Console.WriteLine("======================================\n");

        Console.WriteLine("Please enter first name:");
        string? firstName = Console.ReadLine();

        Console.WriteLine("\nPlease enter last name:");
        string? lastName = Console.ReadLine();

        string? dateOfBirth;
        while (true)
        {
            Console.WriteLine("\nPlease enter date of birth (YYYY-MM-DD): ");
            dateOfBirth = Console.ReadLine();
            if (DateTime.TryParse(dateOfBirth, out _))
                break;
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.\n");
        }

        int passportNumber;
        while (true)
        {
            Console.WriteLine("\nPlease enter passport number: ");
            string? userInput = Console.ReadLine();
            if (int.TryParse(userInput, out passportNumber))
                break;
            Console.WriteLine("Invalid passport number. Please enter a numeric value.\n");
        }

        PassangerModel passanger = new PassangerModel(firstName, lastName, dateOfBirth, passportNumber);
        return passanger;
    }
}
