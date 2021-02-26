using OrderApi.Data;
using OrderApi.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> repository;

        public OrderService(IRepository<Order> repo)
        {
            repository = repo;

        }

        public IEnumerable<Order> GetAllOrders()
        {
            return repository.GetAll();
        }
        public Order getOrder(int id)
        {
            var orderToGet = repository.Get(id);
            return orderToGet;
        }
        public Order CreateOrder(Order order)
        {
            // Call ProductApi to get the product ordered
            RestClient c = new RestClient();
            // You may need to change the port number in the BaseUrl below
            // before you can run the request.
            c.BaseUrl = new Uri("https://localhost:44396/products/");
            var request = new RestRequest(order.ProductId.ToString(), Method.GET);
            var response = c.Execute<Product>(request);
            var orderedProduct = response.Data;

            if (order.Quantity <= orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
            {
                // reduce the number of items in stock for the ordered product,
                // and create a new order.
                orderedProduct.ItemsReserved += order.Quantity;
                var updateRequest = new RestRequest(orderedProduct.Id.ToString(), Method.PUT);
                updateRequest.AddJsonBody(orderedProduct);
                var updateResponse = c.Execute(updateRequest);

                if (updateResponse.IsSuccessful)
                {
                    var newOrder = repository.Add(order);
                    return newOrder;
                }
            }
            throw new ArgumentOutOfRangeException("You try to order more than there is in stock");
        }

    }
}
