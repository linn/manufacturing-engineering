using Linn.Production2.IoC;
using Linn.Production2.Messaging.Host.Jobs;

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
