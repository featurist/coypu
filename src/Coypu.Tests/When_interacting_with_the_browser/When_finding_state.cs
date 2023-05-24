using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Timing;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_making_browser_interactions_robust;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_state
    {
        [SetUp]
        public void SetUp()
        {
            SessionConfiguration = new SessionConfiguration();
        }

        internal BrowserSession BuildSession(TimingStrategy timingStrategy)
        {
            return TestSessionBuilder.Build(SessionConfiguration,new FakeDriver(), timingStrategy, new FakeWaiter(), null, null);
        }

        private SessionConfiguration SessionConfiguration;

        [Test]
        public void It_checks_all_of_the_states_in_a_robust_query_expecting_true()
        {
            bool queriedState1 = false;
            bool queriedState2 = false;
            var state1 = new State(new LambdaQuery<bool>(() =>
                                       {
                                           queriedState1 = true;
                                           return false;
                                       }));
            var state2 = new State(new LambdaQuery<bool>(() =>
                                       {
                                           queriedState2 = true;
                                           return true;
                                       }));
            var state3 = new State(new LambdaQuery<bool>(() => true));
            state3.CheckCondition();

            var robustWrapper = new SpyTimingStrategy();
            robustWrapper.StubQueryResult(true, true);

            var session = BuildSession(robustWrapper);

            Assert.That(session.FindState(new [] {state1, state2, state3}), Is.SameAs(state3));

            Assert.IsFalse(queriedState1);
            Assert.IsFalse(queriedState2);

            var query = robustWrapper.QueriesRan<bool>().Single();
            var queryResult = query.Run();
            Assert.IsTrue(queryResult);

            Assert.IsTrue(queriedState1);
            Assert.IsTrue(queriedState2);
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_1()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            
            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state1));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_2()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));

            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state2));
        }

        [Test]
        public void It_returns_the_state_that_was_found_first_Example_3()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));

            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.That(foundState, Is.SameAs(state3));
        }

        [Test]
        public void It_uses_a_zero_timeout_when_evaluating_the_conditions()
        {
            var robustWrapper = new ImmediateSingleExecutionFakeTimingStrategy();
            var zeroTimeout1 = false;
            var zeroTimeout2 = false;
            var state1 = new State(new LambdaQuery<bool>(() =>
                                                             {
                                                                 zeroTimeout1 = robustWrapper.ZeroTimeout;
                                                                 return false;
                                                             }));
            var state2 = new State(new LambdaQuery<bool>(() =>
                                                             {
                                                                 zeroTimeout2 = robustWrapper.ZeroTimeout;
                                                                 return true;
                                                             }));

            var session = BuildSession(robustWrapper);
            session.FindState(state1, state2);

            Assert.That(zeroTimeout1, Is.EqualTo(true));
            Assert.That(zeroTimeout2, Is.EqualTo(true));

            Assert.That(robustWrapper.ZeroTimeout, Is.EqualTo(false));
        }


        [Test]
        public void When_query_returns_false_It_raises_an_exception()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));

            var robustWrapper = new SpyTimingStrategy();
            robustWrapper.StubQueryResult(true, false);
            
            var session = BuildSession(robustWrapper);

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
}