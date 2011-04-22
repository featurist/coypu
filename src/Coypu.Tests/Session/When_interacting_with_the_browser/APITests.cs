using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using Coypu.UnitTests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.Session.When_interacting_with_the_browser
{
	public class APITests
	{
		protected FakeDriver Driver;
		protected SpyRobustWrapper SpyRobustWrapper;
		protected Coypu.Session Session;

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Coypu.Session(Driver, SpyRobustWrapper);
		}

		protected void Should_find_robustly(Func<string, Node> subject, Action<string, Node> stub)
		{
			var expectedImmediateResult = new TestNode();
			var expectedDeferredResult = new TestNode();

			SpyRobustWrapper.AlwaysReturn(typeof(Node), expectedImmediateResult);
			stub("Find me", expectedDeferredResult);

			var actualImmediateResult = subject("Find me");
			Assert.That(actualImmediateResult, Is.Not.SameAs(expectedDeferredResult), "Result was not found robustly");
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			var actualDeferredResult = ((Func<Node>) SpyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(actualDeferredResult, Is.SameAs(expectedDeferredResult));
		}
	}
}