using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Services;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService service;

        public CustomersController(ICustomerService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return service.GetAllCustomers();
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = service.GetCustomer(id);
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

            var newCustomer = service.CreateCustomer(customer);

            return CreatedAtRoute("GetCustomer", new { id = newCustomer.Id }, newCustomer);
        }


       /* [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest();
            }

            var modifiedProduct = service.GetCustomer(id);

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

            service.UpdateCustomer(modifiedProduct);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (service.GetCustomer(id) == null)
            {
                return NotFound();
            }

            service.DeleteCustomer(id);
            return new NoContentResult();
        }*/
    }
}
