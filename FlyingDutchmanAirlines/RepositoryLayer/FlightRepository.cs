using System;
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
        
        public FlightRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
        {
            if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive())
            {
                Console.WriteLine($"Argument exception in GetFlightByFlight Number! originAirportId = {originAirportId}, destinationAirportId = {destinationAirportId}");
                throw new ArgumentException("Invalid arguments provided.");
            }

            if (!flightNumber.IsPositive())
            {
                Console.WriteLine($"Could not find flight in GetFlightByFlightNumber! flightNumber = {flightNumber} ");
                throw new FlightNotFoundException();
            }

            return await _context.Flights.FirstOrDefaultAsync<Flight>(f => f.FlightNumber == flightNumber) ?? throw new FlightNotFoundException();
        }
    }
}