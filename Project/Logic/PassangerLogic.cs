public static class PassangerLogic
{
    public static PassangerModel? GetById(int id)
    {
        PassangerAccess db = new();
        return db.GetById(id);
    }
    public static List<PassangerListEntry> GetPassengerListForFlight(int flightId)
    {
        PassangerAccess db = new();
        return db.GetPassengerListForFlight(flightId);
    }
}
