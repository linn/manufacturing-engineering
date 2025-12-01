namespace Linn.ManufacturingEngineering.Integration.Tests.InspectionsModuleTests;

using System;
using System.Linq.Expressions;
using System.Net;

using FluentAssertions;

using Linn.ManufacturingEngineering.Domain.LinnApps;
using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
using Linn.ManufacturingEngineering.Resources;

using NSubstitute;

using NUnit.Framework;

public class WhenLookingUpPurchaseOrder : ContextBase
{
    private int orderNumber;

    private int lineNumber;

    [SetUp]
    public void SetUp()
    {
        this.orderNumber = 123;
        this.lineNumber = 1;

        this.PurchaseOrderLineRepository.FindByAsync(Arg.Any<Expression<Func<PurchaseOrderLine, bool>>>())
            .Returns(new PurchaseOrderLine
                         {
                             OrderNumber = this.orderNumber,
                             OrderLine = this.lineNumber,
                             Part = new Part { PartNumber = "PART" }
                         });

        this.Response = this.Client.Get(
            $"/manufacturing-engineering/purchase-orders?orderNumber={this.orderNumber}&lineNumber={this.lineNumber}",
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
        var resource = this.Response.DeserializeBody<PurchaseOrderLineResource>();
        resource.Should().NotBeNull();
        resource.OrderNumber.Should().Be(this.orderNumber);
    }
}
