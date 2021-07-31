using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FlightRepositoryTests
    {
        private FlyingDutchmanAirlinesContext_Stub _context;
        private FlightRepository _repository;
    
        [TestInitialize]
        public async Task TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

            Flight flight = new Flight
            {
                FlightNumber = 1,
                Origin = 1,
                Destination = 2
            };

            Flight flight2 = new Flight
            {
                FlightNumber = 3,
                Origin = 3,
                Destination = 4
            };

             _context.Flights.Add(flight);
             _context.Flights.Add(flight2);
             await _context.SaveChangesAsync();

            _repository = new FlightRepository(_context);
            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public async Task FlightRepository_GetFlightByFlightNumber_Success()
        {
            Flight flight = await _repository.GetFlightByFlightNumber(1);
            Assert.IsNotNull(flight);

            Flight dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == 1);
            Assert.IsNotNull(dbFlight);

            Assert.AreEqual(dbFlight.FlightNumber, flight.FlightNumber);
            Assert.AreEqual(dbFlight.Origin, flight.Origin);
            Assert.AreEqual(dbFlight.Destination, flight.Destination);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task FlightRepository_GetFlightByFlightNumber_Failure_InvalidFlightNumber()
        {
            await _repository.GetFlightByFlightNumber(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(FlightNotFoundException))]
        public async Task FlightRepository_GetFlightByFlightNumber_Failure_DatabaseException()
        {
            await _repository.GetFlightByFlightNumber(2);
        }

        [TestMethod]
        public void FlightRepository_GetFlights_Success()
        {
            Queue<Flight> flights = _repository.GetFlights();

            Flight flight = flights.Dequeue();
            Assert.AreEqual(flight.FlightNumber, 1);
            Assert.AreEqual(flight.Origin, 1);
            Assert.AreEqual(flight.Destination, 2);

            flight = flights.Dequeue();
            Assert.AreEqual(flight.FlightNumber, 3);
            Assert.AreEqual(flight.Origin, 3);
            Assert.AreEqual(flight.Destination, 4);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FlightRepository_GetFlights_Failure_DatabaseException()
        {
            // Manually trigger database error from flight
            _context.WillErrorOnGetFlights = true;
            _repository.GetFlights();
        }
    }
}