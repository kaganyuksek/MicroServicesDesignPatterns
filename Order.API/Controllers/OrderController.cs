using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;
using Shared.Events.Orders;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // For test purposes.
        private readonly AppDbContext Context;
        private readonly IPublishEndpoint PublishEndpoint;
        private readonly ISendEndpointProvider SendEndpoint;

        public OrderController(AppDbContext context, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            Context = context;
            PublishEndpoint = publishEndpoint;
            SendEndpoint = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto order)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var orderItems = order.Items.Select(x => new OrderItem(x.ProductId, x.Price, x.Count)).ToList();
                    var billing = new Address(order.BillingInfo.Line, order.BillingInfo.Province, order.BillingInfo.District);
                    var newOrder = new Models.Order(order.BuyerId, billing, orderItems);

                    await Context.Orders.AddAsync(newOrder);
                    await Context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var orderCreatedEvent = new OrderCreatedEvent
                    {
                        BuyerId = newOrder.BuyerId,
                        OrderId = newOrder.Id,
                        TotalPrice = newOrder.TotalAmount,
                        CardInfo = new CardInformation(order.Payment.HolderNameSurname, order.Payment.CardNumber, order.Payment.ExpireDate, order.Payment.CVV),
                        OrderItems = newOrder.Items.Select(x => new OrderItemMessage(x.ProductId, x.Count)).ToList()
                    };

                    //await SendEndpoint.GetSendEndpoint(new Uri("rabbitmq://localhost/order-created"));
                    await SendEndpoint.Send(orderCreatedEvent); 
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }

            return Accepted();
        }
    }
}
