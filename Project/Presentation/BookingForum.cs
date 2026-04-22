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
        List<(PassangerModel passanger, TicketModel ticket)> bookingValues = [];

        FlightAccess dbAccess = new FlightAccess();
        var seatData = dbAccess.GetLiveSeatData(flight.Id, flight.AircraftId);
        
        List<SeatModel> availableSeats = seatData.availableSeats;
        int totalSeats = seatData.totalSeats;
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
            bookingValues.Add((passanger, ticket));
        }

        Console.Clear();
        Console.WriteLine("======================================");
        Console.WriteLine("          BOOKING SUCCESSFUL!         ");
        Console.WriteLine("======================================");
        Console.WriteLine($"\nFlight:   {flight.FlightNumber}");
        Console.WriteLine($"Tickets:  {numberOfTickets}");
        Console.WriteLine();
        Console.WriteLine("  #  Passenger                   Price");
        Console.WriteLine("  -  ---------                   -----");
        for (int i = 0; i < bookingValues.Count; i++)
        {
            var item = bookingValues[i];
            string name = $"{item.passanger.FirstName} {item.passanger.LastName}";
            Console.WriteLine($"  {i + 1}  {name,-28} €{item.ticket.Price}");
        }

        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();

        
        // code for storing it in the database here
        // would recommend using a tuple that combines the booking and the bookingValues list
        // also keep in mind that my code doesnt assign a passanger id to the ticket (sinde that database decides the id)

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
