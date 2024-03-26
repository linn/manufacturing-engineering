namespace Linn.ManufacturingEngineering.Integration.Tests.ThingModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.ManufacturingEngineering.Domain.LinnApps;
    using Linn.ManufacturingEngineering.Integration.Tests.Extensions;
    using Linn.ManufacturingEngineering.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPostingThing : ContextBase
    {
        private Thing thing;

        private ThingResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new ThingResource
                                {
                                    Id = 123,
                                    Name = "new",
                                    Code = new ThingCodeResource(),
                                    Details = new List<ThingDetailResource>()
                                };
            this.thing = new Thing { Id = 123, Name = "new" };

            this.ThingService.CreateThing(Arg.Any<Thing>()).Returns(this.thing);

            this.Response = this.Client.PostAsJsonAsync(
                "/template/things",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ThingResource>();
            resource.Should().NotBeNull();

            resource.Id.Should().Be(123);
            resource.Name.Should().Be("new");
            resource.Links.First().Rel.Should().Be("self");
        }
    }
}
