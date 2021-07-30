using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class BookingServiceTests
    {
        private FlyingDutchmanAirlinesContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
                new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                    .UseInMemoryDatabase("FlyingDutchman").Options;
            
            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            BookingRepository repository = new(_context);
            CustomerRepository customerRepo = new(_context);
            BookingService service = new(repository, customerRepo);
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);
        }
    }
}