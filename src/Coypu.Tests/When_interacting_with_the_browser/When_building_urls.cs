using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_building_urls
    {
        [SetUp]
        public void SetUp()
        {
            configuration = Configuration.Default();
            fullyQualifiedUrlBuilder = new FullyQualifiedUrlBuilder();
        }

        private Configuration configuration;
        private FullyQualifiedUrlBuilder fullyQualifiedUrlBuilder;

        [Test]
        public void It_defaults_to_localhost()
        {
            configuration.Port = 81;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me",configuration),
                        Is.EqualTo("http://localhost:81/visit/me"));
        }

        [Test]
        public void It_defaults_to_port_80()
        {
            configuration.AppHost = "im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me",configuration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_forms_url_from_host_port_and_virtual_path()
        {
            configuration.AppHost = "im.theho.st";
            configuration.Port = 81;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", configuration),
                        Is.EqualTo("http://im.theho.st:81/visit/me"));
        }

        [Test]
        public void It_handles_missing_leading_slashes_in_virtual_path()
        {
            configuration.AppHost = "im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", configuration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_trailing_and_missing_leading_slashes_with_a_port()
        {
            configuration.AppHost = "im.theho.st/";
            configuration.Port = 123;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", configuration),
                        Is.EqualTo("http://im.theho.st:123/visit/me"));
        }

        [Test]
        public void It_handles_trailing_slashes_in_host()
        {
            configuration.AppHost = "im.theho.st/";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", configuration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_ignores_host_etc_when_supplied_a_fully_qualified_url()
        {
            configuration.AppHost = "im.theho.st";
            configuration.Port = 321;
            configuration.SSL = true;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", configuration),
                        Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", configuration),
                        Is.EqualTo("file:///C:/local/file.here"));
        }

        [Test]
        public void It_supports_SSL()
        {
            configuration.AppHost = "im.theho.st";
            configuration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", configuration),
                        Is.EqualTo("https://im.theho.st/visit/me"));
        }
        
        [Test]
        public void It_supports_SSL_with_ports()
        {
            configuration.AppHost = "im.theho.st";
            configuration.Port = 321;
            configuration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", configuration), Is.EqualTo("https://im.theho.st:321/visit/me"));
        }

        [Test]
        public void It_ignores_host_when_supplied_a_fully_qualified_url() {
            configuration.AppHost = "im.theho.st";
            configuration.Port = 321;
            configuration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", configuration), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", configuration), Is.EqualTo("file:///C:/local/file.here"));
        }

        [Test]
        public void It_ignores_port_when_supplied_a_fully_qualified_url() {
            configuration.Port = 321;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", configuration), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", configuration), Is.EqualTo("file:///C:/local/file.here"));
        }
    }
}