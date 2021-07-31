using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using System;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class AirportRepository
    {
        private FlyingDutchmanAirlinesContext _context;
        public AirportRepository() { }

        public AirportRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public virtual async Task<Airport> GetAirportByID(int id)
        {
            if (!id.IsPositive()) 
            {
                Console.WriteLine($"Argument Exception in GetAirportByID! Airport ID = {id}"); 
                throw new ArgumentException("Invalid argument provided"); 
            }

            return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == id)
                ?? throw new AirportNotFoundException();
        }
    }
}