using FlyingDutchmanAirlines.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer.Views
{
    [TestClass]
    public class FlightViewTests
    {
        [TestMethod]
        public void Constructor_FlightView_Success()
        {
            string flightNumber = "0";
            string originCity = "Amsterdam";
            string originCode = "AMS";
            string destinationCity = "Moscow";
            string destinationCode = "SVO";

            FlightView flightView = new(flightNumber, (originCity, originCode), (destinationCity, destinationCode));
            
            Assert.AreEqual(flightView.FlightNumber, flightNumber);
            Assert.AreEqual(flightView.Origin.City, originCity);
            Assert.AreEqual(flightView.Origin.Code, originCode);
            Assert.AreEqual(flightView.Destination.City, destinationCity);
            Assert.AreEqual(flightView.Destination.Code, destinationCode);
        }

        [TestMethod]
        public void Constructor_FlightView_Success_FlightNumber_Null()
        {
            string flightNumber = null;
            string originCity = "Amsterdam";
            string originCode = "AMS";
            string destinationCity = "Moscow";
            string destinationCode = "SVO";

            FlightView flightView = new(flightNumber, (originCity, originCode), (destinationCity, destinationCode));
            
            Assert.AreEqual(flightView.FlightNumber, "No flight number found.");
            Assert.AreEqual(flightView.Origin.City, originCity);
            Assert.AreEqual(flightView.Origin.Code, originCode);
            Assert.AreEqual(flightView.Destination.City, destinationCity);
            Assert.AreEqual(flightView.Destination.Code, destinationCode);
        }

        [TestMethod]
        public void Constructor_AirportInfo_Success_City_Empty()
        {
            string destinationCity = string.Empty;
            string destinationCode = "SVO";

            AirportInfo airportInfo = new((destinationCity, destinationCode));

            Assert.AreEqual(airportInfo.City, "No city found.");
            Assert.AreEqual(airportInfo.Code, destinationCode);
        }

        [TestMethod]
        public void Constructor_AirportInfo_Success_Code_Empty()
        {
            string destinationCity = "Moscow";
            string destinationCode = string.Empty;

            AirportInfo airportInfo = new((destinationCity, destinationCode));

            Assert.AreEqual(airportInfo.City, destinationCity);
            Assert.AreEqual(airportInfo.Code, "No code found.");
        }
    }
}