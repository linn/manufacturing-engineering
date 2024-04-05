namespace Linn.ManufacturingEngineering.Persistence.LinnApps.Repositories;

using System.Linq;
using Linn.Common.Persistence.EntityFramework;
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
            .Include(a => a.Lines)
            .Include(a => a.EnteredBy)
            .Include(b => b.PurchaseOrderLine).ThenInclude(l => l.Part)
            .SingleOrDefault(a => a.Id == key);
    }

    public override IQueryable<InspectionRecordHeader> FindAll()
    {
        return this.serviceDbContext.InspectionRecords
            .Include(a => a.Lines)
            .Include(a => a.EnteredBy)
            .Include(b => b.PurchaseOrderLine)
            .ThenInclude(l => l.Part)
            .OrderByDescending(x => x.Id);
    }
}
