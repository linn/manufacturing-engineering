﻿namespace Linn.ManufacturingEngineering.Facade.ResourceBuilders;

using System;
using System.Collections.Generic;
using System.Linq;

using Linn.Common.Facade;
using Linn.Common.Resources;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

public class InspectionRecordResourceBuilder : IBuilder<InspectionRecordHeader>
{
    public InspectionRecordResource Build(InspectionRecordHeader model, IEnumerable<string> claims)
    {
        string formattedPercentage = string.Empty;
        if (model.Lines?.Count > 0)
        {
            var passed = model.Lines.Count(x => x.Status == "PASSED");
            double percentage = (double)passed / model.Lines.Count * 100;
            formattedPercentage = percentage.ToString("0.0") + "%";
        }

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
                                                               Timestamp = l.Timestamp?.ToString("o"),
                                                               Status = l.Status,
                                                               HeaderId = l.HeaderId,
                                                               LineNumber = l.LineNumber,
                                                               Mottling = l.Mottling,
                                                               WhiteSpot = l.WhiteSpot,
                                                               Chipped = l.Chipped,
                                                               Marked = l.Marked,
                                                               Pitting = l.Pitting,
                                                               SentToReprocess = l.SentToReprocess
                                                           }).OrderBy(x => x.LineNumber),
                       Links = this.BuildLinks(model, null).ToArray(),
                       PassPercentage = formattedPercentage,
                       SupplierId = model.Order?.Supplier?.Id,
                       SupplierName = model.Order?.Supplier?.Name
        };
    }

    public string GetLocation(InspectionRecordHeader p)
    {
        return $"/manufacturing-engineering/inspections/{p.Id}";
    }

    private IEnumerable<LinkResource> BuildLinks(InspectionRecordHeader model, IEnumerable<string> claims)
    {
        yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
    }

    object IBuilder<InspectionRecordHeader>.Build(InspectionRecordHeader model, IEnumerable<string> claims) => this.Build(model, claims);
}
