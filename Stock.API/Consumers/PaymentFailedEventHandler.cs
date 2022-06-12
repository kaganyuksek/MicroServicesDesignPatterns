using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.Payments;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventHandler : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext Context;
        private readonly ILogger<PaymentFailedEventHandler> Logger;

        public PaymentFailedEventHandler(AppDbContext context, ILogger<PaymentFailedEventHandler> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.Items)
            {
                var stock = await Context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}
