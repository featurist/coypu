using System;
using Coypu.Timing;
using Xunit;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class When_retrying_until_a_timeout
    {
        [Fact]
        public void When_a_query_succeeds_first_time_It_only_tries_once() 
        {
            var expectedResult = new object();
            var alwaysSucceedsQuery = new AlwaysSucceedsQuery<object>(expectedResult,TimeSpan.Zero,TimeSpan.Zero);
            var actualResult = new RetryUntilTimeoutTimingStrategy().Synchronise(alwaysSucceedsQuery);

            Assert.Equal(1, alwaysSucceedsQuery.Tries);
            Assert.Same(expectedResult, actualResult);
        }
        
        [Fact]
        public void When_a_query_throws_an_exception_first_time_It_retries()
        {
            var expectedResult = new object();
            var query = new ThrowsSecondTimeQuery<object>(expectedResult, new Options { Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = TimeSpan.FromMilliseconds(10) });

            var actualReturnValue = new RetryUntilTimeoutTimingStrategy().Synchronise(query);

            Assert.Equal(2, query.Tries);
            Assert.Same(expectedResult, actualReturnValue);
        }

        [Fact]
        public void When_a_query_always_throws_an_exception_It_rethrows_eventually()
        {
            var query = new AlwaysThrowsQuery<object, TestException>(new Options { Timeout = TimeSpan.FromMilliseconds(1000), RetryInterval = TimeSpan.FromMilliseconds(20) });

            Assert.Throws<TestException>(() => new RetryUntilTimeoutTimingStrategy().Synchronise(query));
            Assert.True(query.Tries > 2);
        }

        [Theory]
        [InlineData(1500, 100)]
        [InlineData(300, 70)]
        public void When_a_query_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds)
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            var retryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            var query = new AlwaysThrowsQuery<object, TestException>(new Options { Timeout = expectedTimeout, RetryInterval = retryInterval });

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutTimingStrategy();

            try
            {
                retryUntilTimeoutRobustWrapper.Synchronise(query);
                Assert.True(false, "Expecting test exception");
            }
            catch (TestException){}

            Assert.True(query.LastCall
                > (expectedTimeout.TotalMilliseconds -
                    (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));

            Assert.True(query.LastCall
                < (expectedTimeout.TotalMilliseconds + 
                    (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));

        }
        
        
        [Fact]
        public void When_a_query_throws_a_not_supported_exception_It_does_not_retry()
        {
            var retryInterval = TimeSpan.FromMilliseconds(10);
            
            var robustness = new RetryUntilTimeoutTimingStrategy();

            var query = new AlwaysThrowsQuery<object, NotSupportedException>(new Options{Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = retryInterval});
            Assert.Throws<NotSupportedException>(() => robustness.Synchronise(query));
            Assert.Equal(1, query.Tries);
        }

        [Fact]
        public void When_an_action_succeeds_first_time_It_only_tries_once()
        {
            var alwaysSucceedsQuery = new AlwaysSucceedsQuery<object>(new object(),TimeSpan.Zero,TimeSpan.Zero);
            new RetryUntilTimeoutTimingStrategy().Synchronise(alwaysSucceedsQuery);

            Assert.Equal(1, alwaysSucceedsQuery.Tries);
        }

        [Fact]
        public void When_an_action_throws_an_exception_first_time_It_retries()
        {
            var retryInterval = TimeSpan.FromMilliseconds(10);

            var result = new object();
            var query = new ThrowsSecondTimeQuery<object>(result,new Options{Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = retryInterval});

            new RetryUntilTimeoutTimingStrategy().Synchronise(query);

            Assert.Equal(2, query.Tries);
        }

        [Fact]
        public void When_an_action_always_throws_an_exception_It_rethrows_eventually()
        {
            var query = new AlwaysThrowsQuery<object, TestException>(new Options { Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = TimeSpan.FromMilliseconds(10) });

            Assert.Throws<TestException>(() => new RetryUntilTimeoutTimingStrategy().Synchronise(query));
            Assert.True(query.Tries > 2);
        }

        [Theory]
        [InlineData(1500, 100)]
        [InlineData(300, 70)]
        public void When_an_action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds)
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            var retryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            var query = new AlwaysThrowsQuery<object, TestException>(new Options { Timeout = expectedTimeout, RetryInterval = retryInterval });

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutTimingStrategy();

            try
            {
                retryUntilTimeoutRobustWrapper.Synchronise(query);
                Assert.True(false, "Expecting test exception");
            }
            catch (TestException) { }

            Assert.True(query.LastCall
                > (expectedTimeout.TotalMilliseconds -
                    (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));
            Assert.True(query.LastCall
                < (expectedTimeout.TotalMilliseconds + 
                    (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));
        }


        [Fact]
        public void When_an_action_throws_a_not_supported_exception_It_does_not_retry()
        {
            var retryInterval = TimeSpan.FromMilliseconds(10);

            var robustness = new RetryUntilTimeoutTimingStrategy();

            var query = new AlwaysThrowsQuery<object, NotSupportedException>(new Options { Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = retryInterval });
            Assert.Throws<NotSupportedException>(() => robustness.Synchronise(query));
            Assert.Equal(1, query.Tries);
        }
    }
}