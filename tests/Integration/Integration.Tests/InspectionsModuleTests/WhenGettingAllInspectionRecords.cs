namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FluentAssertions;

using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
using Linn.ManufacturingEngineering.Resources;

using NSubstitute;

using NUnit.Framework;

public class WhenGettingAllInspectionRecords : ContextBase
{
    [SetUp]
    public void SetUp()
    {
        this.InspectionRecordHeaderRepisitory.FindAll()
            .Returns(
                new List<InspectionRecordHeader>
                    {
                        new InspectionRecordHeader
                            {
                                Id = 1,
                                EnteredBy = new Employee { Id = 123 },
                                PurchaseOrderLine = new PurchaseOrderLine { Part = new Part { PartNumber = "PART" } },
                                Lines = new List<InspectionRecordLine>
                                            {
                                                new InspectionRecordLine { Timestamp = DateTime.Now }
                                            }
                            }
                    }.AsQueryable());

        this.Response = this.Client.Get(
                "/manufacturing-engineering/inspections",
                with => { with.Accept("application/json"); })
            .Result;
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
        var resource = this.Response.DeserializeBody<IEnumerable<InspectionRecordResource>>();
        resource.Should().NotBeNull();
        resource.First().Id.Should().Be(1);
    }
}
