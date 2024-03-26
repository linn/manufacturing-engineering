namespace Linn.ManufacturingEngineering.Integration.Tests.ThingModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
    using Linn.ManufacturingEngineering.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingThings : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.ThingRepository.FindAll().Returns(
                new List<Thing> { new Thing { Id = 1 }, new Thing { Id = 2 } }.AsQueryable());

            this.Response = this.Client.Get(
                "/template/things",
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
            var resources = this.Response.DeserializeBody<IEnumerable<ThingResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources?.First().Id.Should().Be(1);
            resources.Second().Id.Should().Be(2);
        }
    }
}
