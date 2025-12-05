namespace Linn.Production2.IoC
{
    using Linn.Production2.Proxy;
    using Linn.Production2.Domain.LinnApps;
    using Linn.Production2.Facade.ResourceBuilders;
    using Linn.Production2.Facade.Services;
    using Linn.Production2.Resources;
    using Microsoft.Extensions.DependencyInjection;
    using RazorEngineCore;
    using Linn.Common.Facade;
    using Linn.Common.Rendering;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddFacade(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IMyAuthorisationService, MyAuthorisationService>();
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
