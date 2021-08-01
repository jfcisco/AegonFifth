using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.ControllerLayer;
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
        [TestMethod]
        public async Task GetFlights_Success()
        {
            Mock<FlightService> mockFlightService = new();
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

            mockFlightService.Setup(service => service.GetFlights())
                .Returns(AsyncEnumerableFlightViewGenerator(mockFlightViews));
            FlightController controller = new(mockFlightService.Object);
            ObjectResult response = await controller.GetFlights() as ObjectResult;
            
            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
            Queue<FlightView> flightViews = response.Value as Queue<FlightView>;
            Assert.IsNotNull(flightViews);
            
            // Check that all mockFlightViews are returned by controller
            Assert.IsTrue(flightViews.All(view => mockFlightViews.Contains(view)));
            
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