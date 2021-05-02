using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class BookingRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;
        public BookingRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public async Task CreateBooking(int customerID, int flightNumber)
        {
            // Validate parameters: customerID and flightNumber should never be negative.
            if (!customerID.IsPositive() || !flightNumber.IsPositive())
            {
                Console.WriteLine($"Argument Exception in CreateBooking! customerID = {customerID}, flightNumber = {flightNumber}.");
                throw new ArgumentException("Invalid arguments provided.");
            }

            // Create new Booking instance using an object initializer
            Booking newBooking = new Booking
            {
                CustomerId = customerID,
                FlightNumber = flightNumber
            };

            try
            {
                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();
            }
            // General catch for exception
            catch (Exception exception)
            {
                Console.WriteLine($"Exception during database query: {exception.Message}");
                throw new CouldNotAddBookingToDatabaseException();
            }
            
        }
    }
}