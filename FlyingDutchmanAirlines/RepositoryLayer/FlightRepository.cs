using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

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
            if (!flightNumber.IsPositive() || !originAirportId.IsPositive() || !destinationAirportId.IsPositive())
            {
                Console.WriteLine($"Argument exception in GetFlightByFlight Number! flightNumber = {flightNumber}, originAirportId = {originAirportId}, destinationAirportId = {destinationAirportId}");
                throw new ArgumentException("Invalid arguments provided.");
            }

            return new Flight();   
        }
    }
}