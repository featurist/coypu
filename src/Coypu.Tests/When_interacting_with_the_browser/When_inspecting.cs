using System;
using System.Linq;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting : BrowserInteractionTests
    {
        protected void Should_wait_for_robustly(bool expecting, bool stubResult, Func<string, bool> subject, Action<string, bool> stub)
        {
          Should_wait_for_robustly<string>(expecting,  stubResult, subject, stub, "Find me " + DateTime.Now.Ticks);
        }
        
        protected void Should_wait_for_robustly<T>(bool expecting, bool stubResult, Func<T, bool> subject, Action<T, bool> stub, T locator)
        {
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