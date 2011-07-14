using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    internal class TestSessionBuilder
    {
        internal static Session Build(WebResources webResources, FileSystem fileSystem)
        {
            return Build(new FakeDriver(), new SpyRobustWrapper(), new FakeWaiter(), webResources, fileSystem);
        }

        internal static Session Build(Driver driver, RobustWrapper robustWrapper, Waiter waiter)
        {
            return Build(driver, robustWrapper, waiter, null, null);
        }

        internal static Session Build(Driver driver, RobustWrapper robustWrapper, Waiter waiter,
                                      WebResources webResources, FileSystem fileSystem)
        {
            return new Session(driver, robustWrapper, waiter, webResources, fileSystem);
        }
    }
}