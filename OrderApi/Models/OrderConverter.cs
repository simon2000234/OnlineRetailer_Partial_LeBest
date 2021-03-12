using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedModels;

namespace OrderApi.Models
{
    public class OrderConverter : IConverter<Order, OrderDTO>
    {
        public Order Convert(OrderDTO sharedOrder)
        {
            var convertedOrderLine = new List<OrderLine>();
            if (sharedOrder.OrderLines != null)
            {
                foreach (var orderLine in sharedOrder.OrderLines)
                {
                    convertedOrderLine.Add(ConvertOderLine(orderLine));
                }
            }
            return new Order
            {
                Id = sharedOrder.Id,
                customerId = sharedOrder.customerId,
                Date = sharedOrder.Date,
                Status = (Order.OrderStatus)sharedOrder.Status,
                OrderLines = convertedOrderLine
            };
        }

        public OrderDTO Convert(Order hiddenOrder)
        {
            var convertedOrderLineDTO = new List<OrderLineDTO>();

            if (hiddenOrder.OrderLines != null)
            {
                foreach (var orderLine in hiddenOrder.OrderLines)
                {

                    convertedOrderLineDTO.Add(ConvertOderLine(orderLine));
                }
            }
            

            return new OrderDTO
            {
                Id = hiddenOrder.Id,
                customerId = hiddenOrder.customerId,
                Date = hiddenOrder.Date,
                Status = (OrderDTO.OrderStatus)hiddenOrder.Status,
                OrderLines = convertedOrderLineDTO
            };
        }

        public OrderLine ConvertOderLine(OrderLineDTO sharedOrderLine)
        {
            return new OrderLine
            {
                id = sharedOrderLine.id,
                OrderId = sharedOrderLine.OrderId,
                ProductId = sharedOrderLine.ProductId,
                Quantity = sharedOrderLine.Quantity
            };
        }

        public OrderLineDTO ConvertOderLine(OrderLine hiddenOrderLine)
        {
            return new OrderLineDTO
            {
                id = hiddenOrderLine.id,
                OrderId = hiddenOrderLine.OrderId,
                ProductId = hiddenOrderLine.ProductId,
                Quantity = hiddenOrderLine.Quantity
            };
        }


    }
}
