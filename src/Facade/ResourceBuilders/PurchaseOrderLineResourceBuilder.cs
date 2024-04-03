namespace Linn.ManufacturingEngineering.Facade.ResourceBuilders;

using System;
using System.Collections.Generic;

using Linn.Common.Facade;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

public class PurchaseOrderLineResourceBuilder : IBuilder<PurchaseOrderLine>
{
    public PurchaseOrderLineResource Build(PurchaseOrderLine model, IEnumerable<string> claims)
    {
        return new PurchaseOrderLineResource
                   {
                      OrderNumber = model.OrderNumber,
                      OrderLine = model.OrderLine,
                      PartDescription = model.Part.Description,
                      PartNumber = model.Part.PartNumber,
                      Qty = model.Qty
                   };
    }

    public string GetLocation(PurchaseOrderLine p)
    {
        throw new NotImplementedException();
    }

    object IBuilder<PurchaseOrderLine>.Build(PurchaseOrderLine model, IEnumerable<string> claims) => this.Build(model, claims);
}
