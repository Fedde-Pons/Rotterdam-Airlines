public class TicketModel
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int FlightId { get; set; }
    public int SeatId { get; set; }
    public int PassengerId { get; set; }
    public float Price { get; set; }
    public int ExtraBaggageKg { get; set; }

    public TicketModel(int bookingId, int flightId, int seatId, int passengerId, float price, int extraBaggageKg)
    {
        BookingId = bookingId;
        FlightId = flightId;
        SeatId = seatId;
        PassengerId = passengerId;
        Price = price;
        ExtraBaggageKg = extraBaggageKg;
    }
    public TicketModel(int bookingId, int flightId, int seatId, float price, int extraBaggageKg)
    {
        BookingId = bookingId;
        FlightId = flightId;
        SeatId = seatId;
        Price = price;
        ExtraBaggageKg = extraBaggageKg;
    }
}
