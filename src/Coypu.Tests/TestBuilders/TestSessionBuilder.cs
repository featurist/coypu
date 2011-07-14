using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    internal class TestSessionBuilder
    {
        internal static Session Build(Driver driver, RobustWrapper robustWrapper, Waiter waiter, ResourceDownloader resourceDownloader, UrlBuilder urlBuilder)
        {
            return new Session(driver, robustWrapper, waiter, resourceDownloader, urlBuilder);
        }
    }
}