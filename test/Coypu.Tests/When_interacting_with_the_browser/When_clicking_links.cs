using System;
using System.Linq;
using Coypu.Drivers;
using Coypu.Queries;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
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
            Assert.That(driver.ClickedElements, Has.Member(linkToBeClicked));
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(false, 321)]
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
            Assert.That(queryResult, Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.Until.Options.Timeout, Is.EqualTo(waitBetweenRetries));
            Assert.That(tryUntilArgs.OverallTimeout, Is.EqualTo(overallTimeout));
        }

        [TestCase(200)]
        [TestCase(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubLinkToBeClicked = StubLinkToBeClicked("Some link locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            sessionConfiguration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
                                    {
                                        Assert.That(milliseconds, Is.EqualTo(expectedWait));

                                        AssertLinkFound();
                                        AssertButtonNotClickedYet(stubLinkToBeClicked);

                                        waiterCalled = true;
                                    });
            browserSession.ClickLink("Some link locator");
            ExecuteLastDeferedRobustAction();

            Assert.That(waiterCalled, "The waiter was not called");
            AssertClicked(stubLinkToBeClicked);
        }

        private void AssertLinkFound()
        {
            Assert.That(driver.FindXPathRequests.Any(), "Wait called before find");
        }

        private void AssertButtonNotClickedYet(StubElement linkToBeClicked)
        {
            Assert.That(driver.ClickedElements, Has.No.Member(linkToBeClicked), "Expected link not to have been clicked yet, but it has been");
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