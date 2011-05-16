using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_clicking_buttons
	{
		private FakeDriver driver;
		private SpyRobustWrapper spyRobustWrapper;
		private Session session;

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			spyRobustWrapper = new SpyRobustWrapper();
			session = new Session(driver, spyRobustWrapper);
		}

		[Test]
		public void It_robustly_finds_by_text_and_clicks()
		{
			var buttonToBeClicked = StubButtonToBeClicked("Some button locator");

			session.ClickButton("Some button locator");

			AssertNotClickedYet(buttonToBeClicked);
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
		public void It_tries_clicking_robustly_until_expected_conditions_met(bool stubUntil, int untilTimeoutSecs)
		{
			var waitBetweenRetries = TimeSpan.FromSeconds(untilTimeoutSecs);
			var buttonToBeClicked = StubButtonToBeClicked("Some button locator");

			session.ClickButton("Some button locator", () => stubUntil, waitBetweenRetries);

			var tryUntilArgs = GetTryUntilArgs();

			AssertNotClickedYet(buttonToBeClicked);
			ExecuteDeferedRobustAction();
			AssertClicked(buttonToBeClicked);

			Assert.That(tryUntilArgs.Until(), Is.EqualTo(stubUntil));
			Assert.That(tryUntilArgs.UntilTimeout, Is.EqualTo(waitBetweenRetries));
		}

        [Test]
        public void It_waits_between_find_and_click_as_configured()
        {
            var expectedWaitBeforeClick = TimeSpan.FromMilliseconds(200);

            var stubLinkToBeClicked = StubButtonToBeClicked("Some button locator");

            Configuration.WaitBeforeClick = TimeSpan.FromSeconds(0);
            var waitBeforeClickControl = RecordWaitBeforeClickButtonTiming("Some button locator", stubLinkToBeClicked);

            Configuration.WaitBeforeClick = expectedWaitBeforeClick;
            var waitBeforeClickActual = RecordWaitBeforeClickButtonTiming("Some button locator", stubLinkToBeClicked);

            Assert.That(waitBeforeClickActual, Is.InRange(expectedWaitBeforeClick - waitBeforeClickControl, expectedWaitBeforeClick + waitBeforeClickControl));
        }

        private TimeSpan RecordWaitBeforeClickButtonTiming(string locator, Element stubLinkToBeClicked)
        {
            session.ClickButton(locator);
            ExecuteLastDeferedRobustAction();

            var times = driver.Timings.Keys;

            var findTiming = times.ElementAt(times.Count - 2);
            var clickTiming = times.ElementAt(times.Count - 1);

            // Verity timings
            Assert.That(driver.Timings[findTiming], Is.StringContaining(locator));
            Assert.That(driver.Timings[clickTiming], Is.StringContaining(stubLinkToBeClicked.Id));

            return TimeSpan.FromTicks(clickTiming) - TimeSpan.FromTicks(findTiming);
        }

		private void AssertNotClickedYet(StubElement buttonToBeClicked)
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
			spyRobustWrapper.DeferredActions.Single()();
		}

        private void ExecuteLastDeferedRobustAction()
        {
            spyRobustWrapper.DeferredActions.Last()();
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