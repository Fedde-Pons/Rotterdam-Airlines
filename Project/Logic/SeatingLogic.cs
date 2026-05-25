public static class SeatingLogic
{
    // Returns the cabin layout for a given aircraft.
    public static AircraftLayoutModel GetLayout(int aircraftId)
    {
        return aircraftId switch
        {
            1 => new AircraftLayoutModel("B O E I N G   7 3 7",
                    new[] { "F", "E", "D", "C", "B", "A" }, 33, 3, 11, 26),
            2 => new AircraftLayoutModel("A I R B U S   A 3 3 0",
                    new[] { "H", "G", "F", "E", "D", "C", "B", "A" }, 43, 10, 18, 33),
            3 => new AircraftLayoutModel("B O E I N G   7 8 7",
                    new[] { "F", "E", "D", "C", "B", "A" }, 38, 6, 10, 20),
            _ => new AircraftLayoutModel("U N K N O W N",
                    new[] { "A" }, 1, 0, 0, 0)
        };
    }

    public static bool IsBusinessRow(int row, AircraftLayoutModel layout)
    {
        return row <= layout.BusinessRows;
    }

    public static double GetSeatPrice(int row, AircraftLayoutModel layout, double economyPrice, double businessPrice)
    {
        return IsBusinessRow(row, layout) ? businessPrice : economyPrice;
    }

    public static (int row, int letterIndex)? GetFirstAvailableSeat(List<SeatModel> availableSeats, AircraftLayoutModel layout)
    {
        for (int li = 0; li < layout.Letters.Length; li++)
            for (int r = 1; r <= layout.TotalRows; r++)
                if (availableSeats.Any(s => s.SeatNumber == $"{r}{layout.Letters[li]}"))
                    return (r, li);

        return null;
    }

    public static (SeatModel seat, double price)? StartSeatSelection(
        FlightModel selectedFlight,
        List<SeatModel> availableSeats,
        List<SeatModel> allSeats,
        double economyPrice,
        double businessPrice)
    {
        string? selectedCode = SeatMap.NavigateSeatMap(selectedFlight, availableSeats, allSeats, economyPrice, businessPrice);

        if (selectedCode == null)
        {
            Console.Clear();
            return null;
        }

        SeatModel chosenSeatModel = availableSeats.First(s => s.SeatNumber == selectedCode);

        double finalPrice = 0;
        if (chosenSeatModel.Seatclass.Equals("Business", StringComparison.OrdinalIgnoreCase))
        {
            finalPrice = businessPrice;
        }
        else
        {
            finalPrice = economyPrice;
        }

        return (chosenSeatModel, finalPrice);
    }
}
