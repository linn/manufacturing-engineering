namespace Linn.ManufacturingEngineering.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.ManufacturingEngineering.Resources;

    public interface IPurchaseOrderLineService
    {
        Task<IResult<PurchaseOrderLineResource>> GetLine(PurchaseOrderLineResource requestResource);
    }
}
