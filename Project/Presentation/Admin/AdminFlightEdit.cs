static class AdminFlightEdit
{
    private static FlightLogic _flightLogic = new FlightLogic();

    public static void ShowflightWithEditValues(FlightModel flight)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Flight number:  {flight.FlightNumber}");
            Console.WriteLine($"From:           {flight.DepartureAirportName} ({flight.DepartureCity}, {flight.DepartureCountry})");
            Console.WriteLine($"To:             {flight.DestinationAirportName} ({flight.DestinationCity}, {flight.DestinationCountry})");
            Console.WriteLine($"Departure time: {flight.DepartureTime}");
            Console.WriteLine($"Arrival time:   {flight.ArrivalTime}");
            Console.WriteLine($"Base price:     €{flight.BasePrice}");
            Console.WriteLine($"Status:         {flight.Status}");
            Console.WriteLine($"Aircraft:       {flight.AircraftManufacturer} {flight.AircraftModel}");

            var (businessBooked, economyBooked) = TicketLogic.GetSeatOccupancy(flight.Id);
            Console.WriteLine($"Occupancy:      Business: {businessBooked} booked | Economy: {economyBooked} booked");

            Console.WriteLine();

            string? input;

            if (flight.Status == "Cancelled")
            {
                Console.WriteLine("This flight has been cancelled.");
                Console.WriteLine("\n1: Return to Flight Management");
                Console.WriteLine("\nPlease enter the number of the option you would like to choose:");
                input = Console.ReadLine();

                if (input == "1")
                {
                    return;
                }

                Console.Clear();
                Console.WriteLine("Invalid option. Please select an option from the list.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            Console.WriteLine("1: Edit departure and arrival time");
            Console.WriteLine("2: Cancel flight (and connected bookings)");
            Console.WriteLine("3: Adjust base price");
            Console.WriteLine("4: Print Passenger List");
            Console.WriteLine("5: Return to Flight Management");
            Console.WriteLine("\nPlease enter the number of the option you would like to choose:");
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Clear();
                Console.WriteLine("Invalid option. Please select an option from the list.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            switch (input)
            {
                case "1":
                    EditArrivalTime(flight);
                    break;
                case "2":
                    Cancelflight(flight);
                    return;
                case "3":
                    EditPrice(flight);
                    break;
                case "4":
                    PassengerList.Show(flight);
                    break;
                case "5":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid option. Please select an option from the list.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void EditArrivalTime(FlightModel flight)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===EDIT FLIGHT TIMES===");
            Console.WriteLine("(Keep empty to keep the existing value)\n");

            Console.WriteLine($"Current departure time: {flight.DepartureTime}");
            Console.Write("Enter new departure time (yyyy-MM-dd HH:mm): ");
            string? departureInput = Console.ReadLine();

            Console.WriteLine($"\nCurrent arrival time: {flight.ArrivalTime}");
            Console.Write("Enter new arrival time (yyyy-MM-dd HH:mm): ");
            string? arrivalInput = Console.ReadLine();

            bool departureEmpty = string.IsNullOrWhiteSpace(departureInput);
            bool arrivalEmpty = string.IsNullOrWhiteSpace(arrivalInput);

            if (departureEmpty && arrivalEmpty)
            {
                Console.Clear();
                Console.WriteLine("No changes made");
                Console.WriteLine("\nPress any key to return to flight details...");
                Console.ReadKey();
                return;
            }

            string currentDeparture = flight.DepartureTime![..16];
            string currentArrival = flight.ArrivalTime![..16];

            string departure = departureEmpty ? currentDeparture : departureInput!;
            string arival = arrivalEmpty ? currentArrival : arrivalInput!;

            Console.Clear();
            Console.WriteLine("Processing update...");

            (bool isSuccesfull, string ErrorMessage) edittedFlight = _flightLogic.EditFlightTime(flight, departure, arival);
            if (!edittedFlight.isSuccesfull)
            {
                Console.Clear();
                Console.WriteLine("Something went wrong:");
                Console.WriteLine(edittedFlight.ErrorMessage);
                Console.WriteLine("\nPress any key to try again...");
                Console.ReadKey();
                continue;
            }

            Console.Clear();
            Console.WriteLine("Flight times have been successfully updated");
            Console.WriteLine("\nPress any key to return to flight details...");
            Console.ReadKey();
            return;
        }
    }

    private static void EditPrice(FlightModel flight)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===ADJUST BASE PRICE===");
            Console.WriteLine("(Keep empty to keep the existing value)\n");

            Console.WriteLine($"Current base price: €{flight.BasePrice}");
            Console.Write("Enter new base price (numbers only): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.Clear();
                Console.WriteLine("No changes made");
                Console.WriteLine("\nPress any key to return to flight details...");
                Console.ReadKey();
                return;
            }

            if (int.TryParse(input, out int price))
            {
                Console.Clear();
                Console.WriteLine("Processing update...");
                _flightLogic.EditPrice(flight, price);

                Console.Clear();
                Console.WriteLine("Base price has been successfully updated");
                Console.WriteLine($"New base price: €{price}");
                Console.WriteLine("\nPress any key to return to flight details...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Invalid input. Please enter a valid number.");
            Console.WriteLine("\nPress any key to try again...");
            Console.ReadKey();
        }
    }

    private static void Cancelflight(FlightModel flight)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===CANCEL FLIGHT===");
            Console.WriteLine();

            Console.WriteLine($"Flight number: {flight.FlightNumber}");
            Console.WriteLine($"Departure: {flight.DepartureTime}");
            Console.WriteLine();
            Console.WriteLine("This action will cancel the flight and all associated bookings.");
            Console.Write("Are you sure you want to proceed? (Y/N): ");
            string? confirm = Console.ReadLine();

            if (confirm?.ToLower() == "y")
            {
                Console.Clear();
                Console.WriteLine("Cancelling flight...");
                _flightLogic.CancelFlight(flight);

                Console.Clear();
                Console.WriteLine("Flight has been successfully cancelled");
                Console.WriteLine("All associated bookings have also been cancelled.");
                Console.WriteLine("\nPress any key to return to flight details...");
                Console.ReadKey();
                return;
            }
            else if (confirm?.ToLower() == "n")
            {
                Console.Clear();
                Console.WriteLine("Flight cancellation aborted.");
                Console.WriteLine("\nPress any key to return to flight details...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
            Console.WriteLine("\nPress any key to try again...");
            Console.ReadKey();
        }
    }
}
