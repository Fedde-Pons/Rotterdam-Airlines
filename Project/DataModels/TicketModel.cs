﻿public class TicketModel
{
    public long Id { get; set; }
    public long BookingId { get; set; }
    public long FlightId { get; set; }
    public long SeatId { get; set; }
    public long PassengerId { get; set; }
    public double Price { get; set; }
    public int ExtraBaggageKg { get; set; }

    public TicketModel(long bookingId, long flightId, long seatId, long passengerId, double price, int extraBaggageKg)
    {
        BookingId = bookingId;
        FlightId = flightId;
        SeatId = seatId;
        PassengerId = passengerId;
        Price = price;
        ExtraBaggageKg = extraBaggageKg;
    }
}
