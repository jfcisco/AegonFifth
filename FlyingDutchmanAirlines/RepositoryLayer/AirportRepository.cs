using FlyingDutchmanAirlines.DatabaseLayer;

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
    }
}