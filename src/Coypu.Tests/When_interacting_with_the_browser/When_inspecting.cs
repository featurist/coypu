using System;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting : BrowserInteractionTests
    {
        protected void Queries_robustly(bool stubResult, Func<string, bool> subject, Action<string, bool> stub)
        {
            Queries_robustly(stubResult, subject, stub, "Find me " + DateTime.Now.Ticks);
        }

        protected void Queries_robustly_reversing_result(bool stubResult, Func<string, bool> subject, Action<string, bool> stub)
        {
            Queries_robustly_reversing_result(stubResult, subject, stub, "Find me " + DateTime.Now.Ticks);
        }

        protected void Queries_robustly<T>(bool stubResult, Func<T, bool> subject, Action<T, bool> stub, T locator)
        {
            stub(locator, stubResult);
            spyRobustWrapper.StubQueryResult(true, !stubResult);

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);
            browserSession.WithTimeout(individualTimeout);

            var actualImmediateResult = subject(locator);

            Assert.That(actualImmediateResult, Is.EqualTo(!stubResult), "Result was not found robustly");

            RunQueryAndCheckTiming<bool>(individualTimeout);

            Assert.That(queryResult, Is.EqualTo(stubResult));
        }

        protected void Queries_robustly_reversing_result<T>(bool stubResult, Func<T, bool> subject, Action<T, bool> stub, T locator)
        {
            stub(locator, stubResult);
            spyRobustWrapper.StubQueryResult(true, !stubResult);

            var actualImmediateResult = subject(locator);

            Assert.That(actualImmediateResult, Is.EqualTo(!stubResult), "Result was not found robustly");

            RunQueryAndCheckTiming<bool>();

            Assert.That(queryResult, Is.EqualTo(!stubResult));
        }
    }
}