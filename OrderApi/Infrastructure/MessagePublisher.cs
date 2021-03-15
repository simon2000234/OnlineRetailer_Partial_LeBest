using System;
using System.Collections.Generic;
using EasyNetQ;
using SharedModels;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public void PublishPaymentRequest(decimal amount, int custId)
        {
            var paymentRequest = new CustomerPaymentRequestMessage
            {
                CustId = custId,
                Payment = amount
            };

            bus.PubSub.Publish(paymentRequest, "PaymentRequest");
        }

        public void PublishOrderStatusChangedMessage(int? customerId, IList<OrderLineDTO> orderLines, string topic)
        {
            var message = new OrderStatusChangedMessage
            { 
                CustomerId = customerId,
                OrderLines = orderLines 
            };

            bus.PubSub.Publish(message, topic);
        }

    }
}
