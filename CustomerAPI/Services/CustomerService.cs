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

        public Customer UpdateCustomer(int OldCustomerId, Customer customerNewData)
        {

            var modifiedCustomer = GetCustomer(OldCustomerId);


            modifiedCustomer.Name = customerNewData.Name;
            modifiedCustomer.Phone = customerNewData.Phone;
            modifiedCustomer.BillingAddress = customerNewData.BillingAddress;
            modifiedCustomer.CreditStanding = customerNewData.CreditStanding;
            modifiedCustomer.Email = customerNewData.Email;
            modifiedCustomer.ShippingAddress = customerNewData.ShippingAddress;

            repo.Edit(modifiedCustomer);
            return modifiedCustomer;

        }

        public void DeleteCustomer(int id)
        {
            repo.Remove(id);
        }

    }

}
