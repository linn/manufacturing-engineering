namespace Linn.ManufacturingEngineering.Domain.LinnApps
{
    using Linn.Common.Email;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Pdf;

    public class ThingService : IThingService
    {
        private readonly IMessageDispatcher<Thing> dispatcher;

        private readonly IEmailService emailSender;

        private readonly IPdfService pdfService;

        private readonly ITemplateEngine templateEngine;

        public ThingService(
            IEmailService emailSender, 
            IPdfService pdfService, 
            ITemplateEngine templateEngine,
            IMessageDispatcher<Thing> dispatcher)
        {
            this.dispatcher = dispatcher;
            this.emailSender = emailSender;
            this.pdfService = pdfService;
            this.templateEngine = templateEngine;
        }

        public void SendThingMessage(string message)
        {
            this.dispatcher.Dispatch(new Thing { Name = "Some Data" });
        }

        public Thing CreateThing(Thing thing)
        {
            return thing;
        }
    }
}
