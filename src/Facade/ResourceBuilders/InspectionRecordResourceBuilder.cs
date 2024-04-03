namespace Linn.ManufacturingEngineering.Facade.ResourceBuilders;

using System;
using System.Collections.Generic;
using System.Linq;

using Linn.Common.Facade;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

public class InspectionRecordResourceBuilder : IBuilder<InspectionRecordHeader>
{
    public InspectionRecordResource Build(InspectionRecordHeader model, IEnumerable<string> claims)
    {
        return new InspectionRecordResource
                   {
                       OrderNumber = model.PurchaseOrderLine.OrderNumber,
                       OrderLine = model.PurchaseOrderLine.OrderLine,
                       Id = model.Id,
                       BatchSize = model.BatchSize,
                       DateOfEntry = model.DateOfEntry.ToString("o"),
                       PreprocessedBatch = model.PreprocessedBatch,
                       EnteredByName = model.EnteredBy.Name,
                       PartNumber = model.PurchaseOrderLine.Part.PartNumber,
                       PartDescription = model.PurchaseOrderLine.Part.Description,
                       OrderQty = model.PurchaseOrderLine.Qty,
                       Lines = model.Lines.Select(l => new InspectionRecordLineResource
                                                           {
                                                               Material = l.Material,
                                                               Timestamp = l.Timestamp.ToString("o"),
                                                               Status = l.Status,
                                                               HeaderId = l.HeaderId,
                                                               LineNumber = l.LineNumber,
                                                               Mottling = l.Mottling,
                                                               WhiteSpot = l.WhiteSpot,
                                                               Chipped = l.Chipped,
                                                               Marked = l.Marked,
                                                               Pitting = l.Pitting
                                                           })
                   };
    }

    public string GetLocation(InspectionRecordHeader p)
    {
        throw new NotImplementedException();
    }

    object IBuilder<InspectionRecordHeader>.Build(InspectionRecordHeader model, IEnumerable<string> claims) => this.Build(model, claims);
}
