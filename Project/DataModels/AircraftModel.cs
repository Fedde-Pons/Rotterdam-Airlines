public class AircraftModel
{
    public int Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int TotalSeats { get; set; }
    public int BusinessSeats { get; set; }
    public int EconomySeats { get; set; }


    public AircraftModel() { }

    public AircraftModel(string manufacturer, string model, int businessSeats, int economySeats)
    {
        Manufacturer = manufacturer;
        Model = model;
        BusinessSeats = businessSeats;
        EconomySeats = economySeats;
        TotalSeats = economySeats + businessSeats;
    }
}