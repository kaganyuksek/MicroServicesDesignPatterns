using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.API;
using Payment.API.Consumers;
using Payment.API.Models;
using Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseInMemoryDatabase("PaymentDb");
});

builder.Services.AddMassTransit(x => {
    x.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        config.ReceiveEndpoint(MessageQueueConst.StockService.StockReservedEvent, e => {
            e.ConfigureConsumer<StockReservedEventHandler>(context);
        });
    });

    x.AddConsumer<StockReservedEventHandler>();
});

builder.Services.AddTransient<AppDbContext>();
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.UseMemoryDbSeed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
