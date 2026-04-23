public static class PricingCoreLogic
{
    public static double CalculateFlightPrice(
        double baseFare,
        double demandFactor,
        double TimeUntilDepartureFactor,
        string seatType,
        double discount = 0 // excluderend membership
        )
    {
        // baseFare = distance × 0.1
        
        // price = (baseFare × distance) × DemandFactor + SeatTypeExtraCost - Discounts
        double price = baseFare * ((demandFactor+TimeUntilDepartureFactor/2)+1);
        price += GetSeatTypeExtraCost(seatType);
        price -= discount;
        
            
        return Math.Round(Math.Max(price, 0), 2); // zeker weten dat het minimaal 0 returned als het negatief zou zijn
    }
    
    private static double GetSeatTypeExtraCost(string seatType) => seatType switch
    {
        "business" => 200,
        "economy" => 0,
        _ => 0
    };
}

// ergens in andermans code moet deze class worden geimplementeerd, en daarna FlightModel updaten,
// en daarna functie daarin gebruiken om basePrice te updaten in db
// make sure om ook in de andere logica, FactoringLogic.cs te gebruiken 