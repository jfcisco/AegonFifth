using System;
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
        
        public BookingService(BookingRepository bookingRepo, CustomerRepository customerRepo)
        {
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
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
                Customer customer;
                try
                {
                    customer = await _customerRepo.GetCustomerByName(customerName);
                }
                catch (CustomerNotFoundException)
                {
                    await _customerRepo.CreateCustomer(customerName);
                    // Recurse to try adding the booking again
                    return await CreateBooking(customerName, flightNumber);
                }

                await _bookingRepo.CreateBooking(customer.CustomerId, flightNumber);
                return (true, null);
            }
            catch (Exception e)
            {
                return (false, e);
            }
        }
    }
}