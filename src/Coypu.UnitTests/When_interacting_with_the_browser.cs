using System;
using System.Linq;
using Coypu.UnitTests.TestDoubles;
using NUnit.Framework;

namespace Coypu.UnitTests
{
	[TestFixture]
	public class When_interacting_with_the_browser
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
		public void Click_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node();
			session.Click(node);

			Assert.That(driver.ClickedNodes, Is.Empty);

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node();
			driver.StubButton("Find button robustly", node);
			session.FindButton("Find button robustly");

			var found = ((Func<Node>) spyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(found, Is.SameAs(node));
		}

		[Test]
		public void Visit_should_pass_message_to_the_driver()
		{
			new Session(driver, null).Visit("http://visit.me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://visit.me"));
		}

		[Test]
		public void Click_button_should_robustly_find_by_text_and_click()
		{
			var node = new Node();
			driver.StubButton("Some button locator", node);

			session.ClickButton("Some button locator");

			Assert.That(driver.ClickedNodes, Has.No.Member(node));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_link_should_robustly_find_link_by_locator_and_click()
		{
			var node = new Node();
			driver.StubLink("Some link locator", node);

			session.ClickLink("Some link locator");

			Assert.That(driver.ClickedNodes, Has.No.Member(node));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedNodes, Has.Member(node));
		}
	}
}