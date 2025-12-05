namespace Linn.Production2.Persistence.LinnApps.Repositories;

using System.Linq;
using System.Threading.Tasks;

using Linn.Common.Persistence.EntityFramework;
using Linn.Production2.Domain.LinnApps;
using Microsoft.EntityFrameworkCore;

public class InspectionRecordHeaderRepository : EntityFrameworkRepository<InspectionRecordHeader, int>
{
    private readonly ServiceDbContext serviceDbContext;

    public InspectionRecordHeaderRepository(ServiceDbContext serviceDbContext)
        : base(serviceDbContext.InspectionRecords)
    {
        this.serviceDbContext = serviceDbContext;
    }

    public override async Task <InspectionRecordHeader> FindByIdAsync(int key)
    {
        return await this.serviceDbContext
            .InspectionRecords
            .Include(a => a.Lines)
            .Include(a => a.EnteredBy)
            .Include(b => b.PurchaseOrderLine).ThenInclude(l => l.Part)
            .SingleOrDefaultAsync(a => a.Id == key);
    }

    public override IQueryable<InspectionRecordHeader> FindAll()
    {
        return this.serviceDbContext.InspectionRecords
            .Include(a => a.Lines)
            .Include(a => a.EnteredBy)
            .Include(b => b.PurchaseOrderLine)
            .ThenInclude(l => l.Part)
            .Include(b => b.Order).ThenInclude(x => x.Supplier)
            .AsNoTracking()
            .OrderByDescending(x => x.Id);
    }
}
