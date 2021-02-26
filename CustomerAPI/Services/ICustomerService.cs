using CustomerAPI.Models;
using System.Collections.Generic;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
        void DeleteCustomer(int id);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomer(int id);
        Customer UpdateCustomer(int id, Customer customer);
    }
}