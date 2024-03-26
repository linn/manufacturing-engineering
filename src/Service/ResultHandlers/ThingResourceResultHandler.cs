namespace Linn.ManufacturingEngineering.Service.ResultHandlers
{
    using System;
    using System.Linq;

    using Linn.Common.Service.Core.Handlers;
    using Linn.ManufacturingEngineering.Resources;

    public class ThingResourceResultHandler : JsonResultHandler<ThingResource>
    {
        public override Func<ThingResource, string> GenerateLocation => r => r.Links.FirstOrDefault(l => l.Rel == "self")?.Href;
    }
}
