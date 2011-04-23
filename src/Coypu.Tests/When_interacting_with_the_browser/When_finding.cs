using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding
	{
		protected FakeDriver Driver;
		protected SpyRobustWrapper SpyRobustWrapper;
		protected Session Session;

		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindButton, Driver.StubButton);
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindLink, Driver.StubLink);
		}

		[Test]
		public void FindField_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindField, Driver.StubField);
		}

		protected void Should_find_robustly(Func<string, Node> subject, Action<string, Node> stub)
		{
			var expectedImmediateResult = new StubNode();
			var expectedDeferredResult = new StubNode();

			SpyRobustWrapper.AlwaysReturn(typeof(Node), expectedImmediateResult);
			stub("Find me", expectedDeferredResult);

			var actualImmediateResult = subject("Find me");
			Assert.That(actualImmediateResult, Is.Not.SameAs(expectedDeferredResult), "Result was not found robustly");
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			var actualDeferredResult = ((Func<Node>) SpyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(actualDeferredResult, Is.SameAs(expectedDeferredResult));
		}

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Session(Driver, SpyRobustWrapper);
		}
	}
}