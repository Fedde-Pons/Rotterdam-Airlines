public static class SeatingLogic
{
    public static (SeatModel seat, double price)? StartSeatSelection(
        FlightModel selectedFlight, 
        List<SeatModel> availableSeats, 
        double economyPrice,
        double businessPrice)
    {
        Console.Clear();
        
        SeatMap.ShowSeatMap(selectedFlight, availableSeats);
        
        Console.WriteLine($"\n=== SEAT SELECTION FOR FLIGHT {selectedFlight.FlightNumber} ===");

        Console.WriteLine($"* Business Class: €{businessPrice:F2}");
        Console.WriteLine($"* Economy Class:  €{economyPrice:F2}");
        Console.WriteLine("--------------------------------------------------");

        SeatModel chosenSeatModel = null;
        while (true)
        {
            Console.Write("\nEnter the Seat Number you want to book from (or type X to cancel): ");
            string userInput = Console.ReadLine()?.Trim().ToUpper();

            if (userInput == "X") 
            {
                Console.Clear();
                return null; 
            }

            foreach (SeatModel seat in availableSeats)
            {
                if (seat.SeatNumber.ToUpper() == userInput)
                {
                    chosenSeatModel = seat;
                    break;
                }
            }

            if (chosenSeatModel != null)
            {
                break;
            }
            else 
            {
                Console.WriteLine("Invalid or taken seat number. Please choose an available seat from the map.");
            }
        }
        
        double finalPrice = 0;
        if (chosenSeatModel.Seatclass.Equals("Business", StringComparison.OrdinalIgnoreCase))
        {
            finalPrice = businessPrice;
        }
        else
        {
            finalPrice = economyPrice;
        }
        
        return (chosenSeatModel, finalPrice);
    }
}