using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourcing.API.EventStores
{
    public abstract class StoreStreamBase
    {
        protected readonly LinkedList<IEvent> Events = new();
        
        private string StreamName { get; }

        private readonly IEventStoreConnection EventStoreConnection;

        protected StoreStreamBase(string streamName, IEventStoreConnection eventStoreConnection)
        {
            StreamName = streamName;
            EventStoreConnection = eventStoreConnection;
        }

        public async Task SaveAsync()
        {
            var newEvents = Events.ToList().Select(x => 
                new EventData(Guid.NewGuid(), 
                x.GetType().Name, 
                true, 
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(x, inputType: x.GetType())),
                Encoding.UTF8.GetBytes(x.GetType().FullName))
                ).ToList();

            await EventStoreConnection.AppendToStreamAsync(StreamName, ExpectedVersion.Any, newEvents);

            Events.Clear();
        }
    }
}
