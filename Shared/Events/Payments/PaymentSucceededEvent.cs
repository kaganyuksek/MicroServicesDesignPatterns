namespace Shared.Events.Payments
{
    public record PaymentSucceededEvent
    {
        public Guid OrderId { get; init; }

        public PaymentSucceededEvent(Guid orderId)
        {
            OrderId = orderId;
        }

        public PaymentSucceededEvent()
        {

        }
    }
}
