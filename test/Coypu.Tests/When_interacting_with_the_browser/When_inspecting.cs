using System;
using Xunit;

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
            SpyTimingStrategy.StubQueryResult(true, !stubResult);

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);

            var options = new SessionConfiguration {Timeout = individualTimeout};

            var actualImmediateResult = subject(locator, options);

            Assert.Equal(!stubResult, actualImmediateResult);

            var queryResult = RunQueryAndCheckTiming<bool>(individualTimeout);

            Assert.Equal(stubResult, queryResult);
        }

        protected void Queries_robustly_reversing_result<T>(bool stubResult, Func<T, Options, bool> subject, Action<T, bool, DriverScope> stub, T locator)
        {
            stub(locator, stubResult, browserSession);
            SpyTimingStrategy.StubQueryResult(true, !stubResult);

            var actualImmediateResult = subject(locator,sessionConfiguration);

            Assert.Equal(!stubResult, actualImmediateResult);

            var queryResult = RunQueryAndCheckTiming<bool>();

            Assert.Equal(!stubResult, queryResult);
        }
    }
}