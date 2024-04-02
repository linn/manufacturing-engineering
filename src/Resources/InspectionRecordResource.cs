namespace Linn.ManufacturingEngineering.Resources;

using System.Collections.Generic;

public class InspectionRecordResource
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
}
