using System;
using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_state : BrowserInteractionTests
    {
        [Test]
        public void It_returns_the_state_that_was_found_first_Example_1()
        {
            var state1 = new State { Condition = () => true };
            var state2 = new State { Condition = () => false };
            var state3 = new State { Condition = () => false };

            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), mockSleepWaiter);

            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state1));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_2()
        {
            var state1 = new State { Condition = () => false };
            var state2 = new State { Condition = () => true };
            var state3 = new State { Condition = () => false };

            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), mockSleepWaiter);

            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state2));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_3()
        {
            var state1 = new State { Condition = () => false };
            var state2 = new State { Condition = () => false };
            var state3 = new State { Condition = () => true };

            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), mockSleepWaiter);

            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state3));
        }

        [Test]
        public void It_uses_a_zero_timeout_when_evaluating_the_conditions()
        {
            var timeout1 = TimeSpan.MaxValue;
            var timeout2 = TimeSpan.MaxValue;
            var state1 = new State
            {
                Condition = () =>
                {
                    timeout1 = Configuration.Timeout;
                    return false;
                }
            };
            var state2 = new State
            {
                Condition = () =>
                {
                    timeout2 = Configuration.Timeout;
                    return true;
                }
            };

            session = new Session(driver, new ImmediateSingleExecutionFakeRobustWrapper(), mockSleepWaiter);

            Configuration.Timeout = TimeSpan.FromSeconds(1);

            session.FindState(state1, state2);

            Assert.That(Configuration.Timeout, Is.EqualTo(TimeSpan.FromSeconds(1)));
            Assert.That(timeout1, Is.EqualTo(TimeSpan.Zero));
            Assert.That(timeout2, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void It_checks_all_of_the_states_in_a_robust_query_expecting_true()
        {
            var queriedState1 = false;
            var queriedState2 = false;
            var state1 = new State
            {
                Condition = () =>
                {
                    queriedState1 = true;
                    return false;
                }
            };
            var state2 = new State
            {
                Condition = () =>
                {
                    queriedState2 = true;
                    return true;
                }
            };
            var state3 = new State { Condition = () => true };
            state3.CheckCondition();

            spyRobustWrapper.StubQueryResult(true,true);

            Assert.That(session.FindState(state1, state2, state3), Is.SameAs(state3));

            Assert.IsFalse(queriedState1);
            Assert.IsFalse(queriedState2);

            var query = ((Func<bool>)spyRobustWrapper.DeferredQueries.Single());
            Assert.IsTrue(query());

            Assert.IsTrue(queriedState1);
            Assert.IsTrue(queriedState2);
        }

        
        [Test]
        public void When_query_returns_false_It_raises_an_exception()
        {
            var state1 = new State { Condition = () => false };
            var state2 = new State { Condition = () => false };

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

    public class ImmediateSingleExecutionFakeRobustWrapper : Robustness.RobustWrapper
    {
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
    }
}