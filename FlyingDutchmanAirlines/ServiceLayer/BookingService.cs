using System;
using System.Threading.Tasks;
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
            return (true, null);
        }
    }
}