using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Worker;
using SagaStateMachine.Worker.Models;
using Shared.Common;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddMassTransit(cfg => {
            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(
                opt => {
                    opt.AddDbContext<DbContext, OrderStateDbContext>((_, options) =>
                    {
                        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), m => {
                            m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                        });
                    });
                }
            );

            cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configuration => {

                configuration.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

                configuration.ReceiveEndpoint(MessageQueueConst.OrderService.OrderCreatedEvent, e => {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });

            }));


        });

        //services.AddMassTransitHostedService();

        //services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
