namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Json;

using FluentAssertions;
using FluentAssertions.Extensions;

using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
using Linn.ManufacturingEngineering.Resources;

using NSubstitute;

using NUnit.Framework;

public class WhenPuttingInspectionRecord : ContextBase
{
    private InspectionRecordResource resource;

    private InspectionRecordHeader record;

    [SetUp]
    public void SetUp()
    {
        this.resource = new InspectionRecordResource
        {
            Lines = new List<InspectionRecordLineResource>
                                          {
                                              new InspectionRecordLineResource
                                                  {
                                                      LineNumber = 1,
                                                      Chipped = "Y",
                                                      Marked = "Y",
                                                      Pitting = "Y",
                                                      SentToReprocess = "Y",
                                                      Material = "OTHER MAT",
                                                      Timestamp = 30.March(2024).ToString("O"),
                                                      Status = "FAILED",
                                                      HeaderId = 123,
                                                      Mottling = "Y",
                                                      WhiteSpot = "Y",
                                                  },
                                              new InspectionRecordLineResource
                                                  {
                                                      LineNumber = 2,
                                                      Chipped = "Y",
                                                      Marked = "Y",
                                                      Pitting = "Y",
                                                      SentToReprocess = "Y",
                                                      Material = "OTHER MAT",
                                                      Timestamp = 30.March(2024).ToString("O"),
                                                      Status = "FAILED",
                                                      HeaderId = 123,
                                                      Mottling = "Y",
                                                      WhiteSpot = "Y",
                                                  }
                                          },
            PreprocessedBatch = "Y",
            BatchSize = 2,
            Id = 123,
            DateOfEntry = 28.March(2024).ToString("O"),
            EnteredByName = "PERSON",
            OrderLine = 1,
            OrderNumber = 100000,
            PartNumber = "PART"
        };

        this.record = new InspectionRecordHeader
                          {
                              Lines = new List<InspectionRecordLine>
                                          {
                                              new InspectionRecordLine
                                                  {
                                                      LineNumber = 1,
                                                      Chipped = "N",
                                                      Marked = "N",
                                                      Pitting = "N",
                                                      SentToReprocess = "N",
                                                      Material = "MAT",
                                                      Timestamp = 28.March(2024),
                                                      Status = "PASSED",
                                                      HeaderId = 123,
                                                      Mottling = "N",
                                                      WhiteSpot = "N",
                                                  }
                                          },
                              PreprocessedBatch = "N",
                              BatchSize = 1,
                              Id = 123,
                              DateOfEntry = 28.March(2024),
                              EnteredBy = new Employee(),
                              OrderLine = 1,
                              OrderNumber = 100000,
                              PurchaseOrderLine = new PurchaseOrderLine
                                                      {
                                                          Part = new Part
                                                                     {
                                                                         PartNumber = "PART"
                                                                     }
                                                      }
                          };
        this.InspectionRecordHeaderRepisitory.FindById(123).Returns(this.record);
        this.EmployeeRepository.FindById(Arg.Any<int>()).Returns(new Employee());
        this.PurchaseOrderLineRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderLine, bool>>>()).Returns(
            new PurchaseOrderLine { OrderLine = 1, OrderNumber = 123, Part = new Part() });
        
        this.Response = this.Client.PutAsJsonAsync(
            $"/manufacturing-engineering/inspections/123",
            this.resource).Result;
    }

    [Test]
    public void ShouldReturnOk()
    {
        this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public void ShouldUpdate()
    {
        // check a new line was added
        this.record.Lines.Count.Should().Be(this.resource.Lines.Count());

        // check header fields were updated
        this.record.PreprocessedBatch.Should().Be(this.resource.PreprocessedBatch);
        this.record.BatchSize.Should().Be(this.resource.BatchSize);

        // check all the existing line updates were applied
        var firstUpdateLine = this.resource.Lines.First();
        this.record.Lines.First().LineNumber.Should().Be(firstUpdateLine.LineNumber);
        this.record.Lines.First().Chipped.Should().Be(firstUpdateLine.Chipped);
        this.record.Lines.First().Marked.Should().Be(firstUpdateLine.Marked);
        this.record.Lines.First().Pitting.Should().Be(firstUpdateLine.Pitting);
        this.record.Lines.First().WhiteSpot.Should().Be(firstUpdateLine.WhiteSpot);
        this.record.Lines.First().Mottling.Should().Be(firstUpdateLine.Mottling);
        this.record.Lines.First().SentToReprocess.Should().Be(firstUpdateLine.SentToReprocess);
        this.record.Lines.First().Status.Should().Be(firstUpdateLine.Status);
        this.record.Lines.First().Timestamp.Should().Be(30.March(2024));

        // check the new line got added with the correct values
        var secondUpdateLine = this.resource.Lines.Last();
        this.record.Lines.Last().LineNumber.Should().Be(secondUpdateLine.LineNumber);
        this.record.Lines.Last().Chipped.Should().Be(secondUpdateLine.Chipped);
        this.record.Lines.Last().Marked.Should().Be(secondUpdateLine.Marked);
        this.record.Lines.Last().Pitting.Should().Be(secondUpdateLine.Pitting);
        this.record.Lines.Last().WhiteSpot.Should().Be(secondUpdateLine.WhiteSpot);
        this.record.Lines.Last().Mottling.Should().Be(secondUpdateLine.Mottling);
        this.record.Lines.Last().SentToReprocess.Should().Be(secondUpdateLine.SentToReprocess);
        this.record.Lines.Last().Status.Should().Be(secondUpdateLine.Status);
        this.record.Lines.Last().Timestamp.Should().Be(30.March(2024));
    }

    [Test]
    public void ShouldCommit()
    {
        this.TransactionManager.Received(1).Commit();
    }

    [Test]
    public void ShouldReturnJsonContentType()
    {
        this.Response.Content.Headers.ContentType.Should().NotBeNull();
        this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
    }

    [Test]
    public void ShouldReturnJsonBody()
    {
        var resource = this.Response.DeserializeBody<InspectionRecordResource>();
        resource.Should().NotBeNull();
        resource.Id.Should().Be(123);
        resource.Lines.Count().Should().Be(2);
    }
}
