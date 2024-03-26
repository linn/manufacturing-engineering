namespace Linn.ManufacturingEngineering.Integration.Tests.ThingModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
    using Linn.ManufacturingEngineering.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPuttingThing : ContextBase
    {
        private ThingResource resource;

        private int thingId;

        [SetUp]
        public void SetUp()
        {
            this.thingId = 123;

            this.resource = new ThingResource { Id = this.thingId, Name = "new name" };

            this.ThingRepository
                .FindById(this.thingId).Returns(new Thing { Id = this.thingId, Name = "new name" });

            this.Response = this.Client.PutAsJsonAsync(
                $"/template/things/{this.thingId}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
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
            var resultResource = this.Response.DeserializeBody<ThingResource>();
            resultResource.Should().NotBeNull();

            resultResource.Name.Should().Be("new name");
        }
    }
}
