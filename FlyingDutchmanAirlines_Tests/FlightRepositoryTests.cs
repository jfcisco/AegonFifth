using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class FlightRepositoryTests
    {
        private FlyingDutchmanAirlinesContext _context;
        private FlightRepository _repository;
    
        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
            _repository = new FlightRepository(_context);
            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public async Task FlightRepository_GetFlightByFlightNumber_Success()
        {
            Flight flight = await _repository.GetFlightByFlightNumber(0, 0, 0);
            Assert.IsNotNull(flight);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FlightRepository_GetFlightByFlightNumber_Failure_InvalidOriginAirportId()
        {
            await _repository.GetFlightByFlightNumber(0, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task FlightRepository_GetFlightByFlightNumber_Failure_InvalidDestinationAirportId()
        {
            await _repository.GetFlightByFlightNumber(0, 0, -1);
        }
    }
}