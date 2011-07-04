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
            Func<object> returnsTrueImmediately = () => expectedResult;

            var actualResult = retryUntilTimeoutRobustWrapper.Query(returnsTrueImmediately, expectedResult);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void When_the_expected_result_is_never_found_It_returns_the_actual_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(200);
            const int retryInterval = 10;
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(retryInterval);

            var expectedResult = new object();
            var unexpectedResult = new object();

            Func<object> returnsUnexpectedResult = () => unexpectedResult;

            var stopWatch = Stopwatch.StartNew();
            var actualResult = retryUntilTimeoutRobustWrapper.Query(returnsUnexpectedResult, expectedResult);
            stopWatch.Stop();

            Assert.That(actualResult, Is.EqualTo(unexpectedResult));

            var actualDuration = stopWatch.ElapsedMilliseconds;
            const int tolerance = retryInterval + When_waiting.ThreadSleepAccuracyMilliseconds;
            Assert.That(actualDuration, Is.InRange(expectedTimeout.Milliseconds - tolerance, 
                                                   expectedTimeout.Milliseconds + tolerance));
        }

        [Test]
        public void When_exceptions_are_always_thrown_It_rethrows_after_timeout()
        {
            Func<bool> alwaysThrows = () => { throw new ExplicitlyThrownTestException("This query always errors"); };

            Assert.Throws<ExplicitlyThrownTestException>(() => retryUntilTimeoutRobustWrapper.Query(alwaysThrows, true));
        }

        [Test]
        public void When_exceptions_are_thrown_It_retries_And_when_expected_result_found_subsequently_It_returns_expected_result_immediately()
        {
            var tries = 0;
            var expectedResult = new object();
            Func<object> throwsFirstTimeThenReturnsExpectedResult =
                () =>
                    {
                        tries++;
                        if (tries < 3)
                        {
                            throw new ExplicitlyThrownTestException("This query always errors");
                        }
                        return expectedResult;
                    };

            Assert.That(retryUntilTimeoutRobustWrapper.Query(throwsFirstTimeThenReturnsExpectedResult, expectedResult), Is.EqualTo(expectedResult));
            Assert.That(tries, Is.EqualTo(3));
        }

        [Test]
        public void When_a_not_supported_exception_is_thrown_It_does_not_retry()
        {
            var tries = 0;
            Func<bool> throwsNotSupported =
                () =>
                {
                    tries++;
                    throw new NotSupportedException("This query always errors");
                };

            Assert.Throws<NotSupportedException>(() => retryUntilTimeoutRobustWrapper.Query(throwsNotSupported, true));
            Assert.That(tries, Is.EqualTo(1));
        }

        [Test]
        public void When_exceptions_are_thrown_It_retries_And_when_unexpected_result_found_subsequently_It_return_unexpected_result_after_timeout()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(250);
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var expectedResult = new object();
            var unexpectedResult = new object();
            var tries = 0;
            Func<object> throwsFirstTimeThenReturnOppositeResult =
                () =>
                    {
                        tries++;
                        if (tries < 3)
                        {
                            throw new ExplicitlyThrownTestException("This query always errors");
                        }
                        return unexpectedResult;
                    };

            Assert.That(retryUntilTimeoutRobustWrapper.Query(throwsFirstTimeThenReturnOppositeResult, expectedResult), Is.EqualTo(unexpectedResult));
            Assert.That(tries, Is.InRange(4,27));
        }
    }
}