using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        [TestMethod]
        public void CreateCustomer_Success()
        {
            CustomerRepository repository = new CustomerRepository();
            Assert.IsNotNull(repository);

            bool canAddCustomer = repository.CreateCustomer("heyo");
            Assert.IsTrue(canAddCustomer);
        }

        [TestMethod]
        public void CreateCustomer_Failure_NameIsNull()
        {
            CustomerRepository repository = new CustomerRepository();
            Assert.IsNotNull(repository);

            bool result = repository.CreateCustomer(null);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateCustomer_Failure_NameIsEmptyString()
        {
            var repository = new CustomerRepository();
            Assert.IsNotNull(repository);

            bool result = repository.CreateCustomer("");
            Assert.IsFalse(result);
        }
    }
}
