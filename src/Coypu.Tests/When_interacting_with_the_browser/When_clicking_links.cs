using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_clicking_links
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

        [TearDown]
        public void TearDown()
        {
            Configuration.WaitBeforeClick = TimeSpan.Zero;
        }

		[Test]
		public void It_robustly_finds_by_text_and_clicks()
		{
			var linkToBeClicked = StubLinkToBeClicked("Some link locator");

			session.ClickLink("Some link locator");

			AssertNotClickedYet(linkToBeClicked);
			ExecuteDeferedRobustAction();
			AssertClicked(linkToBeClicked);
		}

		private void AssertClicked(StubElement linkToBeClicked)
		{
			Assert.That(driver.ClickedElements, Has.Member(linkToBeClicked));
		}

		[TestCase(true, 1)]
		[TestCase(false, 1)]
		[TestCase(false, 321)]
        public void It_tries_clicking_robustly_until_expected_conditions_met(bool stubUntil, int untilTimeoutSecs)
        {
            var waitBetweenRetries = TimeSpan.FromSeconds(untilTimeoutSecs);
            var linkToBeClicked = StubLinkToBeClicked("Some link locator");

            session.ClickLink("Some link locator", () => stubUntil, waitBetweenRetries);

            var tryUntilArgs = GetTryUntilArgs();

            AssertNotClickedYet(linkToBeClicked);
            ExecuteDeferedRobustAction();
            AssertClicked(linkToBeClicked);

            Assert.That(tryUntilArgs.Until(), Is.EqualTo(stubUntil));
            Assert.That(tryUntilArgs.UntilTimeout, Is.EqualTo(waitBetweenRetries));
        }

        [Test]
        public void It_waits_between_find_and_click_as_configured()
        {
            var expectedWaitBeforeClick = TimeSpan.FromMilliseconds(200);

            var stubLinkToBeClicked = StubLinkToBeClicked("Some link locator");

            Configuration.WaitBeforeClick = TimeSpan.FromSeconds(0);
            var waitBeforeClickControl = RecordWaitBeforeClickLinkTiming("Some link locator", stubLinkToBeClicked);

            Configuration.WaitBeforeClick = expectedWaitBeforeClick;
            var waitBeforeClickActual = RecordWaitBeforeClickLinkTiming("Some link locator", stubLinkToBeClicked);

            Assert.That(waitBeforeClickActual, Is.InRange(expectedWaitBeforeClick - waitBeforeClickControl, expectedWaitBeforeClick + waitBeforeClickControl));
        }

	    private TimeSpan RecordWaitBeforeClickLinkTiming(string locator, Element stubLinkToBeClicked)
	    {
            session.ClickLink(locator);
            ExecuteLastDeferedRobustAction();

	        var times = driver.Timings.Keys;

            var findTiming = times.ElementAt(times.Count - 2);
            var clickTiming = times.ElementAt(times.Count - 1);

            // Verity timings
	        Assert.That(driver.Timings[findTiming], Is.StringContaining(locator));
            Assert.That(driver.Timings[clickTiming], Is.StringContaining(stubLinkToBeClicked.Id));
            
	        return TimeSpan.FromTicks(clickTiming) - TimeSpan.FromTicks(findTiming);
	    }

	    private void AssertNotClickedYet(StubElement linkToBeClicked)
		{
			Assert.That(driver.ClickedElements, Has.No.Member(linkToBeClicked));
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

		private StubElement StubLinkToBeClicked(string someLinkLocator)
		{
		    var linkToBeClicked = new StubElement();
		    linkToBeClicked.SetId(Guid.NewGuid().ToString());
			driver.StubLink(someLinkLocator, linkToBeClicked);
			return linkToBeClicked;
		}
	}
}