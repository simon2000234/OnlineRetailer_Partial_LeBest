using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Data;
using CustomerAPI.Models;
using PublicModels;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly IRepository<Customer> repo;

        public CustomersController(IRepository<Customer> repos)
        {
            repo = repos;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            List<Customer> privateCustomers = repo.GetAll().ToList();
            List<PublicCustomer> publicCustomers = new List<PublicCustomer>();
            foreach (var item in privateCustomers)
            {
                PublicCustomer pubCust = new PublicCustomer
                {
                    BillingAddress = item.BillingAddress,
                    Name = item.Name,
                    CreditStanding = item.CreditStanding,
                    Email = item.Email,
                    Id = item.Id,
                    Phone = item.Phone,
                    ShippingAddress = item.ShippingAddress
                };
                publicCustomers.Add(pubCust);
            }

            return repo.GetAll();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repo.Get(id);
            if (item == null)
            {
                return NotFound();
            }

            PublicCustomer pubCust = new PublicCustomer
            {
                BillingAddress = item.BillingAddress, Name = item.Name, CreditStanding = item.CreditStanding,
                Email = item.Email, Id = item.Id, Phone = item.Phone, ShippingAddress = item.ShippingAddress
            };


            return new ObjectResult(pubCust);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PublicCustomer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            Customer privCust = new Customer()
            {
                Phone = customer.Phone, ShippingAddress = customer.ShippingAddress, Id = customer.Id,
                BillingAddress = customer.BillingAddress, CreditStanding = customer.CreditStanding,
                Email = customer.Email, Name = customer.Name
            };

            var newCustomer = repo.Add(privCust);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PublicCustomer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = repo.Get(id);

            if (modifiedProduct == null)
            {
                return NotFound();
            }

            modifiedProduct.Name = customer.Name;
            modifiedProduct.Phone = customer.Phone;
            modifiedProduct.BillingAddress = customer.BillingAddress;
            modifiedProduct.CreditStanding = customer.CreditStanding;
            modifiedProduct.Email = customer.Email;
            modifiedProduct.ShippingAddress = customer.ShippingAddress;

            repo.Edit(modifiedProduct);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }

            repo.Remove(id);
            return new NoContentResult();
        }
    }
}
