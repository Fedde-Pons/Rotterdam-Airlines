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
        int accountID = AccountsLogic.CurrentAccount.Id;
        BookingModel booking = new BookingModel(accountID, date, "Confirmed");
        int numberOfTickets = NumberOfTickets();
        List<(PassangerModel passanger, TicketModel ticket, SeatModel seat)> bookingValues = [];

        FlightLogic flightLogic = new();
        var seatData = flightLogic.GetLiveSeatData(flight.Id, flight.AircraftId);
        
        List<SeatModel> availableSeats = seatData.availableSeats;
        int totalSeats = seatData.allSeats.Count;
        int bookedSeats = seatData.bookedSeats;

        Console.Clear();
        Console.WriteLine("\nPress any key to continue to passenger details...");
        Console.ReadKey();
        // Calculate prices once for the entire booking so all tickets share the same price
        double demandFactor = FactoringLogic.CalculateDemandFactor(bookedSeats, totalSeats);
        DateTime departureDate = DateTime.Parse(flight.DepartureTime);
        double timeFactor = FactoringLogic.CalculateTimeUntilDepartureFactor(departureDate);
        double economyPrice = PricingCoreLogic.CalculateFlightPrice(flight.BasePrice, demandFactor, timeFactor, "economy");
        double businessPrice = PricingCoreLogic.CalculateFlightPrice(flight.BasePrice, demandFactor, timeFactor, "business");

        for (int i = 0; i < numberOfTickets; i++)
        {
            PassangerModel passanger = CreatePassanger(i + 1, numberOfTickets);
            
            SeatModel pickedSeat = null;
            double finalPrice = 0;
            int extraBaggageKg = 0;

            // Loop door elke passagier: laat ze een stoel kiezen, check of ze €15 extra moeten betalen voor beenruimte (alleen in economy), en vraag of ze extra bagage willen.
            while (true)
            {
                var seatingResult = SeatingLogic.StartSeatSelection(flight, availableSeats, seatData.allSeats, economyPrice, businessPrice);

                if (seatingResult == null)
                {
                    return;
                }

                pickedSeat = seatingResult.Value.seat;
                finalPrice = seatingResult.Value.price;

                
                if ((pickedSeat.IsExitRow || pickedSeat.IsFirstRow) && pickedSeat.Seatclass.ToLower() == "economy")
                {
                    Console.Clear();
                    Console.WriteLine("======================================");
                    Console.WriteLine("          EXTRA LEGROOM SEAT          ");
                    Console.WriteLine("======================================\n");
                    Console.WriteLine($"Seat {pickedSeat.SeatNumber} has extra legroom!");
                    Console.WriteLine("This seat costs an additional €15.");
                    Console.WriteLine("\nDo you want to keep this seat? (Y/N): ");
                    
                    string? keepSeat = Console.ReadLine()?.Trim().ToUpper();
                    
                    if (keepSeat != "Y")
                    {
                        continue; 
                    }
                    finalPrice += 15; 
                }
                break; 
            }

            
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine($"    EXTRA BAGGAGE ({passanger.FirstName})");
            Console.WriteLine("======================================\n");
            Console.WriteLine("Add an extra 23 kg checked bag for €25? (Y/N):(Y) ");
            
            string? bagInput = Console.ReadLine()?.Trim().ToUpper();
            if (bagInput == "Y")
            {
                extraBaggageKg = 23;
                finalPrice += 25;
            }

            availableSeats.Remove(pickedSeat);
            bookedSeats++;
            
            TicketModel ticket = CreateTicket(booking.Id, flight.Id, pickedSeat.Id, (int)finalPrice, extraBaggageKg);
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
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (input == "Y") break;
            if (input == "N")
            {
                Console.WriteLine("\nBooking cancelled. Returning to main menu...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Invalid input. Please type Y or N and press Enter:");
        }

        // ── SAVE TO DATABASE ───────────────────────────────────────────
        booking.TotalPrice = totalPrice;

        var entries = bookingValues.Select(bv => (bv.passanger, bv.ticket)).ToList();
        int bookingId = BookingLogic.SaveBooking(booking, entries);

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
            Console.WriteLine($"  Price:       €{ticket.Price}");
            if (ticket.ExtraBaggageKg > 0)
            {
                Console.WriteLine($"  Baggage:     + {ticket.ExtraBaggageKg}kg Checked Bag");
            }
            Console.WriteLine();
        }

        Console.WriteLine($"  Total paid: €{totalPrice}");

        Console.WriteLine("\nYou can find this booking back in 'My Bookings' from the main menu.");
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
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

            Console.WriteLine("\nInvalid input. Please enter a valid number greater than 0.");
            Console.WriteLine("Press any key to try again...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("             BOOK A FLIGHT            ");
            Console.WriteLine("======================================\n");
        }
    }

    private static TicketModel CreateTicket(int bookingID, int flightId, int seatID, int price, int extraBaggageKg)
    {
        TicketModel ticket = new(bookingID, flightId, seatID, price, extraBaggageKg);
        return ticket;
    }

    private static PassangerModel CreatePassanger(int current, int total)
    {
        void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine($"      PASSENGER DETAILS ({current}/{total})       ");
            Console.WriteLine("======================================\n");
        }

        PrintHeader();

        string? firstName;
        while (true)
        {
            Console.WriteLine("Please enter first name:");
            firstName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(firstName))
                break;

            PrintHeader();
            Console.WriteLine("You can not enter an empty value.");
            Console.WriteLine("Please try again.\n");
        }

        string? lastName;
        while (true)
        {
            Console.WriteLine("\nPlease enter last name:");
            lastName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(lastName))
                break;

            PrintHeader();
            Console.WriteLine($"First name: {firstName}\n");
            Console.WriteLine("You can not enter an empty value.");
            Console.WriteLine("Please try again.\n");
        }

        string? dateOfBirth;
        while (true)
        {
            Console.WriteLine("\nPlease enter date of birth (YYYY-MM-DD): ");
            dateOfBirth = Console.ReadLine();

            if (DateTime.TryParse(dateOfBirth, out _))
                break;

            PrintHeader();
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}\n");
            Console.WriteLine("Invalid date format. Please use YYYY-MM-DD.\n");
        }

        int passportNumber;
        while (true)
        {
            Console.WriteLine("\nPlease enter passport number: ");
            string? userInput = Console.ReadLine();

            if (int.TryParse(userInput, out passportNumber))
                break;

            PrintHeader();
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}");
            Console.WriteLine($"Date of birth: {dateOfBirth}\n");
            Console.WriteLine("Invalid passport number. Please enter a numeric value.\n");
        }

        PassangerModel passanger = new PassangerModel(firstName, lastName, dateOfBirth, passportNumber);
        return passanger;
    }
}