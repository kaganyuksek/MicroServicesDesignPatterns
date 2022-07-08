using MassTransit;
using Shared.Events.Orders;
using System.Text.Json;

namespace SagaStateMachine.Worker.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<Guid>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Initially(When(OrderCreatedRequestEvent).Then(context => {
                context.Instance.OrderId = context.Data.OrderId;
                context.Instance.BuyerId = context.Data.BuyerId;
                context.Instance.CreatedDate = DateTime.Now;
                context.Instance.OrderItems = JsonSerializer.Serialize(context.Data.OrderItems);
            })
                .Then(context => {
                    Console.WriteLine($"Order Created - Before: {context.Data.OrderId}");
                })
                .Publish(context => new OrderCreatedEvent((Guid)context.CorrelationId, context.Data.OrderItems))
                .TransitionTo(OrderCreated));
        }

        
    }
}
