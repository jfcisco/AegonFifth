using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
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
        public async Task TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

            Airport testAirport = new Airport();
            _context.Airports.Add(testAirport);
            await _context.SaveChangesAsync();

            _repository = new AirportRepository(_context);

            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public async Task GetAirportByID_Success()
        {
            Airport airport = await _repository.GetAirportByID(1);
            Assert.IsNotNull(airport);
        }

        [TestMethod]
        [ExpectedException(typeof(AirportNotFoundException))]
        public async Task GetAirportByID_Failure_InvalidArgument()
        {
            await _repository.GetAirportByID(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(AirportNotFoundException))]
        public async Task GetAirportByID_Failure_AirportNotExisting()
        {
            await _repository.GetAirportByID(2);
        }
    }
    
}