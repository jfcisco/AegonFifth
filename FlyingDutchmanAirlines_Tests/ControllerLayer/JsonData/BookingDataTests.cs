using System;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer.JsonData
{
    [TestClass]
    public class BookingDataTests
    {
        [TestMethod]
        public void BookingData_Success()
        {
            BookingData data = new()
            {
                FirstName = "Adam",
                LastName = "Smith"
            };
            
            Assert.IsNotNull(data);
            Assert.AreEqual("Adam", data.FirstName);
            Assert.AreEqual("Smith", data.LastName);
        }

        [TestMethod]
        [DataRow(null, "Lastname")]
        [DataRow("Firstname", null)]
        [DataRow("Firstname", "")]
        [DataRow("", "Lastname")]
        [ExpectedException(typeof(ArgumentException))]
        public void BookingData_Failure_ErrorNullOrEmptyNames(string firstName, string lastName)
        {
            BookingData data = new()
            {
                FirstName = firstName,
                LastName = lastName
            };
        }
    }
}