using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Stock.API.Consumers;
using Stock.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("StockDb");
});

builder.Services.AddMassTransit(x => {

    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        config.ReceiveEndpoint(MessageQueueConst.OrderService.OrderCreatedEvent, e => {
            e.ConfigureConsumer<OrderCreatedEventHandler>(context);
        });

        config.ReceiveEndpoint(MessageQueueConst.PaymentService.StockPaymentFailedEvent, e => {
            e.ConfigureConsumer<PaymentFailedEventHandler>(context);
        });
    });

    x.AddConsumer<OrderCreatedEventHandler>();
    x.AddConsumer<PaymentFailedEventHandler>();
});

builder.Services.AddTransient<AppDbContext>();
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Stocks.Add(new Stock.API.Models.Stock(1, 100));
    context.Stocks.Add(new Stock.API.Models.Stock(2, 20));

    context.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
