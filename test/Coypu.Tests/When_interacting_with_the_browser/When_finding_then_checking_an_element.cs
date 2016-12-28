using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser {
    public class When_finding_then_checking_an_element : BrowserInteractionTests
    {
        [Fact]
        public void It_finds_then_synchronises_check_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_click", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromSynchronise(element);

            var toCheck = browserSession.FindCss("something_to_click");

            toCheck.Check();

            Assert.DoesNotContain(toCheck, driver.CheckedElements);

            RunQueryAndCheckTiming();

            Assert.Contains(toCheck, driver.CheckedElements);
        }
    }
}