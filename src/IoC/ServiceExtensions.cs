namespace Linn.ManufacturingEngineering.IoC
{
    using System.Net.Http;

    using Amazon.Extensions.NETCore.Setup;
    using Amazon.SimpleEmail;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Facade;
    using Linn.Common.Pdf;
    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Facade.ResourceBuilders;
    using Linn.ManufacturingEngineering.Facade.Services;
    using Linn.ManufacturingEngineering.Resources;

    using Microsoft.Extensions.DependencyInjection;

    using RazorEngineCore;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddFacade(this IServiceCollection services)
        {
            return services
                .AddTransient<IBuilder<Thing>, ThingResourceBuilder>()
                .AddTransient<IFacadeResourceService<Thing, int, ThingResource, ThingResource>, ThingFacadeService>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<IThingService, ThingService>()
                .AddTransient<IAmazonSimpleEmailService>(
                    x => new AmazonSimpleEmailServiceClient(x.GetService<AWSOptions>()?.Region))
                .AddTransient<IRazorEngine, RazorEngine>()

                .AddTransient<IEmailService>(x => new EmailService(x.GetService<IAmazonSimpleEmailService>()))
                
                .AddTransient<ITemplateEngine, RazorTemplateEngine>()

                .AddTransient<IPdfService>(
                    x => new PdfService(ConfigurationManager.Configuration["PDF_SERVICE_ROOT"], new HttpClient()));
        }
    }
}
