namespace Linn.ManufacturingEngineering.Domain.LinnApps;

public class PurchaseOrderLine
{
    public int OrderNumber { get; set; }

    public int OrderLine { get; set; }

    public Part Part { get; set; }

    public decimal Qty { get; set; }

    public PurchaseOrder Order { get; set; }
}
