using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_clicking
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
			var node = new StubNode();
			session.Click(node);

			Assert.That(driver.ClickedNodes, Is.Empty);

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_button_should_robustly_find_by_text_and_click()
		{
			var node = new StubNode();
			driver.StubButton("Some button locator", node);

			session.ClickButton("Some button locator");

			Assert.That(driver.ClickedNodes, Has.No.Member(node));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_link_should_robustly_find_link_by_locator_and_click()
		{
			var node = new StubNode();
			driver.StubLink("Some link locator", node);

			session.ClickLink("Some link locator");

			Assert.That(driver.ClickedNodes, Has.No.Member(node));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedNodes, Has.Member(node));
		}
	}
}