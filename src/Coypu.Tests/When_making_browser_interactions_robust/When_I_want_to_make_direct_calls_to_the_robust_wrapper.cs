using System;
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
            session.Robustly(() =>
            {
                calledOnWrapper = true;
            });
            spyRobustWrapper.DeferredActions[0]();
            Assert.That(calledOnWrapper, Is.True);
        }

        [Test]
        public void RobustFunction_is_exposed_on_the_session()
        {
            spyRobustWrapper.AlwaysReturnFromRobustly(typeof(string), "immediate result");

            Func<string> function = () => "The expected result";
            
            var immediateResult = session.Robustly(function);
            Assert.That(immediateResult, Is.EqualTo("immediate result"));

            var actualResult = ((Func<string>)spyRobustWrapper.DeferredFunctions[0])();

            Assert.That(actualResult, Is.EqualTo("The expected result"));
        }

        [Test]
        public void TryUntil_is_exposed_on_the_session()
        {
            Action tryThis = () => {};
            Func<bool> until = () => true;
            var waitBeforeRetry = TimeSpan.FromMilliseconds(1234);

            session.TryUntil(tryThis,until,waitBeforeRetry);

            var tryUntil = spyRobustWrapper.DeferredTryUntils[0];

            Assert.That(tryUntil.TryThis, Is.SameAs(tryThis));
            Assert.That(tryUntil.Until, Is.SameAs(until));
            Assert.That(tryUntil.WaitBeforeRetry, Is.EqualTo(waitBeforeRetry));
        }

        [Test]
        public void Query_is_exposed_on_the_session()
        {
            spyRobustWrapper.AlwaysReturnFromQuery("expected query result", "expected query result");
            Func<string> query = () => "query result";

            session.Query(query, "expected query result");

            var actualResult = ((Func<string>)spyRobustWrapper.DeferredQueries[0])();

            Assert.That(actualResult, Is.EqualTo("query result"));
        }
    }
}