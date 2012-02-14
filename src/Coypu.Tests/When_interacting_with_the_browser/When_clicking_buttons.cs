using System;
using System.Linq;
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

            session.ClickButton("Some button locator");

            AssertButtonNotClickedYet(buttonToBeClicked);
            ExecuteDeferedRobustAction();
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

            session.ClickButton("Some button locator", () => stubUntil, waitBetweenRetries);

            var tryUntilArgs = GetTryUntilArgs();

            AssertButtonNotClickedYet(buttonToBeClicked);
            ExecuteDeferedRobustAction();
            AssertClicked(buttonToBeClicked);

            Assert.That(tryUntilArgs.Until(), Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.WaitBeforeRetry, Is.EqualTo(waitBetweenRetries));
        }

        [TestCase(200)]
        [TestCase(300)]
        public void It_waits_between_find_and_click_as_configured(int waitMs)
        {
            var stubButtonToBeClicked = StubButtonToBeClicked("Some button locator");
            var expectedWait = TimeSpan.FromMilliseconds(waitMs);
            Configuration.WaitBeforeClick = expectedWait;

            var waiterCalled = false;
            fakeWaiter.DoOnWait(milliseconds =>
            {
                Assert.That(milliseconds, Is.EqualTo(expectedWait));

                AssertButtonFound();
                AssertButtonNotClickedYet(stubButtonToBeClicked);

                waiterCalled = true;
            });
            session.ClickButton("Some button locator");
            ExecuteLastDeferedRobustAction();

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

        private SpyRobustWrapper.TryUntilArgs GetTryUntilArgs()
        {
            var tryUntilArgs = spyRobustWrapper.DeferredTryUntils.Single();
            tryUntilArgs.TryThis();
            return tryUntilArgs;
        }

        private void ExecuteDeferedRobustAction()
        {
            spyRobustWrapper.DeferredDriverActions.Single().Act();
        }

        private void ExecuteLastDeferedRobustAction()
        {
            spyRobustWrapper.DeferredDriverActions.Last().Act();
        }

        private StubElement StubButtonToBeClicked(string locator)
        {
            var buttonToBeClicked = new StubElement();
            buttonToBeClicked.SetId(Guid.NewGuid().ToString());
            driver.StubButton(locator, buttonToBeClicked);
            return buttonToBeClicked;
        }
    }
}