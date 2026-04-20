public static class SeatingLogic
{
    public static (SeatModel seat, double price)? StartSeatSelection(
        FlightModel selectedFlight, 
        List<SeatModel> availableSeats, 
        int totalSeats, 
        int bookedSeats)
    {
        Console.Clear();
        Console.WriteLine($"=== SEAT SELECTION FOR FLIGHT {selectedFlight.FlightNumber} ===");

        double demandFactor = FactoringLogic.CalculateDemandFactor(bookedSeats, totalSeats);
        DateTime departureDate = DateTime.Parse(selectedFlight.DepartureTime);
        double timeFactor = FactoringLogic.CalculateTimeUntilDepartureFactor(departureDate);

        double economyPrice = PricingCoreLogic.CalculateFlightPrice(selectedFlight.BasePrice, demandFactor, timeFactor, "economy");
        double businessPrice = PricingCoreLogic.CalculateFlightPrice(selectedFlight.BasePrice, demandFactor, timeFactor, "business");

        Console.WriteLine("\n--- Business Class ---");
        foreach (SeatModel seat in availableSeats)
        {
            if (seat.Seatclass.Equals("Business", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"- Seat {seat.SeatNumber} | €{businessPrice:F2} | Window: {seat.IsWindows}");
        }

        Console.WriteLine("\n--- Economy Class ---");
        foreach (SeatModel seat in availableSeats)
        {
            if (seat.Seatclass.Equals("Economy", StringComparison.OrdinalIgnoreCase))
                Console.WriteLine($"- Seat {seat.SeatNumber} | €{economyPrice:F2} | Window: {seat.IsWindows}");
        }

        SeatModel chosenSeatModel = null;
        while (true)
        {
            Console.Write("\nEnter the Seat Number you want to book (or type X to cancel): ");
            string userInput = Console.ReadLine()!.ToUpper();

            if (userInput == "X") 
            {
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

            if (chosenSeatModel != null) break;
            else Console.WriteLine("Invalid seat number. Please choose from the list above.");
        }


        double finalPrice = 0;
        if (chosenSeatModel.Seatclass.Equals("Economy", StringComparison.OrdinalIgnoreCase)) finalPrice = economyPrice;
        if (chosenSeatModel.Seatclass.Equals("Business", StringComparison.OrdinalIgnoreCase)) finalPrice = businessPrice;


        return (chosenSeatModel, finalPrice);
    }
}