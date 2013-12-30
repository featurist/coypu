using System;
using System.Linq;
using Coypu.Drivers;
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
            var overallTimeout  = TimeSpan.FromMilliseconds(waitBeforeRetrySecs * 1000);
            var options = new Options {Timeout = overallTimeout};
            var waitBetweenRetries = TimeSpan.FromSeconds(waitBeforeRetrySecs);
            var buttonToBeClicked = StubButtonToBeClicked("Some button locator", options);

            browserSession.ClickButton("Some button locator", new LambdaPredicateQuery(() => stubUntil, new Options{Timeout = waitBetweenRetries}) , options);

            var tryUntilArgs = SpyTimingStrategy.DeferredTryUntils.Single();

            AssertButtonNotClickedYet(buttonToBeClicked);
            tryUntilArgs.TryThisBrowserAction.Act();
            AssertClicked(buttonToBeClicked);

            var queryResult = tryUntilArgs.Until.Run();
            Assert.That(queryResult, Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.Until.Options.Timeout, Is.EqualTo(waitBetweenRetries));
            Assert.That(tryUntilArgs.OverallTimeout, Is.EqualTo(overallTimeout));
        }

        [TestCase(200)]
        [TestCase(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubButtonToBeClicked = StubButtonToBeClicked("Some button locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            sessionConfiguration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
            {
                Assert.That(milliseconds, Is.EqualTo(expectedWait));

                AssertButtonFound();
                AssertButtonNotClickedYet(stubButtonToBeClicked);

                waiterCalled = true;
            });
            browserSession.ClickButton("Some button locator");
            SpyTimingStrategy.QueriesRan<object>().Last().Run();

            Assert.That(waiterCalled, "The waiter was not called");
            AssertClicked(stubButtonToBeClicked);
        }

        private void AssertButtonFound()
        {
            Assert.That(driver.FindXPathRequests.Any(), "Wait called before find");
        }

        private void AssertButtonNotClickedYet(StubElement buttonToBeClicked)
        {
            Assert.That(driver.ClickedElements, Has.No.Member(buttonToBeClicked));
        }

        private StubElement StubButtonToBeClicked(string locator, Options options = null)
        {
            var buttonToBeClicked = new StubElement {Id = Guid.NewGuid().ToString()};
            var buttonXpath = new Html(browserSession.Browser.UppercaseTagNames).Button(locator, options ?? sessionConfiguration);

            driver.StubAllXPath(buttonXpath, new []{buttonToBeClicked}, browserSession, options ?? sessionConfiguration);

            return buttonToBeClicked;
        }
    }
}