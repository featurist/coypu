using System;
using Coypu.Timing;
using Xunit;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class When_querying_for_an_expected_result
    {
        private RetryUntilTimeoutTimingStrategy _retryUntilTimeoutTimingStrategy;
        private TimeSpan retryInterval;

        public When_querying_for_an_expected_result()
        {
            retryInterval = TimeSpan.FromMilliseconds(10);
            _retryUntilTimeoutTimingStrategy = new RetryUntilTimeoutTimingStrategy();
        }

        [Fact]
        public void When_the_expected_result_is_found_It_returns_the_expected_result_immediately()
        {
            var expectedResult = new object();

            var actualResult = _retryUntilTimeoutTimingStrategy.Synchronise(new AlwaysSucceedsQuery<object>(expectedResult, expectedResult, TimeSpan.FromMilliseconds(200), retryInterval));

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void When_the_expected_result_is_never_found_It_returns_the_actual_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(250);
            const int retryIntervalMS = 70;
            retryInterval = TimeSpan.FromMilliseconds(retryIntervalMS);

            var expectedResult = new object();
            var unexpectedResult = new object();

            var query = new AlwaysSucceedsQuery<object>(unexpectedResult, expectedResult, expectedTimeout, TimeSpan.FromMilliseconds(retryIntervalMS));
            
            var actualResult = _retryUntilTimeoutTimingStrategy.Synchronise(query);

            Assert.Equal(unexpectedResult, actualResult);
            Assert.InRange((int) query.LastCall, 
                (expectedTimeout.Milliseconds - retryIntervalMS),
                (expectedTimeout.Milliseconds + retryIntervalMS));
        }

        [Fact]
        public void When_exceptions_are_always_thrown_It_rethrows_eventually()
        {
            Assert.Throws<TestException>(() => _retryUntilTimeoutTimingStrategy.Synchronise(new AlwaysThrowsQuery<object, TestException>(new Options { Timeout = TimeSpan.FromMilliseconds(200), RetryInterval = retryInterval })));
        }

        [Fact]
        public void When_exceptions_are_thrown_It_retries_And_when_expected_result_found_subsequently_It_returns_expected_result_immediately()
        {
            const int throwsHowManyTimes = 2;
            var expectedResult = new object();
            var query = new ThrowsThenSubsequentlySucceedsQuery<object>(expectedResult, expectedResult, throwsHowManyTimes, new Options { Timeout = TimeSpan.FromMilliseconds(100), RetryInterval = retryInterval });

            Assert.Equal(expectedResult, _retryUntilTimeoutTimingStrategy.Synchronise(query));
            Assert.Equal((throwsHowManyTimes + 1), query.Tries);
        }

        [Fact]
        public void When_a_not_supported_exception_is_thrown_It_does_not_retry()
        {
            var throwsNotSupported = new AlwaysThrowsQuery<object, NotSupportedException>(new Options{Timeout = TimeSpan.FromMilliseconds(200), RetryInterval = retryInterval});

            Assert.Throws<NotSupportedException>(() => _retryUntilTimeoutTimingStrategy.Synchronise(throwsNotSupported));
            Assert.Equal(1, throwsNotSupported.Tries);
        }

        [Fact]
        public void When_exceptions_are_thrown_It_retries_And_when_unexpected_result_found_subsequently_It_keeps_looking_for_expected_result_But_returns_unexpected_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(250);
            const int retryIntervalMS = 70;
            retryInterval = TimeSpan.FromMilliseconds(retryIntervalMS);

            var expectedResult = new object();
            var unexpectedResult = new object();

            var throwsTwiceTimesThenReturnOppositeResult = new ThrowsThenSubsequentlySucceedsQuery<object>(unexpectedResult, expectedResult, 2, new Options{Timeout = expectedTimeout, RetryInterval = retryInterval});

            Assert.Equal(unexpectedResult, _retryUntilTimeoutTimingStrategy.Synchronise(throwsTwiceTimesThenReturnOppositeResult));
            Assert.True(throwsTwiceTimesThenReturnOppositeResult.Tries >= 3);
            Assert.InRange((int) throwsTwiceTimesThenReturnOppositeResult.LastCall, 
                (expectedTimeout.Milliseconds - retryIntervalMS),
                (expectedTimeout.Milliseconds + retryIntervalMS));
        }
    }
}