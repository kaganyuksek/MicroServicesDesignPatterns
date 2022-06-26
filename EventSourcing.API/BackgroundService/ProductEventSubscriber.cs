namespace EventSourcing.API.BackgroundService
{
    using EventSourcing.API.EventStores;
    using EventSourcing.API.Models;
    using EventSourcing.Shared.Events;
    using EventStore.ClientAPI;
    using Microsoft.Extensions.Hosting;
    using System.Text;
    using System.Text.Json;

    public class ProductEventSubscriber : BackgroundService
    {
        private readonly IEventStoreConnection EventStoreConnection;
        private readonly ILogger<ProductEventSubscriber> Logger;
        private readonly IServiceProvider ServiceProvider;

        public ProductEventSubscriber(IEventStoreConnection eventStoreConnection, ILogger<ProductEventSubscriber> logger, IServiceProvider serviceProvider)
        {
            EventStoreConnection = eventStoreConnection;
            Logger = logger;
            ServiceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await EventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName, ProductStream.StreamGroupName, EventAppeared, autoAck: false);
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase arg1, ResolvedEvent arg2)
        {
            Logger.LogInformation("Messages are proccessing...");

            var type = Type.GetType($"{Encoding.UTF8.GetString(arg2.Event.Metadata)}, EventSourcing.Shared");

            var eventData = Encoding.UTF8.GetString(arg2.Event.Data);

            var @event = JsonSerializer.Deserialize(eventData, type);

            using var scope = ServiceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Product? product = null;

            switch (@event)
            {
                case ProductCreatedEvent productCreated:
                    product = new Product()
                    {
                        Name = productCreated.Name,
                        Id = productCreated.Id,
                        Price = productCreated.Price,
                        Stock = productCreated.Stock,
                        UserId = productCreated.UserId
                    };

                    await context.Products.AddAsync(product);
                    break;
                case ProductNameChangedEvent productNameChanged:
                    product = await context.Products.FindAsync(productNameChanged.Id);

                    if (product is not null)
                    {
                        product.Name = productNameChanged.Name;
                    }

                    break;
                case ProductPriceChangedEvent productPriceChanged:
                    product = await context.Products.FindAsync(productPriceChanged.Id);

                    if (product is not null)
                    {
                        product.Price = productPriceChanged.NewPrice;
                    }

                    break;
                case ProductDeletedEvent productDeleted:
                    product = await context.Products.FindAsync(productDeleted.Id);
                    if (product is not null)
                    {
                        context.Products.Remove(product);
                    }
                    break;
                default:
                    break;
            }

            await context.SaveChangesAsync();

            arg1.Acknowledge(arg2.Event.EventId);
        }
    }
}
