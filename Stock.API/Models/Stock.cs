namespace Stock.API.Models
{
    public class Stock
    {
        public Guid Id { get; private set; }
        public int ProductId { get; private set; }
        public int Count { get; set; }

        public Stock(
            int productId,
            int count
            )
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Count = count;
        }
    }
}
