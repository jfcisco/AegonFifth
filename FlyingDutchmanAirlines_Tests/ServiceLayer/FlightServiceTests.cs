using System.Collections.Generic;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
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
        }
        
        [TestMethod]
        public async Task GetFlights_Success()
        {
            Flight flightSaved = new()
            {
                FlightNumber = 1,
                Origin = 2,
                Destination = 3
            };

            Queue<Flight> mockReturn = new Queue<Flight>();
            mockReturn.Enqueue(flightSaved);
            
            _mockFlightRepository.Setup(repository => repository.GetFlights())
                .Returns(mockReturn);

            _mockAirportRepository.Setup(repository => repository.GetAirportByID(2)).ReturnsAsync(new Airport()
            {
                AirportId = 2,
                City = "Mexico City",
                Iata = "MEX"
            });
            _mockAirportRepository.Setup(repository => repository.GetAirportByID(3)).ReturnsAsync(new Airport()
            {
                AirportId = 3,
                City = "Ulaanbaatar",
                Iata = "UBN"
            });
            
            FlightService service = new(_mockFlightRepository.Object, _mockAirportRepository.Object);

            await foreach (FlightView flightView in service.GetFlights())
            {
                Assert.IsNotNull(flightView);
                Assert.AreEqual("1", flightView.FlightNumber);
                Assert.AreEqual("Mexico City", flightView.Origin.City);
                Assert.AreEqual("MEX", flightView.Origin.Code);
                Assert.AreEqual("Ulaanbaatar", flightView.Destination.City);
                Assert.AreEqual("UBN", flightView.Destination.Code);
            }
        }
        
    }
}