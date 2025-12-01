namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System.Net.Http;

using Linn.Common.Facade;
using Linn.Common.Persistence;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Facade.ResourceBuilders;
using Linn.ManufacturingEngineering.Facade.Services;
using Linn.ManufacturingEngineering.IoC;
using Linn.ManufacturingEngineering.Resources;
using Linn.ManufacturingEngineering.Service.Modules;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using NUnit.Framework;

public class ContextBase
{
    protected HttpClient Client { get; set; }

    protected HttpResponseMessage Response { get; set; }

    protected ITransactionManager TransactionManager { get; set; }

    protected IPurchaseOrderLineService PurchaseOrderLineService { get; private set; }

    protected IAsyncFacadeService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource> InspectionRecordsService
    {
        get;
        private set;
    }

    protected IRepository<Employee, int> EmployeeRepository { get; private set; }

    protected IQueryRepository<PurchaseOrderLine> PurchaseOrderLineRepository { get; private set; }

    protected IRepository<InspectionRecordHeader, int> InspectionRecordHeaderRepisitory { get; private set; }

    [SetUp]
    public void SetUpContext()
    {
        this.TransactionManager = Substitute.For<ITransactionManager>();
        this.PurchaseOrderLineRepository = Substitute.For<IQueryRepository<PurchaseOrderLine>>();
        this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
        this.InspectionRecordHeaderRepisitory = Substitute.For<IRepository<InspectionRecordHeader, int>>();
       
        this.PurchaseOrderLineService = new PurchaseOrderLineService(
            this.PurchaseOrderLineRepository,
            new PurchaseOrderLineResourceBuilder());

        this.InspectionRecordsService = new InspectionRecordService(
            this.InspectionRecordHeaderRepisitory,
            this.TransactionManager,
            new InspectionRecordResourceBuilder(),
            this.EmployeeRepository,
            this.PurchaseOrderLineRepository);


        this.Client = TestClient.With<InspectionsModule>(
            services =>
                {
                    services.AddSingleton(this.TransactionManager);
                    services.AddSingleton(this.PurchaseOrderLineService);
                    services.AddSingleton(this.InspectionRecordsService);
                    services.AddHandlers();
                    services.AddRouting();
                },
            FakeAuthMiddleware.EmployeeMiddleware);
    }
}
