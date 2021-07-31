using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class FlightServiceTests
    {
        private Mock<FlightRepository> _mockFlightRepository;
        private Mock<AirportRepository> _mockAirportRepository;
        
        [TestInitialize]
        public void InitializeTests()
        {
            _mockFlightRepository = new Mock<FlightRepository>();
            _mockAirportRepository = new Mock<AirportRepository>();
            
            Flight savedFlight = new()
            {
                FlightNumber = 148,
                Origin = 31,
                Destination = 92
            };
            
            Queue<Flight> mockReturn = new(1);
            mockReturn.Enqueue(savedFlight);

            _mockFlightRepository.Setup(repository => repository.GetFlights()).Returns(mockReturn);
        }
        
        [TestMethod]
        public async Task GetFlights_Success()
        {
            _mockAirportRepository.Setup(repository => repository.GetAirportByID(31)).ReturnsAsync(new Airport()
            {
                AirportId = 31,
                City = "Mexico City",
                Iata = "MEX"
            });
            _mockAirportRepository.Setup(repository => repository.GetAirportByID(92)).ReturnsAsync(new Airport()
            {
                AirportId = 92,
                City = "Ulaanbaatar",
                Iata = "UBN"
            });
            
            FlightService service = new(_mockFlightRepository.Object, _mockAirportRepository.Object);

            await foreach (FlightView flightView in service.GetFlights())
            {
                Assert.IsNotNull(flightView);
                Assert.AreEqual("148", flightView.FlightNumber);
                Assert.AreEqual("Mexico City", flightView.Origin.City);
                Assert.AreEqual("MEX", flightView.Origin.Code);
                Assert.AreEqual("Ulaanbaatar", flightView.Destination.City);
                Assert.AreEqual("UBN", flightView.Destination.Code);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task GetFlights_Failure_FlightNotFound()
        {
            _mockAirportRepository.Setup(repository => repository.GetAirportByID(31))
                .ThrowsAsync(new AirportNotFoundException());

            FlightService service = new(_mockFlightRepository.Object, _mockAirportRepository.Object);
            await foreach (FlightView _ in service.GetFlights()) {}
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFlights_Failure_GeneralException()
        {
            _mockAirportRepository.Setup(repository => repository.GetAirportByID(31))
                .ThrowsAsync(new NullReferenceException());

            FlightService service = new(_mockFlightRepository.Object, _mockAirportRepository.Object);
            await foreach (FlightView _ in service.GetFlights()) {}
        }
    }
}