using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.API.Models;
using Shared.Events.Payments;
using Shared.Events.Stocks;

namespace Payment.API.Consumers
{
    public class StockReservedEventHandler : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventHandler> Logger;
        private readonly IPublishEndpoint PublishEndpoint;
        private readonly AppDbContext Context;

        public StockReservedEventHandler(ILogger<StockReservedEventHandler> logger, IPublishEndpoint publishEndpoint, AppDbContext context)
        {
            Logger = logger;
            PublishEndpoint = publishEndpoint;
            Context = context;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var deposit = await Context.Deposits.FirstOrDefaultAsync(x => x.UserId == context.Message.BuyerId);

            if (deposit == null)
            {
                await Failed(context.Message, "User not found.");
                return;
            }

            if (deposit.Balance >= context.Message.Amount)
            {
                deposit.Balance -= context.Message.Amount;
                await Context.SaveChangesAsync();

                await PublishEndpoint.Publish(
                    new PaymentSucceededEvent(context.Message.OrderId)
                    );
            }
            else
                await Failed(context.Message, "Insufficient balance.");

        }

        private async Task Failed(StockReservedEvent message, string failMessage)
        {
            await PublishEndpoint.Publish(
                new PaymentFailedEvent(
                    message.OrderId,
                    failMessage,
                    message.OrderItems
                    )
                );
        }
    }
}
