public static class PassangerLogic
{
    // naam vaildatie kijkt ook of naam niet alleen -- of '' bevat, moet dus wel een letter.
    public static bool IsValidName(string name)
    {
    if (string.IsNullOrWhiteSpace(name)) return false;
    if (name.Length < 2) return false;
    if (name.All(c => !char.IsLetter(c))) return false;
    return name.All(c => char.IsLetter(c) || c == '-' || c == '\'');
    }
    // leeftijd validatie, kan dus niet in toekomst meer worden geboekt en minimaal 2 weken van current date.
    // datetime .today is een property dat je eigenlijk vandaag geeft als property vanzelfsprekend.
    // .AddDays is een date time methode dat eigenlijk dagen toevoegt of afteld als je in de min gaat, dus - 14 voor 2 weken terug voor validatie.
    public static bool IsValidDateOfBirth(string dateOfBirth)
    {
    if (!DateTime.TryParse(dateOfBirth, out DateTime dob)) return false;
    return dob <= DateTime.Today.AddDays(-14);
    }

    public static PassangerModel CreatePassanger(string? firstName, string? lastName, string? dateOfBirth, int passportNumber)
    {
        return new PassangerModel(firstName, lastName, dateOfBirth, passportNumber);
    }

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
