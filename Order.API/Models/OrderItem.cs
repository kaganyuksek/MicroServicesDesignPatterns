using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public int Count { get; set; }
        public virtual Order Order { get; set; }

        public OrderItem(
            int productId,
            decimal price,
            int count
            )
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Price = price;
            Count = count;
        }

        public OrderItem()
        {

        }
    }
}
