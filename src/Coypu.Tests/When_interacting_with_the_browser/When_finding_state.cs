using System;
using System.Linq;
using Coypu.Robustness;
using Coypu.Tests.TestBuilders;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_state : BrowserInteractionTests
    {
        private void BuildSession()
        {
            session = TestSessionBuilder.Build(driver, new ImmediateSingleExecutionFakeRobustWrapper(), fakeWaiter, null,
                                               null);
        }

        [Test]
        public void It_checks_all_of_the_states_in_a_robust_query_expecting_true()
        {
            bool queriedState1 = false;
            bool queriedState2 = false;
            var state1 = new State(() =>
                                       {
                                           queriedState1 = true;
                                           return false;
                                       });
            var state2 = new State(() =>
                                       {
                                           queriedState2 = true;
                                           return true;
                                       });
            var state3 = new State(() => true);
            state3.CheckCondition();

            spyRobustWrapper.StubQueryResult(true, true);

            Assert.That(session.FindState(state1, state2, state3), Is.SameAs(state3));

            Assert.IsFalse(queriedState1);
            Assert.IsFalse(queriedState2);

            var query = ((Func<bool>) spyRobustWrapper.DeferredQueries.Single());
            Assert.IsTrue(query());

            Assert.IsTrue(queriedState1);
            Assert.IsTrue(queriedState2);
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_1()
        {
            var state1 = new State(() => true);
            var state2 = new State(() => false);
            var state3 = new State(() => false);

            BuildSession();

            State foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state1));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_2()
        {
            var state1 = new State(() => false);
            var state2 = new State(() => true);
            var state3 = new State(() => false);

            BuildSession();

            State foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state2));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_3()
        {
            var state1 = new State(() => false);
            var state2 = new State(() => false);
            var state3 = new State(() => true);

            BuildSession();

            State foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state3));
        }

        [Test]
        public void It_uses_a_zero_timeout_when_evaluating_the_conditions()
        {
            TimeSpan timeout1 = TimeSpan.MaxValue;
            TimeSpan timeout2 = TimeSpan.MaxValue;
            var state1 = new State(() =>
                                       {
                                           timeout1 = Configuration.Timeout;
                                           return false;
                                       });
            var state2 = new State(() =>
                                       {
                                           timeout2 = Configuration.Timeout;
                                           return true;
                                       });

            BuildSession();

            Configuration.Timeout = TimeSpan.FromSeconds(1);

            session.FindState(state1, state2);

            Assert.That(Configuration.Timeout, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(timeout1, Is.EqualTo(TimeSpan.Zero));
            Assert.That(timeout2, Is.EqualTo(TimeSpan.Zero));
        }


        [Test]
        public void When_query_returns_false_It_raises_an_exception()
        {
            var state1 = new State(() => false);
            var state2 = new State(() => false);

            spyRobustWrapper.StubQueryResult(true, false);

            try
            {
                session.FindState(state1, state2);
                Assert.Fail("Expected MissingHTMLException");
            }
            catch (MissingHtmlException e)
            {
                Assert.That(e.Message, Is.EqualTo("None of the given states was reached within the configured timeout."));
            }
        }
    }

    public class ImmediateSingleExecutionFakeRobustWrapper : RobustWrapper
    {
        #region RobustWrapper Members

        public void Robustly(Action action)
        {
            action();
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            return function();
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return query();
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            tryThis();
        }

        #endregion
    }
}