using System;
using System.Collections.Generic;
using System.IO;
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
    public class AirportRepositoryTests
    {
        private FlyingDutchmanAirlinesContext _context;
        private AirportRepository _repository;
        
        [TestInitialize]
        public async Task TestInitialize()
        {
            DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions = new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>().UseInMemoryDatabase("FlyingDutchman").Options;

            _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

            SortedList<string, Airport> airports = new SortedList<string, Airport>
            {
                {
                    "MNL",
                    new Airport
                    {
                        AirportId = 0,
                        City = "Manila",
                        Iata = "MNL"
                    }

                },
                {
                    "PHX",
                    new Airport
                    {
                        AirportId = 1,
                        City = "Phoenix",
                        Iata = "PHX"
                    }
                },
                {
                    "DDH",
                    new Airport
                    {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                    }
                },
                {
                    "RDU",
                    new Airport
                    {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                    }
                }
            };
            
            _context.Airports.AddRange(airports.Values);
            await _context.SaveChangesAsync();

            _repository = new AirportRepository(_context);

            Assert.IsNotNull(_repository);
        }

        [TestMethod]
        public async Task GetAirportByID_Success()
        {
            Airport airport = await _repository.GetAirportByID(0);
            Assert.IsNotNull(airport);

            Assert.AreEqual(0, airport.AirportId);
            Assert.AreEqual("Manila", airport.City);
            Assert.AreEqual("MNL", airport.Iata);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAirportByID_Failure_InvalidArgument()
        {
            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                await _repository.GetAirportByID(-1);
                Assert.IsTrue(writer.ToString().Contains("Argument Exception in GetAirportByID! id = -1"));
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(AirportNotFoundException))]
        public async Task GetAirportByID_Failure()
        {
            await _repository.GetAirportByID(4);
        }
    }
    
}