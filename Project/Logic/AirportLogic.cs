

using System.Text.RegularExpressions;

public static class AirportLogic
{
    public static (bool, string) AddAirport(string name, string address, string city, string country)
    {
        try
        {
            if(!ValidateLocation(city) || !ValidateLocation(country))
            {
                return (false, "not a real location or city");
            }
            
            if(name == "" || address == ""|| city == "" || country == "")
            {
                return (false, "please dont leave a value empty");
            }

            (AirportModel? airport, bool isSucces, String message) airport = ConvertToAirportModel(name, address, city, country);
            if (airport.airport == null || !airport.isSucces)
            {
                return (false, airport.message);
            }

            AirportAccess db = new();
            if (AirportExists(airport.airport, db.GetAllAirports()))
            {
                return (false, "airport already exists");
            }
            db.WriteAirport(airport.airport);
            return (true, "airport successfully added");
        }
        catch
        {
            return (false, "undefined behavior happend");
        }
    }
    public static (AirportModel?, bool, string) ConvertToAirportModel(string name, string address, string city, string country)
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
    private static bool AirportExists(AirportModel airport, List<AirportModel> allAirports)
    {
        foreach (AirportModel existingAirport in allAirports)
        {
            if (airport.Name == existingAirport.Name)
            {
                return true;
            }
        }
        return false;
    }
    private static bool ValidateLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return false;

        if (!Regex.IsMatch(location, @"^[a-zA-Z\s-]+$"))
            return false;

        return true;
    }


}