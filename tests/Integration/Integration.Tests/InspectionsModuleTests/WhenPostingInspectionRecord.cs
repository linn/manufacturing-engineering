namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Resources;

using NSubstitute;

using NUnit.Framework;

public class WhenPostingInspectionRecord : ContextBase
{
    private InspectionRecordResource resource;
    [SetUp]
    public void SetUp()
    {
        this.EmployeeRepository.FindById(33087).Returns(new Employee());
        this.PurchaseOrderLineRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderLine, bool>>>()).Returns(
            new PurchaseOrderLine { OrderLine = 1, OrderNumber = 123, Part = new Part() });
        this.resource = new InspectionRecordResource
                            {
                                BatchSize = 1,
                                EnteredById = 33087,
                                OrderLine = 1,
                                OrderNumber = 123,
                                PreprocessedBatch = "N",
                                Lines = new List<InspectionRecordLineResource>
                                            {
                                                new InspectionRecordLineResource
                                                    {
                                                        LineNumber = 1,
                                                        Timestamp = DateTime.Today.ToString("o"),
                                                        Chipped = "N",
                                                        Status = "PASSED",
                                                        WhiteSpot = "N",
                                                        Marked = "N",
                                                        Material = "MAT",
                                                        Mottling = "N",
                                                        Pitting = "N"
                                                    }
                                            }
                            };
        this.Response = this.Client.PostAsJsonAsync(
            $"/manufacturing-engineering/inspections",
            this.resource).Result;
    }

    [Test]
    public void ShouldReturnCreated()
    {
        this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public void ShouldCommit()
    {
        this.TransactionManager.Received(1).Commit();
    }
}
