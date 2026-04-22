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
            PassangerModel passanger = CreatePassanger();
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
        Console.WriteLine($"\nYou have successfully booked {numberOfTickets} ticket(s) for Flight {flight.FlightNumber}!");
        
        foreach (var bookedItem in bookingValues)
        {
            Console.WriteLine($"- Passenger: {bookedItem.passanger.FirstName} {bookedItem.passanger.LastName} | Ticket Price: €{bookedItem.ticket.Price}");
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
        while (true)
        {
            Console.WriteLine("how many tickets do you want to buy");
            string? UserInput = Console.ReadLine();
            if (int.TryParse(UserInput, out int userInput))
            {
                return userInput; 
            }
            else
            {
                Console.Clear();
                Console.WriteLine("please put in a valid number, press any key to continue");
                Console.ReadKey();
            }
        }

    }
    private static TicketModel CreateTicket(int bookingID ,int flightId,int seatID,int price)
    {
        // replace the 0 with extra baggage later
        TicketModel ticket = new(bookingID, flightId, seatID ,price, 0);
        return ticket;
    }

    private static PassangerModel CreatePassanger()
    {
        Console.Clear();
        string? firstName;
        string? lastName;
        string? dateOfBirth;
        int passportNumber;
        while(true)
        {
            Console.WriteLine("please enter your first name");   
            firstName = Console.ReadLine();
            break;
        }
        while(true)
        {
            Console.WriteLine("please enter your last name");   
            lastName = Console.ReadLine();
            break;
        }
        while(true)
        {
            Console.WriteLine("please enter your date of birth YYYY-MM-DD");   
            dateOfBirth = Console.ReadLine();
            break;
        }
        while(true)
        {
            Console.WriteLine("please put in your passport number");
            string? UserInput = Console.ReadLine();
            if (int.TryParse(UserInput, out int userInput))
            {
                passportNumber = userInput;
                break;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("please put in a valid passport-number, press any key to continue");
                Console.ReadKey();
            }
        }
        PassangerModel passanger = new PassangerModel(firstName, lastName, dateOfBirth, passportNumber);
        Console.WriteLine("passanger created");
        return passanger;
    }
}
