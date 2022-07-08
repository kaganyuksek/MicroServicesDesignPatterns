using Shared.Events.Orders;

namespace Shared.Events.Payments
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; }
        public List<OrderItemMessage> Items { get; set; }

        public PaymentFailedEvent(Guid orderId, string message, List<OrderItemMessage> items)
        {
            OrderId = orderId;
            Message = message;
            Items = items;
        }

        public PaymentFailedEvent()
        {

        }
    }
}
