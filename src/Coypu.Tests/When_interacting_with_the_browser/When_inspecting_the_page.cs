using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_inspecting_the_page
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
		public void HasContent_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, session.HasContent, driver.StubHasContent);
		}

		[Test]
		public void HasContent_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, session.HasContent, driver.StubHasContent);
		}

		[Test]
		public void HasNoContent_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, session.HasNoContent, driver.StubHasContent);
		}

		[Test]
		public void HasNoContent_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, session.HasNoContent, driver.StubHasContent);
		}

		[Test]
		public void HasCss_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, session.HasCss, driver.StubHasCss);
		}

		[Test]
		public void HasCss_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, session.HasCss, driver.StubHasCss);
		}

		[Test]
		public void HasNoCss_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, session.HasNoCss, driver.StubHasCss);
		}

		[Test]
		public void HasNoCss_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, session.HasNoCss, driver.StubHasCss);
		}

		[Test]
		public void HasXPath_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, session.HasXPath, driver.StubHasXPath);
		}

		[Test]
		public void HasXPath_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, session.HasXPath, driver.StubHasXPath);
		}

		[Test]
		public void HasNoXPath_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, session.HasNoXPath, driver.StubHasXPath);
		}

		[Test]
		public void HasNoXPath_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, session.HasNoXPath, driver.StubHasXPath);
		}

		protected void Should_wait_for_robustly(bool expecting, bool stubResult, Func<string, bool> subject, Action<string, bool> stub)
		{
			var locator = "Find me " + DateTime.Now.Ticks;

			var expectedImmediateResult = !stubResult;
			var expectedDeferredResult = stubResult;

			spyRobustWrapper.AlwaysReturnFromWaitFor(expecting, expectedImmediateResult);
			stub(locator, expectedDeferredResult);

			var actualImmediateResult = subject(locator);
			Assert.That(actualImmediateResult, Is.Not.EqualTo(expectedDeferredResult), "Result was not found robustly");
			Assert.That(actualImmediateResult, Is.EqualTo(expectedImmediateResult));

			var actualDeferredResult = spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(actualDeferredResult, Is.EqualTo(expectedDeferredResult));
		}
	}
}