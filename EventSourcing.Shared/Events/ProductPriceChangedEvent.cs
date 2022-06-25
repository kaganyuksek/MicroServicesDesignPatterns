namespace EventSourcing.Shared.Events
{
    public class ProductPriceChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public decimal NewPrice { get; set; }

        public ProductPriceChangedEvent(Guid id, decimal newPrice)
        {
            Id = id;
            NewPrice = newPrice;
        }

        public ProductPriceChangedEvent()
        {

        }
    }
}
