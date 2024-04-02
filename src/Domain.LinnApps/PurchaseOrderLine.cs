namespace Linn.ManufacturingEngineering.Domain.LinnApps;

using System.Collections.Generic;

public class PurchaseOrderLine
{
    public int OrderNumber { get; set; }

    public int OrderLine { get; set; }

    public Part Part { get; set; }

    public decimal Qty { get; set; }

    public IEnumerable<InspectionRecordHeader> InspectionRecords { get; set; }
}
