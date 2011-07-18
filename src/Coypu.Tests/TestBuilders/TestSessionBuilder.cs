using Coypu.Robustness;
using Coypu.WebRequests;

namespace Coypu.Tests.TestBuilders
{
    internal class TestSessionBuilder
    {
        internal static Session Build(Driver driver, RobustWrapper robustWrapper, Waiter waiter,
                                      RestrictedResourceDownloader _restrictedResourceDownloader, UrlBuilder urlBuilder)
        {
            return new Session(driver, robustWrapper, waiter, _restrictedResourceDownloader, urlBuilder);
        }
    }
}