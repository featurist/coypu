using System.Linq;
using Coypu.API;
using NUnit.Framework;

namespace Coypu.UnitTests
{
	[TestFixture]
	public class When_interacting_with_the_browser_session
	{
		[Test]
		public void Visit_should_pass_message_to_the_driver()
		{
			var driver = new FakeDriver();
			new Session(driver).Visit("http://visit.me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://visit.me"));
		}

		[Test]
		public void Click_button_should_find_by_text_and_click()
		{
			var driver = new FakeDriver();
			var node = new Node(driver);
			driver.StubButton("Some button text", node);

			var session = new Session(driver);

			session.ClickButton("Some button text");

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_button_should_find_button_by_locator_and_click()
		{
			var driver = new FakeDriver();
			var node = new Node(driver);
			driver.StubButton("Some button locator", node);

			var session = new Session(driver);

			session.ClickButton("Some button locator");

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void Click_link_should_find_link_by_locator_and_click()
		{
			var driver = new FakeDriver();
			var node = new Node(driver);
			driver.StubLink("Some button locator", node);

			var session = new Session(driver);

			session.ClickLink("Some button locator");

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}
	}
}