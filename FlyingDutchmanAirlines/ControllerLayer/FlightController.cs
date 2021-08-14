using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    [Route("{controller}")]
    public class FlightController : Controller
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }
        
        [HttpGet]
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
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, "An error occured");
            }
        }

        [HttpGet("{flightNumber}")]
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