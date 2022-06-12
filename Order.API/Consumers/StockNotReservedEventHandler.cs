using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events.Stocks;

namespace Order.API.Consumers
{
    public class StockNotReservedEventHandler : IConsumer<StockNotReservedEvent>
    {
        private readonly AppDbContext Context;
        private readonly ILogger<PaymentFailedEventHandler> Logger;


        public StockNotReservedEventHandler(AppDbContext context, ILogger<PaymentFailedEventHandler> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await Context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);

            if (order == null)
            {
                Logger.LogInformation($"Order Id: {context.Message.OrderId} not found.");
                return;
            }

            order.Failed(context.Message.Message);
            await Context.SaveChangesAsync();
        }
    }
}
