namespace EventSourcing.Shared.Events
{
    public class ProductNameChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ProductNameChangedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public ProductNameChangedEvent()
        {

        }
    }
}
