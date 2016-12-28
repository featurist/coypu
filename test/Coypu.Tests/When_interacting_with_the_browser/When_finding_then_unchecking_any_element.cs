using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_finding_then_unchecking_any_element : BrowserInteractionTests
    {
        [Fact]
        public void It_makes_robust_call_to_find_then_clicks_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_click", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromSynchronise(element);

            var toCheck = browserSession.FindCss("something_to_click");

            toCheck.Uncheck();

            Assert.DoesNotContain(toCheck, driver.UncheckedElements);

            RunQueryAndCheckTiming();

            Assert.Contains(toCheck, driver.UncheckedElements);
        }
    }
}