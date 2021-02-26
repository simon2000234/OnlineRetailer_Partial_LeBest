using System.Collections.Generic;
using CustomerAPI.Models;
using System.Linq;

namespace CustomerAPI.Data
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(CustomerApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Products
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> orders = new List<Customer>
            {
                new Customer { Id = 1, BillingAddress = "Hvor din mor bor", CreditStanding = 100, Email = "Chad@email.com", Name = "Chad Chadsen", Phone = "+45-42069420", ShippingAddress = "The Chad Cave"}
            };

            context.Customers.AddRange(orders);
            context.SaveChanges();
        }
    }
}