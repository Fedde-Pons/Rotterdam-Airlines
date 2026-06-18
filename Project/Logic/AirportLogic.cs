

using System.Text.RegularExpressions;

public static class AirportLogic
{
    public static AirportModel? GetAirportById(long id)
    {
        return new AirportAccess().GetAirportById(id);
    }

    public static List<AirportModel> GetAllAirports()
    {
        return new AirportAccess().GetAllAirports();
    }

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
                return (false, "Please make sure to fill in all fields");
            }

            (AirportModel? airport, bool isSucces, String message) airport = ConvertToAirportModel(name, address, city, country);
            if (airport.airport == null || !airport.isSucces)
            {
                return (false, airport.message);
            }

            AirportAccess db = new();
            if (AirportExists(airport.airport, db.GetAllAirports()))
            {
                return (false, "This airport already exists");
            }
            db.WriteAirport(airport.airport);
            return (true, "Airport created successfully");
        }
        catch
        {
            return (false, "undefined behavior happend");
        }
    }
    public static bool EditAirport(long id, string name, string address, string city, string country)
    {
        try
        {
            if (!ValidateLocation(city) || !ValidateLocation(country))
            {
                return false;
            }

            if (name == "" || address == "" || city == "" || country == "")
            {
                return false;
            }

            AirportAccess db = new();
            AirportModel? existing = db.GetAirportById(id);
            if (existing == null)
            {
                return false;
            }

            AirportModel updated = new(id, name, address, city, country);
            db.UpdateAirport(updated);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static List<FlightModel> GetFutureFlightsForAirport(long airportId)
    {
        List<FlightModel> allFlights = new FlightAccess().GetAllFlights();
        DateTime now = DateTime.Now;

        List<FlightModel> futureFlights = new();
        foreach (FlightModel flight in allFlights)
        {
            bool usesThisAirport =
                flight.DepartureAirportId == airportId ||
                flight.DestinationAirportId == airportId;

            if (!usesThisAirport)
            {
                continue;
            }

            if (DateTime.TryParse(flight.DepartureTime, out DateTime departure) && departure > now)
            {
                futureFlights.Add(flight);
            }
        }
        return futureFlights;
    }

    public static (bool, string) DeleteAirport(long id)
    {
        try
        {
            AirportAccess db = new();
            AirportModel? existing = db.GetAirportById(id);
            if (existing == null)
            {
                return (false, "Airport not found in database");
            }

            // an airport can only be removed when no future flights use it.
            List<FlightModel> futureFlights = GetFutureFlightsForAirport(id);
            if (futureFlights.Count > 0)
            {
                return (false, $"Cannot delete: {futureFlights.Count} future flight(s) still use this airport");
            }

            db.DeleteAirport(id);
            return (true, "Airport successfully deleted");
        }
        catch (Exception ex)
        {
            return (false, $"Database error: {ex.Message}");
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