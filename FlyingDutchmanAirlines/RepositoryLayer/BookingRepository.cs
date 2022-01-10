using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class BookingRepository
    {
        private readonly FlyingDutchmanAirlinesContext _context;

<<<<<<< HEAD
        [MethodImpl(MethodImplOptions.NoInlining)]
        public BookingRepository()
        {
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor should only be used for testing");
            }
        }
=======
        // No inlining to ensure no stack frame magic happens (see Chapter 10)
        [MethodImpl(MethodImplOptions.NoInlining)]
        public BookingRepository() 
        { 
            // Executing != Calling when using in the test assembly
            if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
            {
                throw new Exception("This constructor should only be used for testing.");
            }
        }

>>>>>>> 02975db65acc9e1f1139970b75ab086e9ee49f65
        public BookingRepository(FlyingDutchmanAirlinesContext _context)
        {
            this._context = _context;
        }

        public virtual async Task CreateBooking(int customerID, int flightNumber)
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