using System.Collections.Generic;
using System.Threading.Tasks;

namespace Linn.ManufacturingEngineering.Proxy;

public interface IMyAuthorisationService
{
    bool HasPermissionFor(string action, IEnumerable<string> privileges);

    Task<bool> CheckUserHasPermissionToPerformAction(string action, string who);
}
