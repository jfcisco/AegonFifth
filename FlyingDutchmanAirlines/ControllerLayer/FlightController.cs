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
    public class FlightController : Controller
    {
        private readonly FlightService _flightService;

        public FlightController(FlightService flightService)
        {
            _flightService = flightService;
        }
        
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
            catch (FlightNotFoundException exception)
            {
                return StatusCode((int) HttpStatusCode.NotFound, "No flights were found in the database");
            }
            catch (Exception exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, "An error occured");
            }
        }

        public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
        {
            try
            {
                FlightView flight = await _flightService.GetFlightByFlightNumber(flightNumber);
                return StatusCode((int) HttpStatusCode.OK, flight);
            }
            catch (FlightNotFoundException)
            {
                return StatusCode((int) HttpStatusCode.NotFound, "Flight not found");
            }
            catch (ArgumentException)
            {
                return StatusCode((int) HttpStatusCode.BadRequest, "Invalid flight number supplied");
            }
        }
    }
}