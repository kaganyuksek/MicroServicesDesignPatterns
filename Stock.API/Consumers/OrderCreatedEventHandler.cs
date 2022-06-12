using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Events.Orders;
using Shared.Events.Stocks;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventHandler : IConsumer<OrderCreatedEvent>
    {
        private readonly AppDbContext Context;
        private ILogger<OrderCreatedEventHandler> Logger;
        private readonly ISendEndpointProvider SendEndpointProvider;
        private readonly IPublishEndpoint PublishEndpoint;

        public OrderCreatedEventHandler(
            AppDbContext context,
            ILogger<OrderCreatedEventHandler> logger,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint)
        {
            Context = context;
            Logger = logger;
            SendEndpointProvider = sendEndpointProvider;
            PublishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var checkResult = new List<bool>();

            foreach (var item in context.Message.OrderItems)
            {
                checkResult.Add(await Context.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count >= item.Count));
            }

            if (checkResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await Context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }
                }

                await Context.SaveChangesAsync();

                var sendEndpoint = await SendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueueConst.StockService.StockReservedEvent}"));

                await sendEndpoint.Send(
                    new StockReservedEvent(
                         context.Message.OrderId,
                         context.Message.BuyerId,
                         context.Message.TotalPrice,
                         context.Message.OrderItems
                        )
                    );
            }
            else
            {
                await PublishEndpoint.Publish(
                    new StockNotReservedEvent(
                        context.Message.OrderId,
                        "Not enough stock."
                        )
                    );
            }
        }
    }
}
