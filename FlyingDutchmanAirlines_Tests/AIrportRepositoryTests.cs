using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class AirportRepositoryTests
    {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;
        
        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;

            _context = new FlyingDutchmanAirlinesContext(dbContextOptions);
            _repository = new AirportRepository(_context);

            Assert.IsNotNull(_repository);

        }
    }
    
}