namespace Order.API.DTOs.Orders
{
    public sealed class OrderBillingDto
    {
        public string Line { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
