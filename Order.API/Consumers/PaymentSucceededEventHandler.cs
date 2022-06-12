using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events.Payments;

namespace Order.API.Consumers
{
    public class PaymentSucceededEventHandler : IConsumer<PaymentSucceededEvent>
    {
        private readonly AppDbContext Context;
        private readonly ILogger<PaymentSucceededEventHandler> Logger;

        public PaymentSucceededEventHandler(AppDbContext context, ILogger<PaymentSucceededEventHandler> logger)
        {
            Context = context;
            Logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
        {
            var order = await Context.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);

            if (order == null)
            {
                Logger.LogInformation($"Order not found. Id = {context.Message.OrderId}");
                return;
            }

            order.Activate();

            Context.Update(order);

            await Context.SaveChangesAsync();
        }
    }
}
