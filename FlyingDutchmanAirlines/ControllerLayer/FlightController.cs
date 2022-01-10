using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    [Route("[controller]")]
    public class FlightController : Controller
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }
        
        /// <summary>
        /// Get all available flights
        /// </summary>
        /// <returns>Returns all available flights</returns>
        /// <response code="404">No flights found</response>
        /// <response code="500">Internal error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FlightView>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFlights()
        {
            try
            {
                Queue<FlightView> flights = new();

                await foreach (FlightView flight in _flightService.GetFlights())
                {
                    flights.Enqueue(flight);
                }

                return StatusCode((int) HttpStatusCode.OK, flights);
            }
            catch (FlightNotFoundException)
            {
                return StatusCode((int) HttpStatusCode.NotFound, "No flights were found in the database");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int) HttpStatusCode.InternalServerError, "An error occured");
            }
        }

        /// <summary>
        /// Find flight by flight number
        /// </summary>
        /// <param name="flightNumber">Number of flight to return</param>
        /// <returns>Returns a single flight</returns>
        /// <response code="400">Invalid flight number supplied</response>
        /// <response code="404">Flight not found</response>
        [HttpGet("{flightNumber}")]
        [ProducesResponseType(typeof(FlightView), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.BadRequest,
                    "Invalid flight number supplied");

            try
            {
                FlightView flight = await _flightService.GetFlightByFlightNumber(flightNumber);
                return StatusCode((int) HttpStatusCode.OK, flight);
            }
            catch (FlightNotFoundException)
            {
                return StatusCode((int) HttpStatusCode.NotFound, "Flight not found");
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.BadRequest, "Invalid flight number supplied");
            }
        }
    }
}