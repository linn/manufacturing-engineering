namespace Linn.ManufacturingEngineering.IoC
{
    using Microsoft.Extensions.DependencyInjection;

    public static class MessagingExtensions
    {
        public static IServiceCollection AddRabbitConfiguration(this IServiceCollection services)
        {
            return services;
            // all the routing keys the Listener cares about need to be registered here:
            // var routingKeys = new[] { ThingMessage.RoutingKey };
            //
            // return services.AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("template", routingKeys))
            //     .AddSingleton(d => new EventingBasicConsumer(d.GetService<ChannelConfiguration>()?.ConsumerChannel));
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            return services;
            // register handlers for different message types
            //return services.AddSingleton<Handler<ThingMessage>, ThingMessageHandler>();
        }

        public static IServiceCollection AddMessageDispatchers(this IServiceCollection services)
        {
            return services;
            // register dispatchers for different message types:
            // return services.AddTransient<IMessageDispatcher<Thing>>(
            // x => new RabbitMessageDispatcher<Thing>(
            //     x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), ThingMessage.RoutingKey));
        }
    }
}
