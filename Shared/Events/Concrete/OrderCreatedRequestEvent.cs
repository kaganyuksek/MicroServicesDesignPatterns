using Shared.Events.Orders;

namespace Shared.Events
{
    public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new();
    }
}
