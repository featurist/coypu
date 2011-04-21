using System;
using System.Linq;
using NUnit.Framework;

namespace Coypu.UnitTests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding : APITests
	{
		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			var expectedImmediateResult = new Node();
			var expectedDeferredResult = new Node();
			SpyRobustWrapper.AlwaysReturn(expectedImmediateResult);
			Driver.StubButton("Find button robustly", expectedDeferredResult);

			var actualImmediateResult = Session.FindButton("Find button robustly");
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			var actualDeferredResult = ((Func<Node>) SpyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(actualDeferredResult, Is.SameAs(expectedDeferredResult));
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			var expectedImmediateResult = new Node();
			var expectedDeferredResult = new Node();
			SpyRobustWrapper.AlwaysReturn(expectedImmediateResult);
			Driver.StubLink("Find link robustly", expectedDeferredResult);

			var actualImmediateResult = Session.FindLink("Find link robustly");
			Assert.That(actualImmediateResult, Is.SameAs(expectedImmediateResult));

			var actualDeferredResult = ((Func<Node>)SpyRobustWrapper.DeferredFunctions.Single())();
			Assert.That(actualDeferredResult, Is.SameAs(expectedDeferredResult));
		}
	}
}