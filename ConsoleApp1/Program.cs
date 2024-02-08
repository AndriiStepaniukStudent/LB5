using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Flight
{
    public string FlightNumber { get; set; }
    public string Airline { get; set; }
    public string Destination { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string Gate { get; set; }
    public FlightStatus Status { get; set; }
    public TimeSpan Duration { get; set; }
    public string AircraftType { get; set; }
    public string Terminal { get; set; }
}

public enum FlightStatus
{
    OnTime,
    Delayed,
    Cancelled,
    Boarding,
    InFlight
}

public class FlightInformationSystem
{
    private List<Flight> flights = new();

    public void LoadFlightsFromJson(string filePath)
    {
        try
        {
            string jsonData = File.ReadAllText(filePath);
            var flightsData = JsonConvert.DeserializeObject<FlightData>(jsonData);
            if (flightsData != null && flightsData.Flights != null)
            {
                foreach (var flight in flightsData.Flights)
                {
                    //flight.TrimAllProperties();
                    flights.Add(flight);
                }
                Console.WriteLine("Flights loaded successfully.");
            }
            else
            {
                Console.WriteLine("No flights data found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading flights: {ex.Message}");
        }
    }

    public void AddFlight(Flight flight)
    {
        flights.Add(flight);
        Console.WriteLine($"Flight {flight.FlightNumber} added successfully.");
    }

    public void RemoveFlight(string flightNumber)
    {
        var flightToRemove = flights.Find(f => f.FlightNumber == flightNumber);
        if (flightToRemove != null)
        {
            flights.Remove(flightToRemove);
            Console.WriteLine($"Flight {flightNumber} removed successfully.");
        }
        else
        {
            Console.WriteLine($"Flight {flightNumber} not found.");
        }
    }

    public Flight FindFlight(string flightNumber)
    {
        return flights.Find(f => f.FlightNumber == flightNumber);
    }


    public void PrintFlightCountByAirline(string airline)
    {
        int count = flights.FindAll(f => f.Airline.Equals(airline, StringComparison.OrdinalIgnoreCase)).Count;
        Console.WriteLine($"Number of flights by {airline}: {count}");
    }

    public string GetFlightsJsonByAirline(string airline)
    {
        var filteredFlights = flights.FindAll(f => f.Airline.Equals(airline, StringComparison.OrdinalIgnoreCase));
        return SerializeFlightsToJson(filteredFlights);
    }

    private string SerializeFlightsToJson(List<Flight> flights)
    {
        var flightData = new FlightData { Flights = flights };
        return JsonConvert.SerializeObject(flightData, Formatting.Indented);
    }

    private class FlightData
    {
        public required List<Flight> Flights { get; set; }
    }

    static void Main(string[] args)
    {
        var flightSystem = new FlightInformationSystem();
        flightSystem.LoadFlightsFromJson("C:\\Users\\dimas\\source\\repos\\ConsoleApp1\\ConsoleApp1\\file.json");

        var wizAirFlights = flightSystem.FindFlight("AA963");
        PrintFlights("WizAir Flights:", wizAirFlights);

        flightSystem.RemoveFlight("AA963");
        flightSystem.AddFlight(new Flight());

        Flight newFlight = new Flight
        {
            FlightNumber = "AB1237583",
            Airline = "Example Airlines",
            Destination = "Example Destination",
            DepartureTime = DateTime.UtcNow,
            ArrivalTime = DateTime.UtcNow.AddHours(2),
            Status = FlightStatus.OnTime,
            Duration = TimeSpan.FromHours(2),
            AircraftType = "Boeing 737",
            Terminal = "A"
        };

        flightSystem.AddFlight(newFlight);
        var wizAirFlights2 = flightSystem.FindFlight("DL206");
        PrintFlights("WizAir Flights:", wizAirFlights2);
        List<Flight> flightsList = [newFlight];
        var stringFlight = flightSystem.SerializeFlightsToJson(flightsList);
        Console.WriteLine(stringFlight);
    }

    static void PrintFlights(string title, Flight flight)
    {
        Console.WriteLine(title);
        Console.WriteLine($"Flight Number: {flight.FlightNumber}, Airline: {flight.Airline}, Destination: {flight.Destination}, Departure Time: {flight.DepartureTime}, Arrival Time: {flight.ArrivalTime}, Status: {flight.Status}");
        Console.WriteLine();
    }
}
