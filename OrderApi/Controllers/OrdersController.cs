﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderApi.Data;
using OrderApi.Infrastructure;
using OrderApi.Models;
using SharedModels;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IConverter<Order, OrderDTO> orderConverter;
        IOrderRepository repository;
        IProductServiceGateway<ProductDto> productServiceGateway;
        IMessagePublisher messagePublisher;
        ICustomerServiceGateway<PublicCustomer> customerServiceGateway;

        public OrdersController(IRepository<Order> repos,
            IProductServiceGateway<ProductDto> gateway,
            IMessagePublisher publisher, IConverter<Order, OrderDTO> converter, ICustomerServiceGateway<PublicCustomer> customerServiceGateway)
        {
            repository = repos as IOrderRepository;
            productServiceGateway = gateway;
            messagePublisher = publisher;
            orderConverter = converter;
            this.customerServiceGateway = customerServiceGateway;
        }

        // GET orders
        [HttpGet]
        public IEnumerable<OrderDTO> Get()
        {
            var orderDTOList = new List<OrderDTO>();
            foreach (var order in repository.GetAll())
            {
                var orderDTO = orderConverter.Convert(order);
                orderDTOList.Add(orderDTO);
            }

            return orderDTOList;
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody] OrderDTO order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            //Order order = orderConverter.Convert(orderShared);

            if (ProductItemsAvailable(order))
            {
                if (!CheckValidCustomer(order))
                {
                    return StatusCode(500, "Not enough credits or you have not paid for a previous item");

                }
                try
                {
                    // Publish OrderStatusChangedMessage. If this operation
                    // fails, the order will not be created
                    messagePublisher.PublishOrderStatusChangedMessage(
                        order.customerId, order.OrderLines, "completed");

                    // Create order.
                    order.Status = OrderDTO.OrderStatus.completed;

                    var newOrder = repository.Add(orderConverter.Convert(order));
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
                catch
                {
                    return StatusCode(500, "An error happened. Try again.");
                }
            }
            else
            {
                // If there are not enough product items available.
                return StatusCode(500, "Not enough items in stock.");
            }
        }

        private bool CheckValidCustomer(OrderDTO order)
        {
            if (order.customerId == null)
            {
                return false;
            }
            try
            {
                var customer = customerServiceGateway.Get(order.customerId.Value);

                if (customer == null)
                {
                    return false;
                }


                CheckPriceMessage cpm = new CheckPriceMessage { OrderLines = order.OrderLines };

                var price = productServiceGateway.CheckFunds(cpm);

                var fundsAvailable = customer.CreditStanding >= price;


                var orderList = repository.GetAll();
                bool everythingPaidFor = true;
                foreach (var orderInList in orderList)
                {
                    if (orderInList.customerId != null && orderInList.customerId.Value == order.customerId.Value && orderInList.Status != Order.OrderStatus.paid)
                    {
                        everythingPaidFor = false;
                    }
                }

                return fundsAvailable & everythingPaidFor;
            }
            catch (Exception e)
            {
                return false;
            }



        }

        private bool ProductItemsAvailable(OrderDTO order)
        {
            foreach (var orderLine in order.OrderLines)
            {
                // Call product service to get the product ordered.
                var orderedProduct = productServiceGateway.Get(orderLine.ProductId);
                if (orderLine.Quantity > orderedProduct.ItemsInStock - orderedProduct.ItemsReserved)
                {
                    return false;
                }
            }
            return true;
        }

        // PUT orders/5/cancel
        // This action method cancels an order and publishes an OrderStatusChangedMessage
        // with topic set to "cancelled".
        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {

            var order = repository.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            var newStatus = Order.OrderStatus.cancelled;
            if (newStatus == order.Status)
            {
                return new NoContentResult();
            }

            if (order.Status != Order.OrderStatus.completed)
            {
                return BadRequest();
            }

            try
            {
                var orderDTO = orderConverter.Convert(order);
                messagePublisher.PublishOrderStatusChangedMessage(
                    orderDTO.customerId, orderDTO.OrderLines, "cancelled");

                order.Status = newStatus;
                repository.Edit(order);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "Something went wrong :(");
            }
        }

        // PUT orders/5/ship
        // This action method ships an order and publishes an OrderStatusChangedMessage.
        // with topic set to "shipped".
        [HttpPut("{id}/ship")]
        public IActionResult Ship(int id)
        {
            var order = repository.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            var newStatus = Order.OrderStatus.shipped;
            if (newStatus == order.Status)
            {
                return new NoContentResult();
            }

            if (order.Status != Order.OrderStatus.completed)
            {
                return BadRequest();
            }

            try
            {
                var orderDTO = orderConverter.Convert(order);
                messagePublisher.PublishOrderStatusChangedMessage(
                    orderDTO.customerId, orderDTO.OrderLines, "shipped");

                order.Status = newStatus;
                repository.Edit(order);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "Something went wrong :(");
            }
        }

        // PUT orders/5/pay
        // This action method marks an order as paid and publishes an OrderPaidMessage
        // (which have not yet been implemented). The OrderPaidMessage should specify the
        // Id of the customer who placed the order, and a number that indicates how many
        // unpaid orders the customer has (not counting cancelled orders). 
        [HttpPut("{id}/pay")]
        public IActionResult Pay(int id)
        {
            var order = repository.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            var newStatus = Order.OrderStatus.paid;
            if (newStatus == order.Status)
            {
                return new NoContentResult();
            }

            if (order.Status != Order.OrderStatus.shipped)
            {
                return BadRequest();
            }

            try
            {
                var orderDTO = orderConverter.Convert(order);

                order.Status = newStatus;
                repository.Edit(order);

                CheckPriceMessage cpm = new CheckPriceMessage { OrderLines = orderDTO.OrderLines };

                var price = productServiceGateway.CheckFunds(cpm);
                messagePublisher.PublishPaymentRequest(price, order.customerId.Value);
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500, "Something went wrong :(");
            }

        }

    }
}
