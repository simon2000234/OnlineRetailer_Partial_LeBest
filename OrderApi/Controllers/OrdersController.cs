using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Services;
using PublicModels;
using RestSharp;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService service)
        {
            orderService = service;
        }

        // GET: orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var privateOrders = orderService.GetAllOrders();
            List<PublicOrder> publicOrders = new List<PublicOrder>();

            foreach (var order in privateOrders)
            {
                PublicOrder publicOrder = new PublicOrder
                {
                    Date = order.Date, Id = order.Id, ProductId = order.ProductId, Quantity = order.Quantity,
                    Status = order.Status
                };
            }

            return orderService.GetAllOrders();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = orderService.getOrder(id);
            if (item == null)
            {
                return NotFound();
            }

            PublicOrder publicOrder = new PublicOrder
            {
                Date = item.Date,
                Id = item.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Status = item.Status
            };
            return new ObjectResult(publicOrder);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody]PublicOrder order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            Order privatOrder = new Order
            {
                Status = order.Status, Quantity = order.Quantity, Date = order.Date, Id = order.Id,
                ProductId = order.ProductId
            };

            orderService.CreateOrder(privatOrder);

            // If the order could not be created, "return no content".
            return NoContent();
        }

    }
}
