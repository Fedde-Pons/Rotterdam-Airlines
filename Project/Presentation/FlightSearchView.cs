using System;
using System.Collections.Generic;
using Project.Logic;
using Project.DataModels;

namespace Project.Presentation
{
    public static class FlightSearch
    {
        public static void StartSearch()
        {
            FlightList.ShowAllAvailableFlightsShortList();

            Console.WriteLine("\nSEARCH FOR A FLIGHT");

            FlightLogic flightLogic = new FlightLogic();
            List<FlightModel> flights = flightLogic.GetAllFlights();

            Console.Write("Enter your departure city: ");
            string searchDeparture = Console.ReadLine();

            Console.Write("Enter your destination city: ");
            string searchDestination = Console.ReadLine();

            List<FlightModel> routeMatches = new List<FlightModel>();

            foreach (FlightModel flight in flights)
            {
                if (flight.DepartureCity.Equals(searchDeparture, StringComparison.OrdinalIgnoreCase) &&
                    flight.DestinationCity.Equals(searchDestination, StringComparison.OrdinalIgnoreCase))
                {
                    routeMatches.Add(flight);
                }
            }

            if (routeMatches.Count == 0)
            {
                Console.WriteLine("\nSorry, no flights are available for that route.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                Menu.Start();
                return;
            }

            Console.Clear();
            Console.WriteLine("\x1b[3J");

            Console.WriteLine($"\nAVAILABLE FLIGHTS: {searchDeparture.ToUpper()} TO {searchDestination.ToUpper()}");
            foreach (FlightModel f in routeMatches)
            {
                Console.WriteLine($"- Flight {f.FlightNumber} departs at {f.DepartureTime}");
            }

            DateTime searchDate;

            while (true)
            {
                Console.Write("\nEnter your travel date (YYYY-MM-DD): ");
                string dateInput = Console.ReadLine();

                if (DateTime.TryParse(dateInput, out searchDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Input, Please follow the exact travel date format!");
                }
            }

            List<FlightModel> finalMatches = new List<FlightModel>();

            foreach (FlightModel f in routeMatches)
            {
                if (DateTime.Parse(f.DepartureTime).Date == searchDate.Date)
                {
                    finalMatches.Add(f);
                }
            }

            if (finalMatches.Count == 0)
            {
                Console.WriteLine("\nSorry, no flights found on that specific date.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                Menu.Start();
                return;
            }

            Console.Clear();
            Console.WriteLine($"\n FLIGHTS ON {searchDate.ToShortDateString()}");
            foreach (FlightModel f in finalMatches)
            {
                Console.WriteLine($"Flight Number: {f.FlightNumber} | Price: €{f.BasePrice}");
            }

            Console.WriteLine("\nEnter the flight number for more details/booking. Or type X to return to main menu.");
            string userChoice = Console.ReadLine();

            if (userChoice.Equals("X", StringComparison.OrdinalIgnoreCase))
            {
                Menu.Start();
            }
            else
            {
                ShowFlightDetails(userChoice, finalMatches);
            }
        }

        public static void ShowFlightDetails(string userChoice, List<FlightModel> flights)
        {
            bool specificFlightFound = false;

            foreach (FlightModel specificFlight in flights)
            {
                if (specificFlight.FlightNumber.Equals(userChoice, StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    Console.WriteLine($"Flight Number: {specificFlight.FlightNumber}");
                    Console.WriteLine($"Route: {specificFlight.DepartureCity} to {specificFlight.DestinationCity}");
                    Console.WriteLine($"Departure: {specificFlight.DepartureTime}");
                    Console.WriteLine($"Economy: €{specificFlight.BasePrice}");
                    Console.WriteLine("Available Seats: 42");

                    specificFlightFound = true;

                    Console.WriteLine("\nOptions:");
                    Console.WriteLine("1. Proceed to Booking");
                    Console.WriteLine("2. Return to Main Menu");

                    while (true)
                    {
                        Console.Write("\nEnter your choice (1 or 2): ");
                        string bookingChoice = Console.ReadLine();

                        if (bookingChoice == "1")
                        {
                            if (AccountsLogic.CurrentAccount == null)
                            {
                                Console.WriteLine("\nYou need to be logged in before booking a flight.");
                                Console.WriteLine("Press any key to go to login/register...");
                                Console.ReadKey();
                                Menu.AccountMenu();
                                return;
                            }
                            
                            PassangerModel passenger = new PassangerModel(
                                AccountsLogic.CurrentAccount.FirstName,
                                AccountsLogic.CurrentAccount.LastName,
                                AccountsLogic.CurrentAccount.DateOfBirth,
                                123456
                            );

                            string bookingDate = DateTime.Parse(specificFlight.DepartureTime).ToString("yyyy-MM-dd");

                            BookingLogic bookingLogic = new BookingLogic(
                                specificFlight,
                                AccountsLogic.CurrentAccount,
                                passenger,
                                bookingDate
                            );

                            var result = bookingLogic.BookTicket();

                            if (result.IsSuccesfull)
                            {
                                Console.WriteLine("\nBooking saved successfully!");
                            }
                            else
                            {
                                Console.WriteLine($"\nBooking failed: {result.ErrorMessage}");
                            }

                            Console.WriteLine("\nPress any key to return to the main menu.");
                            Console.ReadKey();
                            Menu.Start();
                            return;
                        }
                        else if (bookingChoice == "2")
                        {
                            Menu.Start();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please type 1 or 2.");
                        }
                    }

                    break;
                }
            }

            if (specificFlightFound == false)
            {
                Console.WriteLine("\nSorry, we couldn't find a flight with that number.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                Menu.Start();
            }
        }
    }
}