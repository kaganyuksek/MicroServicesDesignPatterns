namespace Order.API.Models
{
    public class Order
    {
        public Guid Id { get; private set; }
        public OrderStatus Status { get; private set; }
        public Guid BuyerId { get; private set; }
        public Address Address { get; set; }
        public string FailMessage { get; private set; }
        public DateTime CreatedDate { get; init; }
        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public decimal TotalAmount { get; private set; }

        public Order(
            Guid buyerId,
            Address billingAddress,
            List<OrderItem> items
            )
        {
            Id = Guid.NewGuid();
            Status = OrderStatus.Ordered;
            CreatedDate = DateTime.Now;
            FailMessage = string.Empty;

            BuyerId = buyerId;
            Address = billingAddress;
            Items = items;
            TotalAmount = items.Sum(x => x.Price);
        }

        public Order()
        {

        }

        public void Activate()
        {
            Status = OrderStatus.Success;
        }

        public void Failed(string failMessage)
        {
            FailMessage = failMessage;
            Status = OrderStatus.Fail;
        }
    }

    public enum OrderStatus
    {
        Ordered,
        Suspend,
        Success,
        Fail
    }
}
