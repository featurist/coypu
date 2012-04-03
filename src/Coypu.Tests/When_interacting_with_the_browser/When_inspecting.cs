using System;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting : BrowserInteractionTests
    {
        protected void Queries_robustly(bool stubResult, Func<string, Options, bool> subject, Action<string, bool, DriverScope> stub)
        {
            Queries_robustly(stubResult, subject, stub, "Find me " + DateTime.Now.Ticks);
        }

        protected void Queries_robustly_reversing_result(bool stubResult, Func<string, Options, bool> subject, Action<string, bool, DriverScope> stub)
        {
            Queries_robustly_reversing_result(stubResult, subject, stub, "Find me " + DateTime.Now.Ticks);
        }

        protected void Queries_robustly<T>(bool stubResult, Func<T, Options, bool> subject, Action<T, bool, DriverScope> stub, T locator)
        {
            stub(locator, stubResult, browserSession);
            spyRobustWrapper.StubQueryResult(true, !stubResult);

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            var options = new SessionConfiguration {Timeout = individualTimeout};

            var actualImmediateResult = subject(locator, options);

            Assert.That(actualImmediateResult, Is.EqualTo(!stubResult), "Result was not found robustly");

            RunQueryAndCheckTiming<bool>(individualTimeout);

            Assert.That(queryResult, Is.EqualTo(stubResult));
        }

        protected void Queries_robustly_reversing_result<T>(bool stubResult, Func<T, Options, bool> subject, Action<T, bool, DriverScope> stub, T locator)
        {
            stub(locator, stubResult, browserSession);
            spyRobustWrapper.StubQueryResult(true, !stubResult);

            var actualImmediateResult = subject(locator,SessionConfiguration);

            Assert.That(actualImmediateResult, Is.EqualTo(!stubResult), "Result was not found robustly");

            RunQueryAndCheckTiming<bool>();

            Assert.That(queryResult, Is.EqualTo(!stubResult));
        }
    }
}