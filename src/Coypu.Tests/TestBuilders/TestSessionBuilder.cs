using Coypu.Robustness;
using Coypu.Tests.When_interacting_with_the_browser;
using Coypu.WebRequests;

namespace Coypu.Tests.TestBuilders
{
    internal class TestSessionBuilder
    {
        internal static BrowserSession Build(SessionConfiguration SessionConfiguration, Driver driver, RobustWrapper robustWrapper, Waiter waiter,
                                      RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
        {
            return new BrowserSession(new StubDriverFactory(driver), SessionConfiguration, robustWrapper, waiter, restrictedResourceDownloader, urlBuilder);
        }
    }
}