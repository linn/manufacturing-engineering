namespace Linn.ManufacturingEngineering.Service.Modules;

using System.Threading.Tasks;

using Linn.Common.Facade;
using Linn.Common.Service.Core;
using Linn.Common.Service.Core.Extensions;
using Linn.ManufacturingEngineering.Domain.LinnApps;
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
        endpoints.MapGet("/manufacturing-engineering/inspections", this.GetApp);
        endpoints.MapGet("/manufacturing-engineering/inspections/create", this.GetApp);
        endpoints.MapGet("/manufacturing-engineering/inspections/{id}", this.GetById);
        endpoints.MapPost("/manufacturing-engineering/inspections", this.PostInspectionRecord);
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
        IQueryFacadeResourceService<PurchaseOrderLine, PurchaseOrderLineResource, PurchaseOrderLineResource> service)
    {
        await res.Negotiate(service.FindBy(new PurchaseOrderLineResource
                                                 {
                                                     OrderLine = lineNumber,
                                                     OrderNumber = orderNumber
                                                 }));
    }

    private async Task PostInspectionRecord(
        HttpRequest req,
        HttpResponse res,
        InspectionRecordResource resource,
        IFacadeResourceFilterService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        var user = req.HttpContext.User.GetEmployeeNumber();
        resource.EnteredById = user;
        await res.Negotiate(service.Add(resource));
    }

    private async Task GetById(
        HttpRequest _,
        HttpResponse res,
        int id,
        IFacadeResourceFilterService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> service)
    {
        await res.Negotiate(service.GetById(id));
    }
}
