using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests
{
    [TestFixture]
    public class When_starting_and_ending_sessions
    {
        private SessionConfiguration SessionConfiguration;

        [SetUp]
        public void SetUp()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Driver = typeof (FakeDriver);
        }

        [Test]
        public void Dispose_handles_a_disposed_session()
        {
            var browserSession = new BrowserSession(SessionConfiguration);

            browserSession.Dispose();
            browserSession.Dispose();
        }

        [Test]
        public void After_disposing_the_session_a_new_session_is_available()
        {
            BrowserSession firstBrowserSession;
            using (var session = new BrowserSession(SessionConfiguration))
            {
                firstBrowserSession = session;
            }
            using (var session = new BrowserSession(SessionConfiguration))
            {
                Assert.That(session, Is.Not.SameAs(firstBrowserSession));
            }
        }

        [Test]
        public void A_session_gets_its_driver_from_config()
        {
            SessionConfiguration.Driver = typeof (FakeDriver);
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.That(browserSession.Driver, Is.TypeOf(typeof(FakeDriver)));
            }

            SessionConfiguration.Driver = typeof(StubDriver);
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.That(browserSession.Driver, Is.TypeOf(typeof(StubDriver)));
            }
        }

        [Test]
        public void Session_exposes_native_driver_if_you_really_need_it()
        {
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.That(browserSession.Native, Is.EqualTo("Native driver on fake driver"));
            }
        }

    }
}