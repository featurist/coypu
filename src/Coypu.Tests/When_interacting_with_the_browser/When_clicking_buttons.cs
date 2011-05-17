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
			var buttonToBeClicked = StubButtonToBeClicked();

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
			var buttonToBeClicked = StubButtonToBeClicked();

			session.ClickButton("Some button locator", () => stubUntil, waitBetweenRetries);

			var tryUntilArgs = GetTryUntilArgs();

			AssertNotClickedYet(buttonToBeClicked);
			ExecuteDeferedRobustAction();
			AssertClicked(buttonToBeClicked);

			Assert.That(tryUntilArgs.Until(), Is.EqualTo(stubUntil));
			Assert.That(tryUntilArgs.UntilTimeout, Is.EqualTo(waitBetweenRetries));
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

		private StubElement StubButtonToBeClicked()
		{
			var buttonToBeClicked = new StubElement();
			driver.StubButton("Some button locator", buttonToBeClicked);
			return buttonToBeClicked;
		}
	}
}