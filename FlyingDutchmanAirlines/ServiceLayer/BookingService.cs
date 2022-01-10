using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly FlightRepository _flightRepo;
        
        public BookingService(BookingRepository bookingRepo, CustomerRepository customerRepo, FlightRepository flightRepo)
        {
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
            _flightRepo = flightRepo;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
        {
            // Validate arguments
            if (string.IsNullOrEmpty(customerName) || !flightNumber.IsPositive())
            {
                return (false, new ArgumentException());
            }
            
            try
            {
                // Get customer, or create record if not existing
                Customer customer = await GetCustomerFromDatabase(customerName) ??
                                    await AddCustomerToDatabase(customerName);
                
                
                // Check if flightNumber is for an existing flight
                if (!await FlightExistsInDatabase(flightNumber))
                    return (false, new CouldNotAddBookingToDatabaseException());

                await _bookingRepo.CreateBooking(customer.CustomerId, flightNumber);
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
        
        private async Task<bool> FlightExistsInDatabase(int flightNumber)
        {
            try
            {
                return await _flightRepo.GetFlightByFlightNumber(flightNumber) != null;
            }
            catch (FlightNotFoundException)
            {
                return false;
            }
        }
        
        private async Task<Customer> GetCustomerFromDatabase(string name)
        {
            try
            {
                return await _customerRepo.GetCustomerByName(name);
            }
            catch (CustomerNotFoundException)
            {
                return null;
            }
            catch (Exception e)
            {
                // Rethrow exception while preserving the stack trace of the original problem
                ExceptionDispatchInfo.Capture(e.InnerException ?? new Exception()).Throw();
                return null;
            }
        }

        private async Task<Customer> AddCustomerToDatabase(string name)
        {
            await _customerRepo.CreateCustomer(name);
            return await _customerRepo.GetCustomerByName(name);
        }
    }
}