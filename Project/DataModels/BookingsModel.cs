public class BookingsModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string Date { get; set; }
    public double TotalPrice { get; set; }
    public enum Status { Done = 0, onGoing = 1, Canceled = 2}

    public BookingsModel(long accountId, string date, double totalPrice, int Status )
    {
        FlightId = flightId;
        SeatId = seatId;
        PassengerId = passengerId;
        Price = price;
        ExtraBaggageKg = extraBaggageKg;
    }
}


