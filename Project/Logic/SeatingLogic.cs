public static class SeatingLogic
{
    public static (SeatModel seat, double price)? StartSeatSelection(
        FlightModel selectedFlight, 
        List<SeatModel> availableSeats, 
        int totalSeats, 
        int bookedSeats)
    {

        double demandFactor = FactoringLogic.CalculateDemandFactor(bookedSeats, totalSeats);
        DateTime departureDate = DateTime.Parse(selectedFlight.DepartureTime);
        double timeFactor = FactoringLogic.CalculateTimeUntilDepartureFactor(departureDate);

        double economyPrice = PricingCoreLogic.CalculateFlightPrice(selectedFlight.BasePrice, demandFactor, timeFactor, "economy");
        double businessPrice = PricingCoreLogic.CalculateFlightPrice(selectedFlight.BasePrice, demandFactor, timeFactor, "business");

        


    }
}


// THIS IS STILL IN PROGRESS