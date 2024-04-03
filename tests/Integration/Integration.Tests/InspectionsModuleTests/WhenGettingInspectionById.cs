namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System;
using System.Collections.Generic;
using System.Net;

using FluentAssertions;

using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
using Linn.ManufacturingEngineering.Resources;

using NSubstitute;
using NUnit.Framework;

public class WhenGettingInspectionById : ContextBase
{
    [SetUp]
    public void SetUp()
    {
        this.InspectionRecordHeaderRepisitory.FindById(1)
            .Returns(new InspectionRecordHeader
                         {
                            Id = 1,
                            EnteredBy = new Employee { Id = 123 },
                            PurchaseOrderLine = new PurchaseOrderLine { Part = new Part { PartNumber = "PART" } },
                            Lines = new List<InspectionRecordLine> { new InspectionRecordLine
                                                                         {
                                                                             Timestamp = DateTime.Now
                                                                         }
                                                                       }
                         });

        this.Response = this.Client.Get(
            "/manufacturing-engineering/inspections/1",
            with =>
                {
                    with.Accept("application/json");
                }).Result;
    }

    [Test]
    public void ShouldReturnOk()
    {
        this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
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
        resource.Id.Should().Be(1);
    }
}
