using Coypu.Timing;
using Coypu.Tests.When_interacting_with_the_browser;
using Coypu.WebRequests;

namespace Coypu.Tests.TestBuilders
{
    internal class TestSessionBuilder
    {
        internal static BrowserSession Build(SessionConfiguration SessionConfiguration, Driver driver, TimingStrategy timingStrategy, Waiter waiter,
                                      RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
        {
            return new BrowserSession(new StubDriverFactory(driver), SessionConfiguration, timingStrategy, waiter, restrictedResourceDownloader, urlBuilder);
        }
    }
}