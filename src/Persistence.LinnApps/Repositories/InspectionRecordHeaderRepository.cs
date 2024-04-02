using Linn.Common.Persistence.EntityFramework;

namespace Linn.ManufacturingEngineering.Persistence.LinnApps.Repositories;

using System.Linq;

using Linn.ManufacturingEngineering.Domain.LinnApps;

using Microsoft.EntityFrameworkCore;

public class InspectionRecordHeaderRepository : EntityFrameworkRepository<InspectionRecordHeader, int>
{
    private readonly ServiceDbContext serviceDbContext;

    public InspectionRecordHeaderRepository(ServiceDbContext serviceDbContext)
        : base(serviceDbContext.InspectionRecords)
    {
        this.serviceDbContext = serviceDbContext;
    }

    public override InspectionRecordHeader FindById(int key)
    {
        return this.serviceDbContext
            .InspectionRecords
            .Include(b => b.Id)
            .Include(a => a.EnteredBy)
            .Include(b => b.PurchaseOrderLine)
            .SingleOrDefault(a => a.Id == key);
    }
}
