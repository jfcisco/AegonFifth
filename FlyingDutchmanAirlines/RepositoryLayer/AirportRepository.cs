using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

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

        public async Task<Airport> GetAirportByID(int id)
        {
            if (id < 0) { throw new AirportNotFoundException(); }

            return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == id)
                ?? throw new AirportNotFoundException();
        }
    }
}