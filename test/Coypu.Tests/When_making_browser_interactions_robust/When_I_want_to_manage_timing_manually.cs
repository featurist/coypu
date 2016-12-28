using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;
using Xunit;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class When_I_want_to_manage_timing_manually : BrowserInteractionTests
    {
        [Fact]
        public void RetryUntilTimeout_is_exposed_on_the_session()
        {
            var calledOnWrapper = false;
            browserSession.RetryUntilTimeout(() =>
            {
                calledOnWrapper = true;
            });
            SpyTimingStrategy.QueriesRan<object>().First().Run();
            Assert.True(calledOnWrapper);
        }

        [Fact]
        public void Return_from_RetryUntilTimeout_is_exposed_on_the_session()
        {
            Func<string> function = () => "The expected result";

            SpyTimingStrategy.StubQueryResult(SpyTimingStrategy.NO_EXPECTED_RESULT, "immediate result");

            Assert.Equal("immediate result", browserSession.RetryUntilTimeout(function));

            var query = SpyTimingStrategy.QueriesRan<string>().First();
            var queryResult = query.Run();

            Assert.Equal("The expected result", queryResult);
        }

        [Fact]
        public void TryUntil_is_exposed_on_the_session()
        {
            var tried = false;
            var triedUntil = false;
            Action tryThis = () => tried = true;
            Func<bool> until = () => triedUntil = true;
            var overallTimeout = TimeSpan.FromMilliseconds(1234);

            var options = new Options { Timeout = overallTimeout };
            browserSession.TryUntil(tryThis, until, TimeSpan.Zero, options);

            var tryUntil = SpyTimingStrategy.DeferredTryUntils[0];

            Assert.False(tried);
            tryUntil.TryThisBrowserAction.Act();
            Assert.True(tried);

            Assert.False(triedUntil);
            tryUntil.Until.Run();
            Assert.True(triedUntil);

            Assert.Equal(overallTimeout, tryUntil.OverallTimeout);
        }

        [Fact]
        public void Query_is_exposed_on_the_session()
        {
            Func<string> query = () => "query result";

            SpyTimingStrategy.StubQueryResult("expected query result", "immediate query result");

            Assert.Equal("immediate query result", browserSession.Query(query, "expected query result"));

            var robustQuery = SpyTimingStrategy.QueriesRan<string>().First();
            var queryResult = robustQuery.Run();

            Assert.Equal("query result", queryResult);
        }
    }
}