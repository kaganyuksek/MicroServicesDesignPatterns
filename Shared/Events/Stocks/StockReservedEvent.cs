using Shared.Events.Orders;

namespace Shared.Events.Stocks
{
    public class StockReservedEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal Amount { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new();


        public StockReservedEvent(Guid orderId, Guid buyerId, decimal amount, List<OrderItemMessage> orderItems)
        {
            OrderId = orderId;
            BuyerId = buyerId;
            Amount = amount;
            OrderItems = orderItems;
        }
    }
}
