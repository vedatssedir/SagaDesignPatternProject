using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Models;
using Shared;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostServices,services) =>
    {

        services.AddMassTransit(cfg =>
        {

            cfg.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>().EntityFrameworkRepository(opt =>
            {
                opt.AddDbContext<DbContext,OrderStateDbContext>((provider, builder) =>
                {
                    builder.UseSqlServer(hostServices.Configuration.GetConnectionString("SqlCon"), m =>
                    {
                        m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    });
                });
            });


            cfg.AddBus(provider=>Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                configure.Host(hostServices.Configuration.GetConnectionString("RabbitMQ"));


                configure.ReceiveEndpoint(RabbitMqSettingsConst.OrderSaga, e =>
                {
                    e.ConfigureSaga<OrderStateInstance>(provider);
                });






            }));




        });



        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
