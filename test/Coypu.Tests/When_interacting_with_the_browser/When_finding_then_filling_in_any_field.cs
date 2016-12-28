using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser {
    public class When_finding_then_filling_in_any_field : BrowserInteractionTests
    {
        [Fact]
        public void It_makes_robust_call_to_find_then_clicks_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_fill_in", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromSynchronise(element);

            var elementScope = browserSession.FindId("something_to_fill_in");

            elementScope.FillInWith("some filled in stuff");

            Assert.DoesNotContain(elementScope, driver.SetFields.Keys);

            RunQueryAndCheckTiming();

            Assert.Contains(elementScope, driver.SetFields.Keys);
            Assert.Equal("some filled in stuff", driver.SetFields[elementScope].Value);
        }
    }
}