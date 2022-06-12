using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared.Common;
using Shared.Events.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        
        config.ReceiveEndpoint(MessageQueueConst.PaymentService.PaymentSucceededEvent, e => {
            e.ConfigureConsumer<PaymentSucceededEventHandler>(context);
        });

        config.ReceiveEndpoint(MessageQueueConst.PaymentService.PaymentFailedEvent, e => {
            e.ConfigureConsumer<PaymentFailedEventHandler>(context);
        });

        config.ReceiveEndpoint(MessageQueueConst.StockService.StockNotReservedEvent, e => {
            e.ConfigureConsumer<StockNotReservedEventHandler>(context);
        });
    });

    x.AddConsumer<PaymentSucceededEventHandler>();
    x.AddConsumer<PaymentFailedEventHandler>();
    x.AddConsumer<StockNotReservedEventHandler>();

    EndpointConvention.Map<OrderCreatedEvent>(new Uri($"queue:{MessageQueueConst.OrderService.OrderCreatedEvent}"));
});

builder.Services.AddTransient<AppDbContext>();
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
