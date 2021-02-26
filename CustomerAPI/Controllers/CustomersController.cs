using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Data;
using CustomerAPI.Models;

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
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var newCustomer = repo.Add(customer);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
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
