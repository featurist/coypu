using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_interacting_with_the_browser;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_I_want_to_make_direct_calls_to_the_robust_wrapper : BrowserInteractionTests
    {
        [Test]
        public void RobustAction_is_exposed_on_the_session()
        {
            var calledOnWrapper = false;
            browserSession.RetryUntilTimeout(() =>
            {
                calledOnWrapper = true;
            });
            spyRobustWrapper.QueriesRan<object>().First().Run();
            Assert.That(calledOnWrapper, Is.True);
        }

        [Test]
        public void RobustFunction_is_exposed_on_the_session()
        {
            Func<string> function = () => "The expected result";

            spyRobustWrapper.StubQueryResult(SpyRobustWrapper.NO_EXPECTED_RESULT, "immediate result");

            Assert.That(browserSession.RetryUntilTimeout(function), Is.EqualTo("immediate result"));

            var query = spyRobustWrapper.QueriesRan<string>().First();
            query.Run();

            Assert.That(query.Result, Is.EqualTo("The expected result"));
        }

        [Test]
        public void TryUntil_is_exposed_on_the_session()
        {
            var tried = false;
            var triedUntil = false;
            Action tryThis = () => tried = true;
            Func<bool> until = () => triedUntil = true;
            var overallTimeout = TimeSpan.FromMilliseconds(1234);

            var options = new Options { Timeout = overallTimeout };
            browserSession.TryUntil(tryThis, until,TimeSpan.Zero,options);

            var tryUntil = spyRobustWrapper.DeferredTryUntils[0];

            Assert.That(tried, Is.False);
            tryUntil.TryThisBrowserAction.Act();
            Assert.That(tried, Is.True);

            Assert.That(triedUntil, Is.False);
            tryUntil.Until.Run();
            Assert.That(triedUntil, Is.True);

            Assert.That(tryUntil.OverallTimeout, Is.EqualTo(overallTimeout));
        }

        [Test]
        public void Query_is_exposed_on_the_session()
        {
            Func<string> query = () => "query result";

            spyRobustWrapper.StubQueryResult("expected query result", "immediate query result");

            Assert.That(browserSession.Query(query, "expected query result"), Is.EqualTo("immediate query result"));

            var robustQuery = spyRobustWrapper.QueriesRan<string>().First();
            robustQuery.Run();

            Assert.That(robustQuery.Result, Is.EqualTo("query result"));
        }
    }
}