namespace Linn.Production2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Production2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderLineRepository : EntityFrameworkQueryRepository<PurchaseOrderLine>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderLineRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.PurchaseOrderLines)
        {
            this.serviceDbContext = serviceDbContext;
        }


        public override async Task<PurchaseOrderLine> FindByAsync(Expression<Func<PurchaseOrderLine, bool>> expression)
        {
            return await this.serviceDbContext.PurchaseOrderLines.Include(x => x.Part).Include(x => x.Order)
                       .FirstOrDefaultAsync(expression);
        }
    }
}
