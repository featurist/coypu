using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	public class When_inspecting
	{
		protected FakeDriver Driver;
		protected Session Session;
		protected SpyRobustWrapper SpyRobustWrapper;

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Session(Driver, SpyRobustWrapper);
		}

		protected void Should_wait_for_robustly(bool expecting, bool stubResult, Func<string, bool> subject, Action<string, bool> stub)
		{
			var locator = "Find me " + DateTime.Now.Ticks;

			var expectedImmediateResult = !stubResult;
			var expectedDeferredResult = stubResult;

			SpyRobustWrapper.AlwaysReturnFromWaitFor(expecting, expectedImmediateResult);
			stub(locator, expectedDeferredResult);

			var actualImmediateResult = subject(locator);
			Assert.That(actualImmediateResult, Is.Not.EqualTo(expectedDeferredResult), "Result was not found robustly");
			Assert.That(actualImmediateResult, Is.EqualTo(expectedImmediateResult));

			var actualDeferredResult = SpyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(actualDeferredResult, Is.EqualTo(expectedDeferredResult));
		}
	}
}