using System.Runtime.CompilerServices;

public static class BookingForums
{
    public static void Start(FlightModel flight, string date)
    {
        int accountID = AccountsLogic.CurrentAccount.Id;
        BookingModel booking = new BookingModel(accountID, date, "ongoing");
        int numberOfTickets = NumberOfTickets();
        for(int i = 0; i < numberOfTickets; i++)
        {
            
        }
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
    private static TicketModel CreateTicket(int bookingID ,int flightId,int seatID, int passangerID,int price)
    {
        // replace the 0 with extra baggage later
        TicketModel ticket = new(bookingID, flightId, seatID,passangerID,price, 0);
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
