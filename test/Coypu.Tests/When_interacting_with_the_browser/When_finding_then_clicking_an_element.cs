using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser {
    public class When_finding_then_clicking_an_element_ : BrowserInteractionTests
    {
        [Fact]
        public void It_finds_then_synchronises_click_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_click", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromSynchronise(element);

            var elementScope = browserSession.FindId("something_to_click");
            elementScope.Click();

            Assert.DoesNotContain(elementScope, driver.ClickedElements);

            RunQueryAndCheckTiming();

            Assert.Contains(elementScope, driver.ClickedElements);
        }
    }
}