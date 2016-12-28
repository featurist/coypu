using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_building_urls
    {
        [SetUp]
        public void SetUp()
        {
            SessionConfiguration = new SessionConfiguration();
            fullyQualifiedUrlBuilder = new FullyQualifiedUrlBuilder();
        }

        private SessionConfiguration SessionConfiguration;
        private FullyQualifiedUrlBuilder fullyQualifiedUrlBuilder;

        [Fact]
        public void It_defaults_to_localhost()
        {
            SessionConfiguration.Port = 81;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me",SessionConfiguration),
                        Is.EqualTo("http://localhost:81/visit/me"));
        }

        [Fact]
        public void It_defaults_to_port_80()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me",SessionConfiguration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Fact]
        public void It_forms_url_from_host_port_and_virtual_path()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 81;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", SessionConfiguration),
                        Is.EqualTo("http://im.theho.st:81/visit/me"));
        }

        [Fact]
        public void It_handles_missing_leading_slashes_in_virtual_path()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Fact]
        public void It_handles_basic_auth_provided_in_the_host()
        {
            SessionConfiguration.AppHost = "http://someone:example@im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("http://someone:example@im.theho.st/visit/me"));
        }

        [Fact]
        public void It_handles_protocol_provided_in_the_host()
        {
            SessionConfiguration.AppHost = "http://im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Fact]
        public void It_handles_ssl_protocol_provided_in_the_host()
        {
            SessionConfiguration.AppHost = "https://im.theho.st";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("https://im.theho.st/visit/me"));
        }

        [Fact]
        public void SSL_overrides_protocol_provided_in_the_host()
        {
            SessionConfiguration.AppHost = "http://im.theho.st";
            SessionConfiguration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("https://im.theho.st/visit/me"));
        }

        [Fact]
        public void It_handles_trailing_and_missing_leading_slashes_with_a_port()
        {
            SessionConfiguration.AppHost = "im.theho.st/";
            SessionConfiguration.Port = 123;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("visit/me", SessionConfiguration),
                        Is.EqualTo("http://im.theho.st:123/visit/me"));
        }

        [Fact]
        public void It_handles_trailing_slashes_in_host()
        {
            SessionConfiguration.AppHost = "im.theho.st/";
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", SessionConfiguration),
                        Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Fact]
        public void It_ignores_host_etc_when_supplied_a_fully_qualified_url()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 321;
            SessionConfiguration.SSL = true;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", SessionConfiguration),
                        Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", SessionConfiguration),
                        Is.EqualTo("file:///C:/local/file.here"));
        }

        [Fact]
        public void It_encodes_the_fully_qualified_url_when_supplied_an_unencoded_relative_url()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 321;
            SessionConfiguration.SSL = true;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/over.here?c=p|pe", SessionConfiguration),
                        Is.EqualTo("https://im.theho.st:321/over.here?c=p%7Cpe"));
        }

        [Fact]
        public void It_encodes_the_fully_qualified_url_when_supplied_an_unencoded_fully_qualified_url()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 321;
            SessionConfiguration.SSL = true;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here?c=p|pe", SessionConfiguration),
                        Is.EqualTo("http://www.someother.site/over.here?c=p%7Cpe"));
        }

        [Fact]
        public void It_supports_SSL()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", SessionConfiguration),
                        Is.EqualTo("https://im.theho.st/visit/me"));
        }
        
        [Fact]
        public void It_supports_SSL_with_ports()
        {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 321;
            SessionConfiguration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("/visit/me", SessionConfiguration), Is.EqualTo("https://im.theho.st:321/visit/me"));
        }

        [Fact]
        public void It_ignores_host_when_supplied_a_fully_qualified_url() {
            SessionConfiguration.AppHost = "im.theho.st";
            SessionConfiguration.Port = 321;
            SessionConfiguration.SSL = true;
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", SessionConfiguration), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", SessionConfiguration), Is.EqualTo("file:///C:/local/file.here"));
        }

        [Fact]
        public void It_ignores_port_when_supplied_a_fully_qualified_url() {
            SessionConfiguration.Port = 321;

            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("http://www.someother.site/over.here", SessionConfiguration), Is.EqualTo("http://www.someother.site/over.here"));
            Assert.That(fullyQualifiedUrlBuilder.GetFullyQualifiedUrl("file:///C:/local/file.here", SessionConfiguration), Is.EqualTo("file:///C:/local/file.here"));
        }
    }
}