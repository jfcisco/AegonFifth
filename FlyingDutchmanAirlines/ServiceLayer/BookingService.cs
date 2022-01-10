<<<<<<< HEAD
﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
=======
using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Exceptions;
>>>>>>> 02975db65acc9e1f1139970b75ab086e9ee49f65

namespace FlyingDutchmanAirlines.ServiceLayer
{
    public class BookingService
    {
<<<<<<< HEAD
        private readonly BookingRepository _bookingRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly FlightRepository _flightRepo;
        
        public BookingService(BookingRepository bookingRepo, CustomerRepository customerRepo, FlightRepository flightRepo)
        {
            _bookingRepo = bookingRepo;
            _customerRepo = customerRepo;
            _flightRepo = flightRepo;
=======
        private BookingRepository _bookingRepository;
        private CustomerRepository _customerRepository;

        public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository) 
        {
            _bookingRepository = bookingRepository;
            _customerRepository = customerRepository;
>>>>>>> 02975db65acc9e1f1139970b75ab086e9ee49f65
        }

        public async Task<(bool, Exception)> CreateBooking(string customerName, int flightNumber)
        {
<<<<<<< HEAD
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
=======
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
>>>>>>> 02975db65acc9e1f1139970b75ab086e9ee49f65
        }
    }
}