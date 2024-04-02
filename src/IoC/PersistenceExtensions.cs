namespace Linn.ManufacturingEngineering.IoC
{
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Persistence.LinnApps;
    using Linn.ManufacturingEngineering.Persistence.LinnApps.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services.AddScoped<ServiceDbContext>()
                .AddTransient<DbContext>(a => a.GetService<ServiceDbContext>())
                .AddTransient<ITransactionManager, TransactionManager>()
                .AddTransient<IQueryRepository<PurchaseOrderLine>, PurchaseOrderLineRepository>()
                .AddTransient<IRepository<Employee, int>, EntityFrameworkRepository<Employee, int>>(
                    r => new EntityFrameworkRepository<Employee, int>(r.GetService<ServiceDbContext>()?.Employees))
                .AddTransient<IRepository<InspectionRecordHeader, int>, InspectionRecordHeaderRepository);
        }
    }
}
