namespace Linn.ManufacturingEngineering.Domain.LinnApps;

using System;
using System.Collections.Generic;

public class InspectionRecordLine
{
    public string Material { get; set; }

    public DateTime Timestamp { get; set; }

    public string Status { get; set; }

    public IEnumerable<string> FailureModes { get; set; }

    public int HeaderId { get; set; }

    public int LineNumber { get; set; }
}
