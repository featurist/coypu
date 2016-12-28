using System;
using System.Linq;
using Coypu.Drivers;
using Coypu.Queries;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_clicking_links : BrowserInteractionTests
    {
        [Fact]
        public void It_robustly_finds_by_text_and_clicks()
        {
            var linkToBeClicked = StubLinkToBeClicked("Some link locator");

            browserSession.ClickLink("Some link locator");

            AssertButtonNotClickedYet(linkToBeClicked);

            RunQueryAndCheckTiming();

            AssertClicked(linkToBeClicked);
        }

        private void AssertClicked(StubElement linkToBeClicked)
        {
            Assert.Contains(linkToBeClicked, driver.ClickedElements);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 1)]
        [InlineData(false, 123)]
        public void It_tries_clicking_robustly_until_expected_conditions_met(bool stubUntil, int waitBeforeRetrySecs)
        {
            var overallTimeout = TimeSpan.FromSeconds(waitBeforeRetrySecs * 1000);
            var options = new Options{Timeout = overallTimeout};
            var waitBetweenRetries = TimeSpan.FromSeconds(waitBeforeRetrySecs);
            var linkToBeClicked = StubLinkToBeClicked("Some link locator", options);

            browserSession.ClickLink("Some link locator", new LambdaPredicateQuery(() => stubUntil, new Options{Timeout = waitBetweenRetries}), options);

            var tryUntilArgs = GetTryUntilArgs();

            AssertButtonNotClickedYet(linkToBeClicked);
            tryUntilArgs.TryThisBrowserAction.Act();
            AssertClicked(linkToBeClicked);

            var queryResult = tryUntilArgs.Until.Run();
            Assert.Equal(stubUntil, queryResult);
            Assert.Equal(waitBetweenRetries, tryUntilArgs.Until.Options.Timeout);
            Assert.Equal(overallTimeout, tryUntilArgs.OverallTimeout);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubLinkToBeClicked = StubLinkToBeClicked("Some link locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            sessionConfiguration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
                                    {
                                        Assert.Equal(expectedWait, milliseconds);

                                        AssertLinkFound();
                                        AssertButtonNotClickedYet(stubLinkToBeClicked);

                                        waiterCalled = true;
                                    });
            browserSession.ClickLink("Some link locator");
            ExecuteLastDeferedRobustAction();

            Assert.True(waiterCalled, "The waiter was not called");
            AssertClicked(stubLinkToBeClicked);
        }

        private void AssertLinkFound()
        {
            Assert.True(driver.FindXPathRequests.Any(), "Wait called before find");
        }

        private void AssertButtonNotClickedYet(StubElement linkToBeClicked)
        {
            Assert.DoesNotContain(linkToBeClicked, driver.ClickedElements);
        }

        private SpyTimingStrategy.TryUntilArgs GetTryUntilArgs()
        {
            return SpyTimingStrategy.DeferredTryUntils.Single();
        }

        private void ExecuteLastDeferedRobustAction()
        {
            SpyTimingStrategy.QueriesRan<object>().Last().Run();
        }

        private StubElement StubLinkToBeClicked(string locator, Options options = null)
        {
            var linkToBeClicked = new StubElement { Id = Guid.NewGuid().ToString() };
            var linkXPath = new Html(sessionConfiguration.Browser.UppercaseTagNames).Link(locator, options ?? sessionConfiguration);
            driver.StubAllXPath(linkXPath, new[]{linkToBeClicked}, browserSession, options ?? sessionConfiguration);
            return linkToBeClicked;
        }
    }
}