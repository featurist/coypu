using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_clicking_links : BrowserInteractionTests
    {
        [Test]
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
            var waitBetweenRetries = TimeSpan.FromSeconds(waitBeforeRetrySecs);
            var overallTimeout = TimeSpan.FromSeconds(waitBeforeRetrySecs + 1000);
            var linkToBeClicked = StubLinkToBeClicked("Some link locator");

            var options = new Options{Timeout = overallTimeout};
            browserSession.ClickLink("Some link locator", new LambdaPredicateQuery(() => stubUntil), waitBetweenRetries,options);

            var tryUntilArgs = GetTryUntilArgs();

            AssertButtonNotClickedYet(linkToBeClicked);
            tryUntilArgs.TryThisBrowserAction.Act();
            AssertClicked(linkToBeClicked);

            tryUntilArgs.Until.Run();
            Assert.That(tryUntilArgs.Until.Result, Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.WaitBeforeRetry, Is.EqualTo(waitBetweenRetries));
            Assert.That(tryUntilArgs.OverallTimeout, Is.EqualTo(overallTimeout));
        }

        [TestCase(200)]
        [TestCase(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubLinkToBeClicked = StubLinkToBeClicked("Some link locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            SessionConfiguration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
                                    {
                                        Assert.That(milliseconds, Is.EqualTo(expectedWait));

                                        AssertButtonFound();
                                        AssertButtonNotClickedYet(stubLinkToBeClicked);

                                        waiterCalled = true;
                                    });
            browserSession.ClickLink("Some link locator");
            ExecuteLastDeferedRobustAction();

            Assert.That(waiterCalled, "The waiter was not called");
            AssertClicked(stubLinkToBeClicked);
        }

        private void AssertButtonFound()
        {
            Assert.That(driver.FindLinkRequests.Contains("Some link locator"), "Wait called before find");
        }

        private void AssertButtonNotClickedYet(StubElement linkToBeClicked)
        {
            Assert.That(driver.ClickedElements, Has.No.Member(linkToBeClicked), "Expected link not to have been clicked yet, but it has been");
        }

        private SpyRobustWrapper.TryUntilArgs GetTryUntilArgs()
        {
            return spyRobustWrapper.DeferredTryUntils.Single();
        }

        private void ExecuteLastDeferedRobustAction()
        {
            spyRobustWrapper.QueriesRan<object>().Last().Run();
        }

        private StubElement StubLinkToBeClicked(string someLinkLocator)
        {
            var linkToBeClicked = new StubElement { Id = Guid.NewGuid().ToString() };
            driver.StubLink(someLinkLocator, linkToBeClicked, browserSession);
            return linkToBeClicked;
        }
    }
}