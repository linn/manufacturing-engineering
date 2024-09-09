namespace Linn.ManufacturingEngineering.Resources;

using System.Collections.Generic;

using Linn.Common.Resources;

public class InspectionRecordResource : HypermediaResource
{
    public int OrderNumber { get; set; }

    public int OrderLine { get; set; }

    public string PreprocessedBatch { get; set; }

    public string DateOfEntry { get; set; }

    public decimal BatchSize { get; set; }

    public IEnumerable<InspectionRecordLineResource> Lines { get; set; }

    public int Id { get; set; }

    public string EnteredByName { get; set; }

    public int EnteredById { get; set; }

    public string PartNumber { get; set; }

    public string PartDescription { get; set; }

    public decimal? OrderQty { get; set; }

    public string PassPercentage { get; set; }

    public int? SupplierId { get; set; }

    public string SupplierName { get; set; }
}
