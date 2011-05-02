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
			var element = new StubElement();
			session.Click(element);

			Assert.That(driver.ClickedElements, Is.Empty);

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedElements, Has.Member(element));
		}

		[Test]
		public void Click_button_should_robustly_find_by_text_and_click()
		{
			var element = new StubElement();
			driver.StubButton("Some button locator", element);

			session.ClickButton("Some button locator");

			Assert.That(driver.ClickedElements, Has.No.Member(element));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedElements, Has.Member(element));
		}

		[Test]
		public void Click_link_should_robustly_find_link_by_locator_and_click()
		{
			var element = new StubElement();
			driver.StubLink("Some link locator", element);

			session.ClickLink("Some link locator");

			Assert.That(driver.ClickedElements, Has.No.Member(element));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedElements, Has.Member(element));
		}
	}
}