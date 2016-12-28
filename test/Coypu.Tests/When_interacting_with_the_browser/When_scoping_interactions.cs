using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_scoping_interactions : BrowserInteractionTests
    {
        [Fact]
        public void It_sets_the_scope_on_the_driver() 
        {
            browserSession = TestSessionBuilder.Build(new SessionConfiguration(), driver, new ImmediateSingleExecutionFakeTimingStrategy(), fakeWaiter, null, stubUrlBuilder);
            var section = new StubElement();
            var expectedLink = new StubElement();
            driver.StubId("some_section", section, browserSession, sessionConfiguration);

            var innerScope = browserSession.FindSection("some section");

            driver.StubId("some_element", expectedLink, innerScope, sessionConfiguration);

            var actualLink = innerScope.FindId("some_element").Now();

            Assert.Same(expectedLink, actualLink);

        }
    }
}