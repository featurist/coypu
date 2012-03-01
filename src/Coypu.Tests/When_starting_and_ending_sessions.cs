using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests
{
    [TestFixture]
    public class When_starting_and_ending_sessions
    {
        private Configuration configuration;

        [SetUp]
        public void SetUp()
        {
            configuration = Configuration.Default();
            configuration.Driver = typeof (FakeDriver);
        }

        [Test]
        public void Dispose_handles_a_disposed_session()
        {
            var browserSession = new BrowserSession(configuration);

            browserSession.Dispose();
            browserSession.Dispose();
        }

        [Test]
        public void After_disposing_the_session_a_new_session_is_available()
        {
            BrowserSession firstBrowserSession;
            using (var session = new BrowserSession(configuration))
            {
                firstBrowserSession = session;
            }
            using (var session = new BrowserSession(configuration))
            {
                Assert.That(session, Is.Not.SameAs(firstBrowserSession));
            }
        }

        [Test]
        public void A_session_gets_its_driver_from_config()
        {
            configuration.Driver = typeof (FakeDriver);
            using (var browserSession = new BrowserSession(configuration))
            {
                Assert.That(browserSession.Driver, Is.TypeOf(typeof(FakeDriver)));
            }

            configuration.Driver = typeof(StubDriver);
            using (var browserSession = new BrowserSession(configuration))
            {
                Assert.That(browserSession.Driver, Is.TypeOf(typeof(StubDriver)));
            }
        }

        [Test]
        public void Session_exposes_native_driver_if_you_really_need_it()
        {
            using (var browserSession = new BrowserSession(configuration))
            {
                Assert.That(browserSession.Native, Is.EqualTo("Native driver on fake driver"));
            }
        }

    }
}