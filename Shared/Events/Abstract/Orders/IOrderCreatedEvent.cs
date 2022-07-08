using MassTransit;

namespace Shared.Events.Orders
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}