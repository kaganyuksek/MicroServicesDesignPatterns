namespace EventSourcing.Shared.Events
{
    public class ProductDeletedEvent : IEvent
    {
        public Guid Id { get; set; }

        public ProductDeletedEvent(Guid id)
        {
            Id = id;
        }

        public ProductDeletedEvent()
        {

        }
    }
}
