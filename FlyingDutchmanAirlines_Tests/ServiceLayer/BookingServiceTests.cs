using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines_Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class BookingServiceTests
    {
        private Mock<BookingRepository> _mockBookingRepository;
        private Mock<CustomerRepository> _mockCustomerRepository;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockBookingRepository = new();
            _mockCustomerRepository = new();
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Leo Tolstoy"))
                .Returns(Task.FromResult(new Customer("Leo Tolstoy")));

            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);

            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        [DataRow("", 0)]
        [DataRow(null, -1)]
        [DataRow("Galileo", -1)]
        public async Task CreateBooking_Failure_InvalidArguments(string name, int flightNumber)
        {
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object);

            (bool result, Exception exception) = await service.CreateBooking(name, flightNumber);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public async Task CreateBooking_Failure_RepositoryArgumentException()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 1))
                .Throws(new ArgumentException());
            _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Leo Tolstoy"))
                .Returns(Task.FromResult(new Customer("Leo Tolstoy") { CustomerId = 0 }));
            
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object);

            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 1);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentException));
        }

        [TestMethod]
        public async Task CreateBooking_Failure_CouldNotAddBookingToDatabase()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(1, 2))
                .Throws(new CouldNotAddBookingToDatabaseException());
            _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Eise Eisinga"))
                .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));
            
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Eise Eisinga", 2);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}