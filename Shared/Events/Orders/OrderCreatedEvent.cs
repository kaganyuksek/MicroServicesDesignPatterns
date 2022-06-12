namespace Shared.Events.Orders
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public CardInformation CardInfo { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    }

    public record CardInformation
    {
        public string? HolderNameSurname { get; init; }
        public string? CardNumber { get; init; }
        public string? ExpireDate { get; init; }
        public string? CVV { get; init; }

        public CardInformation(
            string holderNameSurname,
            string cardNumber,
            string expireDate,
            string cVV
            )
        {
            HolderNameSurname = holderNameSurname;
            CardNumber = cardNumber;
            ExpireDate = expireDate;
            CVV = cVV;
        }

        public CardInformation()
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
