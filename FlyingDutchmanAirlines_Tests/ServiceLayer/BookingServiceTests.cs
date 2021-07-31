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
        private Mock<FlightRepository> _mockFlightRepository;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockBookingRepository = new();
            _mockCustomerRepository = new();
            _mockFlightRepository = new();
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Leo Tolstoy"))
                .ReturnsAsync(new Customer("Leo Tolstoy"));
            _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
                .ReturnsAsync(new Flight());

            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);

            Assert.IsTrue(result);
            Assert.IsNull(exception);
        }

        [TestMethod]
        public async Task CreateBooking_Success_CustomerNotInDatabase()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
            _mockCustomerRepository.SetupSequence(repository => repository.GetCustomerByName("Leo Tolstoy"))
                .Throws(new CustomerNotFoundException()) // Tolstoy doesnt exist yet
                .ReturnsAsync(new Customer("Leo Tolstoy") { CustomerId = 0}); // Tolstoy exists
            _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
                .ReturnsAsync(new Flight());

            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);
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
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);

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
            _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(1))
                .Returns(Task.FromResult(new Flight()));
            
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);

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
            
            BookingService service = new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Eise Eisinga", 2);
            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }

        [TestMethod]
        public async Task CreateBooking_Failure_FlightNotInDatabase()
        {
            _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(19))
                .Throws(new FlightNotFoundException());
            BookingService service =
                new(_mockBookingRepository.Object, _mockCustomerRepository.Object, _mockFlightRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Maurits Escher", 19);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }

        [TestMethod]
        public async Task CreateBooking_Failure_CustomerNotInDatabase_RepositoryFailure()
        {
            _mockBookingRepository.Setup(repository => repository.CreateBooking(0, 0))
                .Throws(new CouldNotAddBookingToDatabaseException());
            _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Bill Gates"))
                .Returns(Task.FromResult(new Customer("Bill Gates")));
            _mockFlightRepository.Setup(repository => repository.GetFlightByFlightNumber(0))
                .Returns(Task.FromResult(new Flight()));

            BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object,
                _mockFlightRepository.Object);
            (bool result, Exception exception) = await service.CreateBooking("Bill Gates", 0);

            Assert.IsFalse(result);
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(CouldNotAddBookingToDatabaseException));
        }
    }
}