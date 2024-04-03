namespace Linn.ManufacturingEngineering.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Service.Core.Handlers;
    using Linn.ManufacturingEngineering.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddTransient<IHandler, JsonResultHandler<PurchaseOrderLineResource>>()
                .AddTransient<IHandler, JsonResultHandler<InspectionRecordResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<InspectionRecordResource>>>();
        }
    }
}
