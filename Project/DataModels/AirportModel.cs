using System.Security.Authentication;

public class AirportModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public AirportModel(string name, string address, string city, string country)
    {
        Name = name;
        Address = address;
        City = city;
        Country = country;
    }
}