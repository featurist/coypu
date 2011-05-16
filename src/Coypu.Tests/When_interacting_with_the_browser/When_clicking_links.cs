using System;
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

		[Test]
		public void It_robustly_finds_by_text_and_clicks()
		{
			var linkToBeClicked = StubLinkToBeClicked();

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
			var linkToBeClicked = StubLinkToBeClicked();

			session.ClickLink("Some link locator", () => stubUntil, waitBetweenRetries);

			var tryUntilArgs = GetTryUntilArgs();

			AssertNotClickedYet(linkToBeClicked);
			ExecuteDeferedRobustAction();
			AssertClicked(linkToBeClicked);

			Assert.That(tryUntilArgs.Until(), Is.EqualTo(stubUntil));
			Assert.That(tryUntilArgs.UntilTimeout, Is.EqualTo(waitBetweenRetries));
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

		private StubElement StubLinkToBeClicked()
		{
			var linkToBeClicked = new StubElement();
			driver.StubLink("Some link locator", linkToBeClicked);
			return linkToBeClicked;
		}
	}
}