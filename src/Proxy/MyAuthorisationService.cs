using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Linn.Common.Configuration;

namespace Linn.ManufacturingEngineering.Proxy;

public class MyAuthorisationService : IMyAuthorisationService
{
    private readonly HttpClient httpClient;

    public MyAuthorisationService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public bool HasPermissionFor(string action, IEnumerable<string> privileges)
    {
        return Satisfies(action, privileges);
    }

    public async Task<bool> CheckUserHasPermissionToPerformAction(string action, string who)
    {
        var url = $"{ConfigurationManager.Configuration["PROXY_ROOT"]}/authorisation/permissions?who={who}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var privileges = doc.RootElement.EnumerateArray()
            .Select(e => e.GetProperty("privilege").GetString())
            .Where(p => !string.IsNullOrEmpty(p));
        return this.HasPermissionFor(action, privileges);
    }

    private static bool Satisfies(string privilegeRequired, IEnumerable<string> privileges)
    {
        return privileges != null && privileges.Contains(privilegeRequired);
    }
}