using System;
using System.Threading;
using CustomerAPI.Data;
using CustomerAPI.Models;
using CustomerAPI.Services;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using SharedModels;

namespace CustomerAPI.Infrastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;
        private readonly ICustomerService service;



        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.PubSub.Subscribe<CustomerPaymentRequestMessage>("RequestForPayment",
                    HandelCustomerRequest, x => x.WithTopic("PaymentRequest"));



                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }


        private void HandelCustomerRequest(CustomerPaymentRequestMessage cprm)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var customerRepo = services.GetService<IRepository<Customer>>();

                var customer = customerRepo.Get(cprm.CustId);
                customer.CreditStanding = customer.CreditStanding - cprm.Payment; 
                customerRepo.Edit(customer);
            }
        }
    }
}