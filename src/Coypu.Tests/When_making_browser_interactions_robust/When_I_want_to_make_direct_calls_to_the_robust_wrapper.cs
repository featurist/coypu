using System;
using System.Linq;
using Coypu.Queries;
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
            session.RetryUntilTimeout(() =>
            {
                calledOnWrapper = true;
            });
            spyRobustWrapper.QueriesRan<Query<object>>().First().Run();
            Assert.That(calledOnWrapper, Is.True);
        }

        [Test]
        public void RobustFunction_is_exposed_on_the_session()
        {
            Func<string> function = () => "The expected result";

            spyRobustWrapper.StubQueryResult(SpyRobustWrapper.NO_EXPECTED_RESULT, "immediate result");

            Assert.That(session.RetryUntilTimeout(function), Is.EqualTo("immediate result"));

            var query = spyRobustWrapper.QueriesRan<Query<string>>().First();
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
            var waitBeforeRetry = TimeSpan.FromMilliseconds(1234);

            session.TryUntil(tryThis,until,waitBeforeRetry);

            var tryUntil = spyRobustWrapper.DeferredTryUntils[0];

            Assert.That(tried, Is.False);
            tryUntil.TryThisDriverAction.Act();
            Assert.That(tried, Is.True);

            Assert.That(triedUntil, Is.False);
            tryUntil.UntilThisPredicate.Satisfied();
            Assert.That(triedUntil, Is.True);

            Assert.That(tryUntil.WaitBeforeRetry, Is.EqualTo(waitBeforeRetry));
        }

        [Test]
        public void Query_is_exposed_on_the_session()
        {
            Func<string> query = () => "query result";

            spyRobustWrapper.StubQueryResult("expected query result", "immediate query result");

            Assert.That(session.Query(query, "expected query result"), Is.EqualTo("immediate query result"));

            var robustQuery = spyRobustWrapper.QueriesRan<Query<string>>().First();
            robustQuery.Run();

            Assert.That(robustQuery.Result, Is.EqualTo("query result"));
        }
    }
}