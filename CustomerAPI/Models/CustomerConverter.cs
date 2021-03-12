using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels;

namespace CustomerAPI.Models
{
    public class CustomerConverter
    {
        public Customer Convert(PublicCustomer sharedCustomer)
        {
            return new Customer
            {
                Id = sharedCustomer.Id,
                Name = sharedCustomer.Name,
                Email = sharedCustomer.Email,
                Phone = sharedCustomer.Phone,
                BillingAddress = sharedCustomer.BillingAddress,
                ShippingAddress = sharedCustomer.ShippingAddress,
                CreditStanding = sharedCustomer.CreditStanding
            };
        }

        public PublicCustomer Convert(Customer hiddenCustomer)
        {
            return new PublicCustomer
            {
                Id = hiddenCustomer.Id,
                Name = hiddenCustomer.Name,
                Email = hiddenCustomer.Email,
                Phone = hiddenCustomer.Phone,
                BillingAddress = hiddenCustomer.BillingAddress,
                ShippingAddress = hiddenCustomer.ShippingAddress,
                CreditStanding = hiddenCustomer.CreditStanding
            };
        }

    }
}
