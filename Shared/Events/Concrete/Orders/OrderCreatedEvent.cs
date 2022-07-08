namespace Shared.Events.Orders
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();

        public Guid CorrelationId { get; private set; }

        public OrderCreatedEvent(Guid correlationId, List<OrderItemMessage> items)
        {
            CorrelationId = correlationId;
            OrderItems = items;
        }

        public OrderCreatedEvent()
        {

        }
    }

    public sealed class OrderItemMessage
    {
        public int ProductId { get; private set; }
        public int Count { get; private set; }

        public OrderItemMessage(
            int productId,
            int count
            )
        {
            ProductId = productId;
            Count = count;
        }
    }
}
