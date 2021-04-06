namespace FlyingDutchmanAirlines.RepositoryLayer
{
    public class CustomerRepository
    {
        public bool CreateCustomer(string name) 
        { 
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return true;
        }
    }
}