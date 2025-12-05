namespace Linn.Production2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Production2.Resources;

    public interface IPurchaseOrderLineService
    {
        Task<IResult<PurchaseOrderLineResource>> GetLine(PurchaseOrderLineResource requestResource);
    }
}
