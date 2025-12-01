using Linn.ManufacturingEngineering.IoC;
using Linn.ManufacturingEngineering.Messaging.Host.Jobs;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        {
            services.AddLog();
            services.AddCredentialsExtensions();
            services.AddServices();
            services.AddPersistence();
            services.AddRabbitConfiguration();
            services.AddMessageHandlers();
            services.AddHostedService<Listener>();
        })
    .Build();

await host.RunAsync();
