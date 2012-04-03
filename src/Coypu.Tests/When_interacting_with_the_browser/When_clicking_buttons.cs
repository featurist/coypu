using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_clicking_buttons : BrowserInteractionTests
    {
        [Test]
        public void It_robustly_finds_by_text_and_clicks()
        {
            var buttonToBeClicked = StubButtonToBeClicked("Some button locator");

            browserSession.ClickButton("Some button locator");

            AssertButtonNotClickedYet(buttonToBeClicked);

            RunQueryAndCheckTiming();

            AssertClicked(buttonToBeClicked);
        }

        private void AssertClicked(StubElement buttonToBeClicked)
        {
            Assert.That(driver.ClickedElements, Has.Member(buttonToBeClicked));
        }

        [TestCase(true, 1)]
        [TestCase(false, 1)]
        [TestCase(false, 321)]
        public void It_tries_clicking_robustly_until_expected_conditions_met(bool stubUntil, int waitBeforeRetrySecs)
        {
            var waitBetweenRetries = TimeSpan.FromSeconds(waitBeforeRetrySecs);
            var buttonToBeClicked = StubButtonToBeClicked("Some button locator");
            var overallTimeout  = TimeSpan.FromMilliseconds(waitBeforeRetrySecs + 1000);

            var options = new Options {Timeout = overallTimeout};
            browserSession.ClickButton("Some button locator", new LambdaPredicateQuery(() => stubUntil), waitBetweenRetries, options);

            var tryUntilArgs = spyRobustWrapper.DeferredTryUntils.Single();

            AssertButtonNotClickedYet(buttonToBeClicked);
            tryUntilArgs.TryThisBrowserAction.Act();
            AssertClicked(buttonToBeClicked);

            tryUntilArgs.Until.Run();
            Assert.That(tryUntilArgs.Until.Result, Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.WaitBeforeRetry, Is.EqualTo(waitBetweenRetries));
            Assert.That(tryUntilArgs.OverallTimeout, Is.EqualTo(overallTimeout));
        }

        [TestCase(200)]
        [TestCase(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubButtonToBeClicked = StubButtonToBeClicked("Some button locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            SessionConfiguration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
            {
                Assert.That(milliseconds, Is.EqualTo(expectedWait));

                AssertButtonFound();
                AssertButtonNotClickedYet(stubButtonToBeClicked);

                waiterCalled = true;
            });
            browserSession.ClickButton("Some button locator");
            spyRobustWrapper.QueriesRan<object>().Last().Run();

            Assert.That(waiterCalled, "The waiter was not called");
            AssertClicked(stubButtonToBeClicked);
        }

        private void AssertButtonFound()
        {
            Assert.That(driver.FindButtonRequests.Contains("Some button locator"), "Wait called before find");
        }

        private void AssertButtonNotClickedYet(StubElement buttonToBeClicked)
        {
            Assert.That(driver.ClickedElements, Has.No.Member(buttonToBeClicked));
        }

        private StubElement StubButtonToBeClicked(string locator)
        {
            var buttonToBeClicked = new StubElement {Id = Guid.NewGuid().ToString()};
            driver.StubButton(locator, buttonToBeClicked, browserSession);
            return buttonToBeClicked;
        }
    }
}