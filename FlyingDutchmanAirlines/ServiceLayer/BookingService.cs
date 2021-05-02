using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
        private BookingRepository _bookingRepository;
        private CustomerRepository _customerRepository;

        public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository) 
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
        {
           try
           {
               Customer customer;

               try
               {
                   customer = await _customerRepository.GetCustomerByName(customerName);
               }
               catch(CustomerNotFoundException)
               {
                   await _customerRepository.CreateCustomer(customerName);
                   return await CreateBooking(customerName, flightNumber);
               }

               await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
               return (true, null);
           }
           catch(Exception exception)
           {
               return (false, exception);
           }
        }
    }
}