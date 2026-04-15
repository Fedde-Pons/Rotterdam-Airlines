public class AircraftModel
{
    public int Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int TotalSeats { get; set; }
    public int BuisnessSeats { get; set; }
    public int EconomySeats { get; set; }


    public AircraftModel(string manufacturer, string model, int buisnessSeats, int economySeats)
    {
        Manufacturer = manufacturer;
        Model = model;
        BuisnessSeats = buisnessSeats;
        EconomySeats = economySeats;
        TotalSeats = economySeats + buisnessSeats;
    }
}