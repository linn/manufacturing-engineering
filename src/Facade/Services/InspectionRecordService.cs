namespace Linn.ManufacturingEngineering.Facade.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Linn.Common.Facade;
using Linn.Common.Persistence;
using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

public class InspectionRecordService : FacadeFilterResourceService<InspectionRecordHeader, int, InspectionRecordResource, InspectionRecordResource, InspectionRecordResource>
{
    private readonly IRepository<Employee, int> employeeRepository;

    private readonly IQueryRepository<PurchaseOrderLine> orderLineRepository;

    public InspectionRecordService(
        IRepository<InspectionRecordHeader, int> repository,
        ITransactionManager transactionManager,
        IBuilder<InspectionRecordHeader> builder,
        IRepository<Employee, int> employeeRepository,
        IQueryRepository<PurchaseOrderLine> orderLineRepository)
        : base(repository, transactionManager, builder)
    {
        this.employeeRepository = employeeRepository;
        this.orderLineRepository = orderLineRepository;
    }

    protected override InspectionRecordHeader CreateFromResource(InspectionRecordResource resource, IEnumerable<string> privileges = null)
    {
        var enteredBy = this.employeeRepository.FindById(resource.EnteredById);
        var orderLine = this.orderLineRepository.FindBy(
            x => x.OrderLine == resource.OrderLine && x.OrderNumber == resource.OrderNumber);
        var lines = resource.Lines.Select(
            x => new InspectionRecordLine
                     {
                         Material = x.Material,
                         Timestamp = !string.IsNullOrEmpty(x.Timestamp) ? DateTime.Parse(x.Timestamp) : null,
                         Status = x.Status,
                         HeaderId = x.HeaderId,
                         LineNumber = x.LineNumber,
                         Mottling = x.Mottling,
                         WhiteSpot = x.WhiteSpot,
                         Chipped = x.Chipped,
                         Marked = x.Marked,
                         Pitting = x.Pitting,
                         SentToReprocess = x.SentToReprocess
                     });
        return new InspectionRecordHeader
                   {
                       OrderLine = resource.OrderLine,
                       OrderNumber = resource.OrderNumber,
                       PurchaseOrderLine = orderLine,
                       PreprocessedBatch = resource.PreprocessedBatch,
                       DateOfEntry = DateTime.Now,
                       BatchSize = resource.BatchSize,
                       EnteredBy = enteredBy,
                       Lines = lines.ToList()
                   };
    }

    protected override void UpdateFromResource(
        InspectionRecordHeader entity,
        InspectionRecordResource updateResource,
        IEnumerable<string> privileges = null)
    {
        entity.BatchSize = updateResource.BatchSize;
        entity.PreprocessedBatch = updateResource.PreprocessedBatch;
        entity.Lines = updateResource.Lines.Select(
            x => new InspectionRecordLine
                     {
                         Material = x.Material,
                         Timestamp = !string.IsNullOrEmpty(x.Timestamp) ? DateTime.Parse(x.Timestamp) : null,
                         Status = x.Status,
                         HeaderId = x.HeaderId,
                         LineNumber = x.LineNumber,
                         Mottling = x.Mottling,
                         WhiteSpot = x.WhiteSpot,
                         Chipped = x.Chipped,
                         Marked = x.Marked,
                         Pitting = x.Pitting,
                         SentToReprocess = x.SentToReprocess
                     }).ToList();
    }

    protected override Expression<Func<InspectionRecordHeader, bool>> SearchExpression(string searchTerm)
    {
        throw new NotImplementedException();
    }

    protected override void SaveToLogTable(
        string actionType,
        int userNumber,
        InspectionRecordHeader entity,
        InspectionRecordResource resource,
        InspectionRecordResource updateResource)
    {
        throw new NotImplementedException();
    }

    protected override void DeleteOrObsoleteResource(InspectionRecordHeader entity, IEnumerable<string> privileges = null)
    {
        throw new NotImplementedException();
    }

    protected override Expression<Func<InspectionRecordHeader, bool>> FilterExpression(InspectionRecordResource searchResource)
    {
        throw new NotImplementedException();
    }

    protected override Expression<Func<InspectionRecordHeader, bool>> FindExpression(InspectionRecordResource searchResource)
    {
        throw new NotImplementedException();
    }
}
