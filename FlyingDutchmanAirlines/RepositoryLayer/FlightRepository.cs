using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class FlightRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;
        
        public FlightRepository() {}
        public FlightRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public virtual async Task<Flight> GetFlightByFlightNumber(int flightNumber)
        {
            if (!flightNumber.IsPositive())
            {
                Console.WriteLine($"Could not find flight in GetFlightByFlightNumber! flightNumber = {flightNumber} ");
                throw new FlightNotFoundException();
            }

            return await _context.Flights.FirstOrDefaultAsync<Flight>(f => f.FlightNumber == flightNumber) 
                   ?? throw new FlightNotFoundException();
        }

        public virtual Queue<Flight> GetFlights()
        {
            Queue<Flight> flights = new();
            foreach (Flight flight in _context.Flights)
            {
                flights.Enqueue(flight);
            }
            
            return flights;
        }
    }
}