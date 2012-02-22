using System;
using System.Diagnostics;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_querying_for_an_expected_result
    {
        private RetryUntilTimeoutRobustWrapper retryUntilTimeoutRobustWrapper;

        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(200);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();
        }

        [Test]
        public void When_the_expected_result_is_found_It_returns_the_expected_result_immediately()
        {
            var expectedResult = new object();

            var actualResult = retryUntilTimeoutRobustWrapper.Query(new AlwaysSucceedsQuery<object>(expectedResult, expectedResult));

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void When_the_expected_result_is_never_found_It_returns_the_actual_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(250);
            const int retryInterval = 70;
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(retryInterval);

            var expectedResult = new object();
            var unexpectedResult = new object();

            var query = new AlwaysSucceedsQuery<object>(unexpectedResult, expectedResult);
            
            var actualResult = retryUntilTimeoutRobustWrapper.Query(query);

            Assert.That(actualResult, Is.EqualTo(unexpectedResult));
            Assert.That(query.LastCall, Is.InRange(expectedTimeout.Milliseconds - retryInterval,
                                             expectedTimeout.Milliseconds + retryInterval));
        }

        [Test]
        public void When_exceptions_are_always_thrown_It_rethrows_eventually()
        {
            Assert.Throws<TestException>(() => retryUntilTimeoutRobustWrapper.Query(new AlwaysThrowsQuery<TestException>()));
        }

        [Test]
        public void When_exceptions_are_thrown_It_retries_And_when_expected_result_found_subsequently_It_returns_expected_result_immediately()
        {
            const int throwsHowManyTimes = 2;
            var expectedResult = new object();
            var query = new ThrowsThenSubsequentlySucceedsQuery<object>(expectedResult, expectedResult, throwsHowManyTimes);

            Assert.That(retryUntilTimeoutRobustWrapper.Query(query), Is.EqualTo(expectedResult));
            Assert.That(query.Tries, Is.EqualTo(throwsHowManyTimes + 1));
        }

        [Test]
        public void When_a_not_supported_exception_is_thrown_It_does_not_retry()
        {
            var throwsNotSupported = new AlwaysThrowsQuery<NotSupportedException>();

            Assert.Throws<NotSupportedException>(() => retryUntilTimeoutRobustWrapper.Query(throwsNotSupported));
            Assert.That(throwsNotSupported.Tries, Is.EqualTo(1));
        }

        [Test]
        public void When_exceptions_are_thrown_It_retries_And_when_unexpected_result_found_subsequently_It_keeps_looking_for_expected_result_But_returns_unexpected_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(250);
            const int retryInterval = 70;
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(retryInterval);

            var expectedResult = new object();
            var unexpectedResult = new object();

            var throwsTwiceTimesThenReturnOppositeResult = new ThrowsThenSubsequentlySucceedsQuery<object>(unexpectedResult, expectedResult, 2);

            Assert.That(retryUntilTimeoutRobustWrapper.Query(throwsTwiceTimesThenReturnOppositeResult), Is.EqualTo(unexpectedResult));
            Assert.That(throwsTwiceTimesThenReturnOppositeResult.Tries, Is.GreaterThanOrEqualTo(3));
            Assert.That(throwsTwiceTimesThenReturnOppositeResult.LastCall, Is.InRange(expectedTimeout.Milliseconds - retryInterval,
                                                                                      expectedTimeout.Milliseconds));
        }
    }
}