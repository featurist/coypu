using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.Session.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_clicking : APITests
	{
		[Test]
		public void Click_should_make_robust_call_to_underlying_driver()
		{
			var node = new TestNode();
			Session.Click(node);

			Assert.That(Driver.ClickedNodes, Is.Empty);

			SpyRobustWrapper.DeferredActions.Single()();

			Assert.That(Driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_button_should_robustly_find_by_text_and_click()
		{
			var node = new TestNode();
			Driver.StubButton("Some button locator", node);

			Session.ClickButton("Some button locator");

			Assert.That(Driver.ClickedNodes, Has.No.Member(node));
			SpyRobustWrapper.DeferredActions.Single()();
			Assert.That(Driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_link_should_robustly_find_link_by_locator_and_click()
		{
			var node = new TestNode();
			Driver.StubLink("Some link locator", node);

			Session.ClickLink("Some link locator");

			Assert.That(Driver.ClickedNodes, Has.No.Member(node));
			SpyRobustWrapper.DeferredActions.Single()();
			Assert.That(Driver.ClickedNodes, Has.Member(node));
		}
	}
}