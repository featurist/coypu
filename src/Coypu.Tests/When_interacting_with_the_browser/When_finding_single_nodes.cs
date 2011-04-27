using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding_single_nodes
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
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindButton, driver.StubButton);
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindLink, driver.StubLink);
		}

		[Test]
		public void FindField_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindField, driver.StubField);
		}

		[Test]
		public void FindCss_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindCss, driver.StubCss);
		}

		[Test]
		public void FindXPath_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(session.FindXPath, driver.StubXPath);
		}

		protected void Should_find_robustly(Func<string, Node> subject, Action<string, Node> stub)
		{
			var locator = "Find me " + DateTime.Now.Ticks;

			var expectedImmediateResult = new StubNode();
			var expectedDeferredResult = new StubNode();

			spyRobustWrapper.AlwaysReturnFromRobustly(typeof(Node), expectedImmediateResult);
			stub(locator, expectedDeferredResult);

			var actualImmediateResult = subject(locator);
			Assert.That(actualImmediateResult, Is.Not.SameAs(expectedDeferredResult), "Result was not found robustly");
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			var actualDeferredResult = ((Func<Node>) spyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(actualDeferredResult, Is.SameAs(expectedDeferredResult));
		}
	}
}