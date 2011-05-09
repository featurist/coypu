using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_clicking_any_element
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
		public void It_should_make_robust_call_to_underlying_driver()
		{
			var element = new StubElement();
			session.Click(element);

			Assert.That(driver.ClickedElements, Is.Empty);

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedElements, Has.Member(element));
		}

	}
}