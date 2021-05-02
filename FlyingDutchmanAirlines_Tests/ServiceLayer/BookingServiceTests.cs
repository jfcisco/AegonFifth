using System;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer
{
    [TestClass]
    public class BookingServiceTests
    {
        [TestInitialize]
        public void TestInitialize()
        {   
            
        }

        [TestMethod]
        public async Task CreateBooking_Success()
        {
            Mock<BookingRepository> mockBookingRepo = new Mock<BookingRepository>();
            mockBookingRepo.Setup(repository => repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
 
            Mock<CustomerRepository> mockCustomerRepo = new Mock<CustomerRepository>();
            mockCustomerRepo.Setup(repository => repository.GetCustomerByName("Leo Tolstoy")).Returns(Task.FromResult<Customer>(new Customer("Leo Tolstoy")));

            BookingService service = new BookingService(mockBookingRepo.Object, mockCustomerRepo.Object);
            (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);
            Assert.IsNotNull(service);
            Assert.IsTrue(result);
        }
    }
}