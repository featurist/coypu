using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_building_urls
    {
        private ConfiguredHostUrlBuilder configuredHostUrlBuilder;

        [SetUp]
        public void SetUp()
        {
            Configuration.Reset();
            configuredHostUrlBuilder = new ConfiguredHostUrlBuilder();
        }

        [TearDown]
        public void TearDown()
        {
            Configuration.Reset();
        }

        [Test]
        public void It_forms_url_from_host_port_and_virtual_path()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 81;
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("http://im.theho.st:81/visit/me"));
        }

        [Test]
        public void It_defaults_to_localhost()
        {
            Configuration.Port = 81;
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("http://localhost:81/visit/me"));
        }

        [Test]
        public void It_defaults_to_port_80()
        {
            Configuration.AppHost = "im.theho.st";
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_trailing_slashes_in_host()
        {
            Configuration.AppHost = "im.theho.st/";
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_missing_leading_slashes_in_virtual_path()
        {
            Configuration.AppHost = "im.theho.st";
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("visit/me"), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_trailing_and_missing_leading_slashes_with_a_port()
        {
            Configuration.AppHost = "im.theho.st/";
            Configuration.Port = 123;
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("visit/me"), Is.EqualTo("http://im.theho.st:123/visit/me"));
        }

        [Test]
        public void It_supports_SSL()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.SSL = true;
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("https://im.theho.st/visit/me"));
        }

        [Test]
        public void It_supports_SSL_with_ports()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 321;
            Configuration.SSL = true;
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("/visit/me"), Is.EqualTo("https://im.theho.st:321/visit/me"));
        }

        [Test]
        public void It_ignores_host_when_supplied_a_fully_qualified_url() {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 321;
            Configuration.SSL = true;

            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here"), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here"), Is.EqualTo("file:///C:/local/file.here"));
        }

        [Test]
        public void It_ignores_port_when_supplied_a_fully_qualified_url() {
            Configuration.Port = 321;

            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here"), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(configuredHostUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here"), Is.EqualTo("file:///C:/local/file.here"));
        }
    }
}