namespace Linn.ManufacturingEngineering.IoC
{
    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Configuration;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Messaging.Handlers;
    using Linn.ManufacturingEngineering.Messaging.Messages;

    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client.Events;

    public static class MessagingExtensions
    {
        public static IServiceCollection AddRabbitConfiguration(this IServiceCollection services)
        {
            // all the routing keys the Listener cares about need to be registered here:
            var routingKeys = new[] { ThingMessage.RoutingKey };

            return services.AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("template", routingKeys))
                .AddSingleton(d => new EventingBasicConsumer(d.GetService<ChannelConfiguration>()?.ConsumerChannel));
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            // register handlers for different message types
            return services.AddSingleton<Handler<ThingMessage>, ThingMessageHandler>();
        }

        public static IServiceCollection AddMessageDispatchers(this IServiceCollection services)
        {
            // register dispatchers for different message types:
            return services.AddTransient<IMessageDispatcher<Thing>>(
            x => new RabbitMessageDispatcher<Thing>(
                x.GetService<ChannelConfiguration>(), x.GetService<ILog>(), ThingMessage.RoutingKey));
        }
    }
}
