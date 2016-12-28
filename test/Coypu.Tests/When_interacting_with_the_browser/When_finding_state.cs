using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Timing;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_making_browser_interactions_robust;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_finding_state
    {

        internal BrowserSession BuildSession(TimingStrategy timingStrategy)
        {
            SessionConfiguration = new SessionConfiguration();
            return TestSessionBuilder.Build(SessionConfiguration,new FakeDriver(), timingStrategy, new FakeWaiter(), null, null);
        }

        private SessionConfiguration SessionConfiguration;

        [Fact]
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

            Assert.Same(state3, session.FindState(new [] {state1, state2, state3}));

            Assert.False(queriedState1);
            Assert.False(queriedState2);

            var query = robustWrapper.QueriesRan<bool>().Single();
            var queryResult = query.Run();
            Assert.True(queryResult);

            Assert.True(queriedState1);
            Assert.True(queriedState2);
        }

        [Fact]
        public void It_returns_the_state_that_was_found_first_Example_1()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            
            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.Same(state1, foundState);
        }

        [Fact]
        public void It_returns_the_state_that_was_found_first_Example_2()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));

            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.Same(state2, foundState);
        }

        [Fact]
        public void It_returns_the_state_that_was_found_first_Example_3()
        {
            var state1 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state2 = new State(new AlwaysSucceedsQuery<bool>(false, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));
            var state3 = new State(new AlwaysSucceedsQuery<bool>(true, true, TimeSpan.Zero, SessionConfiguration.RetryInterval));

            var session = BuildSession(new ImmediateSingleExecutionFakeTimingStrategy());
            var foundState = session.FindState(state1, state2, state3);

            Assert.Same(state3, foundState);
        }

        [Fact]
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

            Assert.True(zeroTimeout1);
            Assert.True(zeroTimeout2);

            Assert.False(robustWrapper.ZeroTimeout);
        }


        [Fact]
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
                Assert.True(false, "Expected MissingHTMLException");
            }
            catch (MissingHtmlException e)
            {
                Assert.Equal("None of the given states was reached within the configured timeout.", e.Message);
            }
        }
    }
}