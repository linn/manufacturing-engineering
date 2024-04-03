namespace Linn.ManufacturingEngineering.Domain.LinnApps;

using System;
using System.Collections.Generic;

public class InspectionRecordHeader
{
    public PurchaseOrderLine PurchaseOrderLine { get; set; }

    public int OrderNumber { get; set; }

    public int OrderLine { get; set; }

    public string PreprocessedBatch { get; set; }

    public DateTime DateOfEntry { get; set; }

    public decimal BatchSize { get; set; }

    public ICollection<InspectionRecordLine> Lines { get; set; }

    public int Id { get; set; }

    public Employee EnteredBy { get; set; }
}

