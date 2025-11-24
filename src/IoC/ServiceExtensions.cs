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
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IRazorEngine, RazorEngine>()
                .AddTransient<ITemplateEngine, RazorTemplateEngine>()
                .AddTransient<IPurchaseOrderLineService, PurchaseOrderLineService>()
                .AddTransient<IBuilder<PurchaseOrderLine>, PurchaseOrderLineResourceBuilder>()
                .AddTransient<IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource>, InspectionRecordService>()
                .AddTransient<IBuilder<InspectionRecordHeader>, InspectionRecordResourceBuilder>();
        }
    }
}
