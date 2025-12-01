
namespace Linn.ManufacturingEngineering.Service.Modules;

using System.Threading.Tasks;

using Linn.Common.Facade;
using Linn.Common.Service;
using Linn.Common.Service.Extensions;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Facade.Services;
using Linn.ManufacturingEngineering.Proxy;
using Linn.ManufacturingEngineering.Resources;
using Linn.ManufacturingEngineering.Service.Extensions;
using Linn.ManufacturingEngineering.Service.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Org.BouncyCastle.Ocsp;

public class InspectionsModule : IModule
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/manufacturing-engineering/purchase-orders", this.GetOrderLine);
        endpoints.MapGet("/manufacturing-engineering/inspections", this.GetAll);
        endpoints.MapGet("/manufacturing-engineering/inspections/create", this.GetApp);
        endpoints.MapGet("/manufacturing-engineering/inspections/{id}", this.GetById);
        endpoints.MapPost("/manufacturing-engineering/inspections", this.PostInspectionRecord);
        endpoints.MapPut("/manufacturing-engineering/inspections/{id}", this.PutInspectionRecord);
    }

    private async Task GetApp(HttpRequest req, HttpResponse res)
    {
        await res.Negotiate(new ViewResponse { ViewName = "Index.cshtml" });
    }

    private async Task GetOrderLine(
        int orderNumber,
        int lineNumber,
        HttpRequest _,
        HttpResponse res,
        IPurchaseOrderLineService service)
    {
        await res.Negotiate(await service.GetLine(new PurchaseOrderLineResource
                                                 {
                                                     OrderLine = lineNumber,
                                                     OrderNumber = orderNumber
                                                 }));
    }

    private async Task PostInspectionRecord(
        HttpRequest req,
        HttpResponse res,
        InspectionRecordResource resource,
        IMyAuthorisationService authService,
        IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        var user = req.HttpContext.User.GetEmployeeNumber();

        // placeholder demo for show and tell 
        // need to assume client submits a freshly minted token
        // would require client code changes to ensure a new token is fetched before posting
        var privilegesFromToken = req.HttpContext.GetPrivileges();
        
        var permittedFromToken = authService.HasPermissionFor("inspections.create", privilegesFromToken);
        
        resource.EnteredById = user;
        await res.Negotiate(await service.Add(resource));
    }

    private async Task PutInspectionRecord(
        HttpRequest req,
        HttpResponse res,
        int id,
        IMyAuthorisationService authService,
        InspectionRecordResource resource,
        IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        var user = req.HttpContext.User.GetEmployeeNumber();
        resource.EnteredById = user;
        
        // placeholder demo for show and tell 
        // alternative, almost definitely better approach
        // disregard the access token - it could be old for all we know
        // fetch the live permissions from the authorisation API
        var hasPermissionRightNow 
            = await authService
                .CheckUserHasPermissionToPerformAction("inspections.update", req.HttpContext.User.GetEmployeeUrl());
        
        await res.Negotiate(await service.Update(id, resource, null));
    }

    private async Task GetById(
        HttpRequest _,
        HttpResponse res,
        int id,
        IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        await res.Negotiate(await service.GetById(id));
    }

    private async Task GetAll(
        HttpRequest _,
        HttpResponse res,
        IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        await res.Negotiate(await service.GetAll());
    }
}
