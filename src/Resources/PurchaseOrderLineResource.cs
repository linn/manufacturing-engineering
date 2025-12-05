namespace Linn.Production2.Resources;

public class PurchaseOrderLineResource
{
    public int OrderNumber { get; set; }

    public int OrderLine { get; set; }

    public string PartNumber { get; set; }

    public string PartDescription { get; set; }

    public decimal Qty { get; set; }
}
