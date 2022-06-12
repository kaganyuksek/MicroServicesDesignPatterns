using Order.API.DTOs.Orders;

namespace Order.API.DTOs
{
    public sealed class OrderCreateDto
    {
        public Guid BuyerId { get; set; }
        public OrderPaymentDto Payment { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public OrderBillingDto BillingInfo { get; set; }
    }
}
