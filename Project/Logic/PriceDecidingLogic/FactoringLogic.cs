public static class FactoringLogic
{
    // Returns 0 (empty flight) → 1 (fully booked)
    public static double CalculateDemandFactor(int bookedSeats, int totalSeats)
    {
        if (totalSeats <= 0) return 0;
        double ratio = (double)bookedSeats / totalSeats;
        return Math.Clamp(ratio, 0.0, 1.0); // Math.Clamp zorgt ervoor dat een waarde binnen een bepaald bereik blijft.
        // stel ratio was 1.3, dan returned het 1.0
    }

    // Returns 0 (far in the future) → 1 (departure is today/overdue)
    public static double CalculateTimeUntilDepartureFactor(DateTime departureDate)
    {
        double daysUntil = (departureDate - DateTime.Today).TotalDays;
        if (daysUntil <= 0) return 1.0;
        return 1.0 / (1.0 + daysUntil);
    }
}
