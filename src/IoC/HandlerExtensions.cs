namespace Linn.ManufacturingEngineering.IoC
{
    using System.Collections.Generic;

    using Linn.Common.Service.Core;
    using Linn.Common.Service.Core.Handlers;
    using Linn.ManufacturingEngineering.Resources;

    using Microsoft.Extensions.DependencyInjection;

    public static class HandlerExtensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services
                .AddTransient<IHandler, JsonResultHandler<ThingResource>>()
                .AddTransient<IHandler, JsonResultHandler<IEnumerable<ThingResource>>>()
                .AddTransient<IHandler, JsonResultHandler<ProcessResultResource>>()
                .AddTransient<IHandler, CsvResultHandler<IEnumerable<ThingResource>>>()
                .AddTransient<IHandler, CsvResultHandler<ThingResource>>();
        }
    }
}
