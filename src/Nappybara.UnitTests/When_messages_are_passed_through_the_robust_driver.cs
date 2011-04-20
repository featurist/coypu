using System;
using System.Linq;
using Nappybara.Drivers;
using NUnit.Framework;

namespace Nappybara.UnitTests
{
	[TestFixture]
	public class When_messages_are_passed_through_the_robust_driver
	{
		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			spyRobustWrapper = new SpyRobustWrapper();
		}

		private FakeDriver driver;
		private SpyRobustWrapper spyRobustWrapper;

		[Test]
		public void ClickButton_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node(driver);
			new RobustDriver(driver, spyRobustWrapper).Click(node);

			Assert.That(driver.ClickedNodes, Has.No.Member(node));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node(driver);
			driver.StubButton("Find button robustly", node);

			new RobustDriver(driver, spyRobustWrapper).FindButton("Find button robustly");

			var found = ((Func<Node>) spyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(found, Is.SameAs(node));
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			var node = new Node(driver);
			driver.StubLink("Find link robustly", node);

			new RobustDriver(driver, spyRobustWrapper).FindLink("Find link robustly");

			var found = ((Func<Node>) spyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(found, Is.SameAs(node));
		}

		[Test]
		public void Visit_should_make_direct_call_to_underlying_driver()
		{
			new RobustDriver(driver, spyRobustWrapper).Visit("some url");
		}
	}
}