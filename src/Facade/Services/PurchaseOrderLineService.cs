namespace Linn.ManufacturingEngineering.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Resources;

    public class PurchaseOrderLineService : IPurchaseOrderLineService
    {
        private readonly IQueryRepository<PurchaseOrderLine> repository;

        private readonly IBuilder<PurchaseOrderLine> resourceBuilder;

        public PurchaseOrderLineService(
            IQueryRepository<PurchaseOrderLine> repository,
            IBuilder<PurchaseOrderLine> resourceBuilder)
        {
            this.repository = repository;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<PurchaseOrderLineResource>> GetLine(PurchaseOrderLineResource requestResource)
        {
            var line = await this.repository.FindByAsync(
                p => p.OrderNumber == requestResource.OrderNumber
                     && p.OrderLine == requestResource.OrderLine);

            return new SuccessResult<PurchaseOrderLineResource>((PurchaseOrderLineResource)this.resourceBuilder.Build(line, null));
        }
    }
}
