using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_visiting : BrowserInteractionTests
    {
        [Test]
        public void It_passes_message_directly_to_the_driver()
        {
            session.Visit("http://visit.me");

            Assert.That(driver.Visits.SingleOrDefault(), Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            Configuration.AppHost = default(string);
            Configuration.Port = default(int);
            Configuration.SSL = default(bool);
        }

        [Test]
        public void It_forms_url_from_host_port_and_virtual_path()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 81;

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st:81/visit/me"));
        }

        [Test]
        public void It_defaults_to_localhost()
        {
            Configuration.Port = 81;

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://localhost:81/visit/me"));
        }

        [Test]
        public void It_defaults_to_port_80()
        {
            Configuration.AppHost = "im.theho.st";

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_trailing_slashes_in_host()
        {
            Configuration.AppHost = "im.theho.st/";

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_missing_leading_slashes_in_virtual_path()
        {
            Configuration.AppHost = "im.theho.st";

            session.Visit("visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
        }

        [Test]
        public void It_handles_trailing_and_missing_leading_slashes_with_a_port()
        {
            Configuration.AppHost = "im.theho.st/";
            Configuration.Port = 123;

            session.Visit("visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st:123/visit/me"));
        }

        [Test]
        public void It_supports_SSL()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.SSL = true;

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("https://im.theho.st/visit/me"));
        }
        [Test]
        public void It_supports_SSL_with_ports()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 321;
            Configuration.SSL = true;

            session.Visit("/visit/me");

            Assert.That(driver.Visits.Single(), Is.EqualTo("https://im.theho.st:321/visit/me"));
        }

        [Test]
        public void It_ignores_host_etc_when_supplied_a_fully_qualified_url()
        {
            Configuration.AppHost = "im.theho.st";
            Configuration.Port = 321;
            Configuration.SSL = true;

            session.Visit("http://www.someother.site/over.here");
            Assert.That(driver.Visits.Last(), Is.EqualTo("http://www.someother.site/over.here"));

            session.Visit("file:///C:/local/file.here");
            Assert.That(driver.Visits.Last(), Is.EqualTo("file:///C:/local/file.here"));
        }
    }
}