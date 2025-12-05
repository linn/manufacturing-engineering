namespace Linn.Production2.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Service.Handlers;
    using Linn.Production2.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHandler, JsonResultHandler<PurchaseOrderLineResource>>()
                .AddSingleton<IHandler, JsonResultHandler<InspectionRecordResource>>()
                .AddSingleton<IHandler, JsonResultHandler<IEnumerable<InspectionRecordResource>>>();
        }
    }
}
