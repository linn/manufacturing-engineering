namespace Linn.ManufacturingEngineering.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Messaging.Messages;

    using Microsoft.Extensions.DependencyInjection;


    using Newtonsoft.Json;

    public class ThingMessageHandler : Handler<ThingMessage>
    {
        private readonly IServiceProvider serviceProvider;

        public ThingMessageHandler(ILog logger, IServiceProvider serviceProvider)
            : base(logger)
        {
            this.serviceProvider = serviceProvider;
        }

        public override bool Handle(ThingMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);

            // create a service scope in the context of handling this message
            using var scope = this.serviceProvider.CreateScope();

            // obtain required scoped services like so:
            var dependency = scope.ServiceProvider.GetRequiredService<IThingService>();
            // they will be disposed along with the scope when this Handle() function returns

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<Thing>(enc);
                this.Logger.Info("Data: " + resource.Name);

                return true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message);
                return false;
            }
        }
    }
}
