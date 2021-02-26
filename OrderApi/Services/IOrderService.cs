using OrderApi.Models;
using System.Collections.Generic;

namespace OrderApi.Services
{
    public interface IOrderService
    {
        Order CreateOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order getOrder(int id);
    }
}