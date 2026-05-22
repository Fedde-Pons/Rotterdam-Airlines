using System.Data.Common;
using System.Runtime.InteropServices;

public static class AirportLogic
{
    public static (bool, string) AddAirport(string name, string address, string city, string country )
    {
        try
        {
            (AirportModel? airport, bool isSucces, String message) airport = ConvertToAirportModel(name, address, city, country);
            if (airport.airport == null || !airport.isSucces)
            {
                return (false, airport.message);   
            }
            AirportAccess db = new();
            db.WriteAirport(airport.airport);
            return (true, "airport successfully added");
        }
        catch
        {
            return (false, "undefined behavior happend");
        }
    }
    public static (AirportModel?, bool, string) ConvertToAirportModel(string name, string address, string city, string country )
    {
        try
        {
            AirportModel airport = new(name, address, city, country);
            return (airport, true, "sucess");
        }
        catch
        {
            return (null, false, "couldnt convert to airport model");
        }
    }
}