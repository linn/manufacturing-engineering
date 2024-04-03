namespace Linn.ManufacturingEngineering.Resources;

public class InspectionRecordLineResource
{
    public string Material { get; set; }

    public string Timestamp { get; set; }

    public string Status { get; set; }

    public int HeaderId { get; set; }

    public int LineNumber { get; set; }

    public string Mottling { get; set; }

    public string WhiteSpot { get; set; }

    public string Chipped { get; set; }

    public string Marked { get; set; }

    public string Pitting { get; set; }

    public string SentToReprocess { get; set; }
}
