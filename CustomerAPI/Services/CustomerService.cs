using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Data;
using CustomerAPI.Models;

namespace CustomerAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> repo;
        public CustomerService(IRepository<Customer> repos)
        {
            repo = repos;
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return repo.GetAll();
        }

        public Customer GetCustomer(int id)
        {
            return repo.Get(id);
        }

        public Customer CreateCustomer(Customer customer)
        {
            return repo.Add(customer);
        }

      /*  public Customer UpdateCustomer(Customer modifiedCustomer, Customer customer)
        {
         

            modifiedCustomer.Name = customer.Name;
            modifiedCustomer.Phone = customer.Phone;
            modifiedCustomer.BillingAddress = customer.BillingAddress;
            modifiedCustomer.CreditStanding = customer.CreditStanding;
            modifiedCustomer.Email = customer.Email;
            modifiedCustomer.ShippingAddress = customer.ShippingAddress;

            repo.Edit(modifiedCustomer);
            return modifiedCustomer;

        }*/

        public void DeleteCustomer(int id)
        {
            repo.Remove(id);
        }

    }

}
