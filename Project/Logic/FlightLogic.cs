using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FlightLogic
{
    private FlightAccess flightAccess = new FlightAccess();

    public List<FlightModel> GetAllFlights()
    {
        List<FlightModel> flightList = flightAccess.GetAllFlights();
        return flightList;
    }

    public List<FlightModel> GetAllFutureFlights()
    {
        List<FlightModel> flights = flightAccess.GetAllFlights();

        return flights
            .Where(f => DateTime.TryParse(f.DepartureTime, out DateTime departureTime)
                        && departureTime > DateTime.Now)
            .OrderBy(f => DateTime.Parse(f.DepartureTime))
            .ToList();
    }

    public List<FlightModel> GetAllAvailableFlightsSorted()
    {
        List<FlightModel> flights = flightAccess.GetAllAvailableFlights();

        return flights
            .Where(f => DateTime.TryParse(f.DepartureTime, out DateTime departureTime)
                        && departureTime > DateTime.Now)
            .OrderBy(f => DateTime.Parse(f.DepartureTime))
            .ToList();
    }

    public FlightModel? GetFlightById(int id)
    {
        FlightModel? flight = flightAccess.RetrieveFlight(id);

        if (flight == null)
        {
            return null;
        }

        if (!DateTime.TryParse(flight.DepartureTime, out DateTime departureTime))
        {
            return null;
        }

        if (departureTime <= DateTime.Now)
        {
            return null;
        }

        return flight;
    }

    public SeatModel? GetSeatById(int id)
    {
        return flightAccess.RetrieveSeat(id);
    }

    public bool FlightNumberExists(string flightNumber)
    {
        return flightAccess.RetrieveFlight(flightNumber) != null;
    }

    public List<AircraftModel> GetAllAircrafts()
    {
        return flightAccess.GetAllAircrafts();
    }

    public List<AirportModel> GetAllAirports()
    {
        return new AirportAccess().GetAllAirports();
    }

    public (bool Success, string ErrorMessage) AddFlight(
        string? flightNumber,
        string? aircraftIdInput,
        string? departureAirportIdInput,
        string? destinationAirportIdInput,
        string? departureTimeInput,
        string? arrivalTimeInput,
        string? basePriceInput)
    {
        if (string.IsNullOrWhiteSpace(flightNumber) ||
            string.IsNullOrWhiteSpace(aircraftIdInput) ||
            string.IsNullOrWhiteSpace(departureAirportIdInput) ||
            string.IsNullOrWhiteSpace(destinationAirportIdInput) ||
            string.IsNullOrWhiteSpace(departureTimeInput) ||
            string.IsNullOrWhiteSpace(arrivalTimeInput) ||
            string.IsNullOrWhiteSpace(basePriceInput))
        {
            return (false, "All required fields must be filled.");
        }

        if (!int.TryParse(aircraftIdInput, out int aircraftId))
            return (false, "Invalid aircraft id.");

        if (!int.TryParse(departureAirportIdInput, out int departureAirportId))
            return (false, "Invalid departure airport id.");

        if (!int.TryParse(destinationAirportIdInput, out int destinationAirportId))
            return (false, "Invalid destination airport id.");

        if (departureAirportId == destinationAirportId)
            return (false, "Departure airport and destination airport cannot be the same.");

        const int rotterdamZuidId = 7;
        if (departureAirportId != rotterdamZuidId && destinationAirportId != rotterdamZuidId)
            return (false, "One of the airports must be Rotterdam Zuid.");

        if (!DateTime.TryParseExact(departureTimeInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime departureTime))
            return (false, "Invalid departure time. Use format yyyy-MM-dd HH:mm.");

        if (!DateTime.TryParseExact(arrivalTimeInput, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime arrivalTime))
            return (false, "Invalid arrival time. Use format yyyy-MM-dd HH:mm.");

        if (departureTime <= DateTime.Now)
            return (false, "Departure time must be in the future.");

        if (arrivalTime <= departureTime)
            return (false, "Arrival time must be after departure time.");

        if (!double.TryParse(basePriceInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double basePrice) || basePrice <= 0)
            return (false, "Invalid base price.");

        FlightModel flight = new FlightModel
        {
            FlightNumber = flightNumber,
            AircraftId = aircraftId,
            DepartureAirportId = departureAirportId,
            DestinationAirportId = destinationAirportId,
            DepartureTime = departureTime.ToString("yyyy-MM-dd HH:mm"),
            ArrivalTime = arrivalTime.ToString("yyyy-MM-dd HH:mm"),
            BasePrice = basePrice,
            Status = "Scheduled"
        };

        bool saved = flightAccess.AddFlight(flight);

        if (!saved)
            return (false, "Flight could not be saved.");

        return (true, "Flight created successfully.");
    }

    public static string CreateFlightsSummary(List<FlightModel>? flights)
    {
        if (flights == null || flights.Count == 0)
            return "";

        StringBuilder sb = new StringBuilder();

        foreach (FlightModel f in flights.OrderBy(f => f.FlightNumber))
        {
            sb.Append($"{f.FlightNumber}|{f.DepartureTime}|{f.ArrivalTime}|{f.BasePrice}|{f.Status}|{f.DepartureCity}|{f.DestinationCity};");
        }

        return sb.ToString();
    }
}