// this is a single passenger of all the passengers in a flight
public class PassangerListEntry
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PassportNumber { get; set; }
    public string SeatNumber { get; set; }
    public string SeatClass { get; set; }
    public int ExtraBaggageKg { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
