namespace Linn.ManufacturingEngineering.Integration.Tests.ThingModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
    using Linn.ManufacturingEngineering.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingThing : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.ThingRepository.FindById(1).Returns(new Thing { Id = 1 });

            this.Response = this.Client.Get(
                "/template/things/1",
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
            var resource = this.Response.DeserializeBody<ThingResource>();
            resource.Should().NotBeNull();
            resource.Id.Should().Be(1);
        }
    }
}
