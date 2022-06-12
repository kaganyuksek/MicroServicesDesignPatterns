namespace Shared.Events.Stocks
{
    public record StockNotReservedEvent
    {
        public Guid OrderId { get; init; }
        public string Message { get; init; }

        public StockNotReservedEvent(Guid orderId, string message)
        {
            OrderId = orderId;
            Message = message;
        }
    }
}
