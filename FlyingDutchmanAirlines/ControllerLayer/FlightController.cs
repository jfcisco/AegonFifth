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
                return StatusCode((int) HttpStatusCode.NotFound);
            }
            catch (Exception exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}