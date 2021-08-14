using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    [Route("{controller}")]
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }
        
        [HttpPost("{flightNumber}")]
        public async Task<IActionResult> CreateBooking(
            [FromBody] BookingData body,
            int flightNumber)
        {
            if (ModelState.IsValid && flightNumber.IsPositive())
            {
                string customerName = $"{body.FirstName} {body.LastName}"; 
                (bool result, Exception exception) = await _bookingService.CreateBooking(customerName, flightNumber);

                if (result)
                {
                    return StatusCode((int)HttpStatusCode.Created);
                }
                else
                {
                    // Deviating from book since specs call for status code 500 for any internal server errors
                    return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
                }
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, ModelState.Root.Errors.First().ErrorMessage);
        }
    }
}