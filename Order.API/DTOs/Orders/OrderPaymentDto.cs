namespace Order.API.DTOs.Orders
{
    public sealed class OrderPaymentDto
    {
        public string HolderNameSurname { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CVV { get; set; }
    }
}
