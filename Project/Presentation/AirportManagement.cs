static class AirportManagement
{
    //show all the airports that are currently in the database
    public static void ViewAllAirports()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== All Airports ===\n");

            List<AirportModel> airports = new AirportAccess().GetAllAirports();

            if (airports.Count == 0)
            {
                Console.WriteLine("No airports found.");
                Console.WriteLine("\nPress any key to return to Airport Management...");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < airports.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {airports[i].Name} ({airports[i].City}, {airports[i].Country})");
            }

            Console.WriteLine("\nEnter the number of an airport to open it, or enter q to go back:");
            string? input = Console.ReadLine();

            if (input == "q")
            {
                return;
            }

            if (int.TryParse(input, out int selection) && selection >= 1 && selection <= airports.Count)
            {
                ShowAirportDetails(airports[selection - 1]);
            }
            else
            {
                Console.WriteLine("Invalid input, please try again.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    //show all the airport details of the selected airport
    private static void ShowAirportDetails(AirportModel airport)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== Airport: {airport.Name} ===\n");
            Console.WriteLine($"Id:      {airport.Id}");
            Console.WriteLine($"Name:    {airport.Name}");
            Console.WriteLine($"Address: {airport.Address}");
            Console.WriteLine($"City:    {airport.City}");
            Console.WriteLine($"Country: {airport.Country}");

            Console.WriteLine("\n1: Edit this airport");
            Console.WriteLine("2: Delete this airport");
            Console.WriteLine("3: Back to the airport list");

            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    EditAirportView(airport);
                    AirportModel? refreshedAirport = new AirportAccess().GetAirportById(airport.Id);
                    if (refreshedAirport == null)
                    {
                        return;
                    }
                    airport = refreshedAirport;
                    break;
                case "2":
                    if (DeleteAirportView(airport))
                    {
                        return;
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid input, please try again.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    //edit airport part
    private static void EditAirportView(AirportModel airport)
    {
        Console.Clear();
        Console.WriteLine($"=== Edit Airport: {airport.Name} ===\n");
        Console.WriteLine("Leave a field empty to keep the current value.\n");

        Console.Write($"Name [{airport.Name}]: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name)) name = airport.Name;

        Console.Write($"Address [{airport.Address}]: ");
        string? address = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(address)) address = airport.Address;

        Console.Write($"City [{airport.City}]: ");
        string? city = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(city)) city = airport.City;

        Console.Write($"Country [{airport.Country}]: ");
        string? country = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(country)) country = airport.Country;

        bool success = AirportLogic.EditAirport(airport.Id, name, address, city, country);
        if (success)
        {
            Console.WriteLine("\nAirport successfully updated.");
        }
        else
        {
            Console.WriteLine("\nCould not update the airport. Please check that the city and country are valid and no fields are empty.");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    //return true if airport is deleted, false if not
    private static bool DeleteAirportView(AirportModel airport)
    {
        Console.Clear();
        Console.WriteLine($"=== Delete Airport: {airport.Name} ===\n");

        List<FlightModel> futureFlights = AirportLogic.GetFutureFlightsForAirport(airport.Id);

        if (futureFlights.Count > 0)
        {
            Console.WriteLine("This airport cannot be deleted because the following future flights still use it:\n");
            Console.WriteLine($"{"Flight",-10} {"Departure",-22} {"From",-28} {"To",-28} {"Status"}");
            Console.WriteLine(new string('-', 101));
            foreach (FlightModel flight in futureFlights)
            {
                Console.WriteLine($"{flight.FlightNumber,-10} {flight.DepartureTime,-22} {flight.DepartureAirportName,-28} {flight.DestinationAirportName,-28} {flight.Status}");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return false;
        }

        Console.WriteLine("Are you sure you want to delete this airport? (y/n):");
        string? confirm = Console.ReadLine();
        if (confirm?.ToLower() != "y")
        {
            Console.WriteLine("Deletion cancelled.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        bool success = AirportLogic.DeleteAirport(airport.Id);
        if (success)
        {
            Console.Clear();
            Console.WriteLine("\nAirport successfully deleted.");
        }
        else
        {
            Console.WriteLine("\nCould not delete the airport.");
        }
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        return success;
    }
}
