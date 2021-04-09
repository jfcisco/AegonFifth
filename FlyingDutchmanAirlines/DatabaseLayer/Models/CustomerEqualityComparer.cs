using System;
using System.Collections.Generic;
using System.Security.Cryptography;

#nullable disable

namespace FlyingDutchmanAirlines.DatabaseLayer.Models
{
    internal class CustomerEqualityComparer : EqualityComparer<Customer>
    {
        public override int GetHashCode(Customer obj)
        {
            int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue / 2);
            return (obj.CustomerId + obj.Name.Length + randomNumber).GetHashCode();
        }

        public override bool Equals(Customer x, Customer y)
        {
            return x.Name == y.Name && x.CustomerId == y.CustomerId;
        }
    }
}
