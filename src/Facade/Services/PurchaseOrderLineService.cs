namespace Linn.ManufacturingEngineering.Facade.Services;

using System;
using System.Linq.Expressions;

using Linn.Common.Facade;
using Linn.Common.Persistence;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

public class PurchaseOrderLineService : QueryFacadeResourceService<PurchaseOrderLine, PurchaseOrderLineResource, PurchaseOrderLineResource>
{
    public PurchaseOrderLineService(IQueryRepository<PurchaseOrderLine> repository, IBuilder<PurchaseOrderLine> resourceBuilder)
        : base(repository, resourceBuilder)
    {
    }

    protected override Expression<Func<PurchaseOrderLine, bool>> SearchExpression(string searchTerm)
    {
        throw new NotImplementedException();
    }

    protected override Expression<Func<PurchaseOrderLine, bool>> FilterExpression(PurchaseOrderLineResource searchResource)
    {
        throw new NotImplementedException();
    }

    protected override Expression<Func<PurchaseOrderLine, bool>> FindExpression(PurchaseOrderLineResource searchResource)
    {
        return x => x.OrderNumber == searchResource.OrderNumber && x.OrderLine == searchResource.OrderLine;
    }
}
