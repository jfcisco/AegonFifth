using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer
{
    [TestClass]
    public class FlightControllerTests
    {
        private Mock<FlightService> _mockFlightService;
        
        [TestInitialize]
        public void InitializeTests()
        {
            _mockFlightService = new Mock<FlightService>();   
        }
        
        [TestMethod]
        public async Task GetFlights_Success()
        {
            List<FlightView> mockFlightViews = new()
            {
                new FlightView(
                    "1832",
                    ("Groningen", "GRQ"),
                    ("Phoenix", "PHX")),
                new FlightView(
                    "841",
                    ("New York City", "NYC"),
                    ("London", "LHR"))
            };

            _mockFlightService.Setup(service => service.GetFlights())
                .Returns(AsyncEnumerableFlightViewGenerator(mockFlightViews));
            FlightController controller = new(_mockFlightService.Object);
            ObjectResult response = await controller.GetFlights() as ObjectResult;
            
            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.OK, response.StatusCode);
            Queue<FlightView> flightViews = response.Value as Queue<FlightView>;
            Assert.IsNotNull(flightViews);
            
            // Check that all mockFlightViews are returned by controller
            Assert.IsTrue(flightViews.All(view => mockFlightViews.Contains(view)));
        }

        [TestMethod]
        public async Task GetFlights_Failure_FlightNotFoundException_404()
        {
            _mockFlightService.Setup(service => service.GetFlights()).Throws(new FlightNotFoundException());
            FlightController controller = new(_mockFlightService.Object);

            ObjectResult response = await controller.GetFlights() as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("No flights were found in the database", response.Value);
        }

        [TestMethod]
        public async Task GetFlights_Failure_GeneralException_500()
        {
            _mockFlightService.Setup(service => service.GetFlights()).Throws(new StackOverflowException());
            FlightController controller = new(_mockFlightService.Object);
            
            ObjectResult response = await controller.GetFlights() as ObjectResult;
            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual("An error occured", response.Value);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Success()
        {
            FlightView mockFlightView = new("0", ("Lagos", "LOS"), ("Marrakesh", "RAK"));
            _mockFlightService.Setup(service => service.GetFlightByFlightNumber(0)).ReturnsAsync(mockFlightView);

            FlightController controller = new(_mockFlightService.Object);
            ObjectResult response = await controller.GetFlightByFlightNumber(0) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(mockFlightView, response.Value);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Failure_FlightNotFoundException_404()
        {
            _mockFlightService.Setup(service => service.GetFlightByFlightNumber(-1))
                .ThrowsAsync(new FlightNotFoundException());

            FlightController controller = new(_mockFlightService.Object);
            ObjectResult response = await controller.GetFlightByFlightNumber(-1) as ObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual("Flight not found", response.Value);
        }

        [TestMethod]
        public async Task GetFlightByFlightNumber_Failure_ArgumentException_400()
        {
            _mockFlightService.Setup(service => service.GetFlightByFlightNumber(-1))
                .ThrowsAsync(new ArgumentException());

            FlightController controller = new(_mockFlightService.Object);
            ObjectResult response = await controller.GetFlightByFlightNumber(-1) as ObjectResult;
            
            Assert.IsNotNull(response);
            Assert.AreEqual((int) HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Invalid flight number supplied", response.Value);
        }

#pragma warning disable 1998
        private async IAsyncEnumerable<FlightView> AsyncEnumerableFlightViewGenerator(IEnumerable<FlightView> flights)
#pragma warning restore 1998
        {
            foreach (FlightView flight in flights)
            {
                yield return flight;
            }
        }
    }
}