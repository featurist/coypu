using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests
{
    public class When_starting_and_ending_sessions
    {
        private SessionConfiguration SessionConfiguration;

        public When_starting_and_ending_sessions()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Driver = typeof (FakeDriver);
        }

        [Fact]
        public void Dispose_handles_a_disposed_session()
        {
            var browserSession = new BrowserSession(SessionConfiguration);

            browserSession.Dispose();
            browserSession.Dispose();
        }

        [Fact]
        public void A_session_gets_its_driver_from_config()
        {
            SessionConfiguration.Driver = typeof (FakeDriver);
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.IsType<FakeDriver>(browserSession.Driver);
            }

            SessionConfiguration.Driver = typeof(StubDriver);
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.IsType<StubDriver>(browserSession.Driver);
            }
        }

        [Fact]
        public void Session_exposes_native_driver_if_you_really_need_it()
        {
            using (var browserSession = new BrowserSession(SessionConfiguration))
            {
                Assert.Equal("Native driver on fake driver", browserSession.Native);
            }
        }

    }
}