using MassTransit;
using Shared.Events.Orders;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
