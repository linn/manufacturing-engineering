namespace Linn.ManufacturingEngineering.Persistence.LinnApps.Repositories;

using System;
using System.Linq;
using System.Linq.Expressions;

using Linn.Common.Persistence;
using Linn.ManufacturingEngineering.Domain.LinnApps;

using Microsoft.EntityFrameworkCore;

public class PurchaseOrderLineRepository : IQueryRepository<PurchaseOrderLine>
{
    private readonly ServiceDbContext serviceDbContext;

    public PurchaseOrderLineRepository(ServiceDbContext serviceDbContext)
    {
        this.serviceDbContext = serviceDbContext;
    }

    public PurchaseOrderLine FindBy(Expression<Func<PurchaseOrderLine, bool>> expression)
    {
        return this.serviceDbContext.PurchaseOrderLines.Include(x => x.Part)
            .Include(x => x.Order).FirstOrDefault(expression);
    }

    public IQueryable<PurchaseOrderLine> FilterBy(Expression<Func<PurchaseOrderLine, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<PurchaseOrderLine> FindAll()
    {
        throw new NotImplementedException();
    }
}
