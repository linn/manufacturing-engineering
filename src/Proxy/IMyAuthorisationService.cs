using System.Collections.Generic;
using System.Threading.Tasks;

namespace Linn.Production2.Proxy;

public interface IMyAuthorisationService
{
    bool HasPermissionFor(string action, IEnumerable<string> privileges);

    Task<bool> CheckUserHasPermissionToPerformAction(string action, string who);
}
