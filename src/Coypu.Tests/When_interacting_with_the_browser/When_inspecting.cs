using System;
using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting : BrowserInteractionTests
    {
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

            var actualDeferredResult = spyRobustWrapper.DeferredQueries.Cast<Func<bool>>().Single()();
            Assert.That(actualDeferredResult, Is.EqualTo(expectedDeferredResult));
        }
    }
}