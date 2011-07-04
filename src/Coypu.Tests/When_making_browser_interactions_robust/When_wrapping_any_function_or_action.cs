using System;
using System.Diagnostics;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_wrapping_any_function_or_action
    {
        [Test]
        public void When_a_Function_throws_a_recurring_exception_It_retries_until_the_timeout_is_reached_then_rethrow()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(200);
            const int retryIntervalMilliseconds = 10;
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(retryIntervalMilliseconds);

            var robustness = new RetryUntilTimeoutRobustWrapper();

            Func<object> function = () => { throw new ExplicitlyThrownTestException("Fails every time"); };

            var stopWatch = Stopwatch.StartNew();
            try
            {
                robustness.Robustly(function);
                Assert.Fail("Expected 'Fails every time' exception");
            }
            catch (ExplicitlyThrownTestException e)
            {
                Assert.That(e.Message, Is.EqualTo("Fails every time"));
            }
            stopWatch.Stop();
            var actualDuration = stopWatch.ElapsedMilliseconds;

            const int tolerance = retryIntervalMilliseconds + When_waiting.ThreadSleepAccuracyMilliseconds;
            Assert.That(actualDuration, Is.InRange(expectedTimeout.Milliseconds - tolerance, 
                                                   expectedTimeout.Milliseconds + tolerance));
        }

        [Test]
        public void When_a_Function_throws_an_exception_first_time_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            var robustness = new RetryUntilTimeoutRobustWrapper();
            var tries = 0;
            var expectedReturnValue = new object();
            Func<object> function = () =>
                                        {
                                            tries++;
                                            if (tries == 1)
                                                throw new Exception("Fails first time");

                                            return expectedReturnValue;
                                        };

            var actualReturnValue = robustness.Robustly(function);

            Assert.That(tries, Is.EqualTo(2));
            Assert.That(actualReturnValue, Is.SameAs(expectedReturnValue));
        }

        [Test]
        public void When_a_Function_throws_a_not_supported_exception_It_does_not_retry()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            var robustness = new RetryUntilTimeoutRobustWrapper();
            var tries = 0;
            Func<object> function = () =>
            {
                tries++;
                throw new NotSupportedException("Fails first time");
            };

            Assert.Throws<NotSupportedException>(() => robustness.Robustly(function));
            Assert.That(tries, Is.EqualTo(1));
        }

        [Test]
        public void When_an_Action_throws_a_recurring_exception_It_retries_until_the_timeout_is_reached_then_rethrows()
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(200);
            const int retryInterval = 10;
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(retryInterval);
            var robustness = new RetryUntilTimeoutRobustWrapper();
            Action action = () => { throw new ExplicitlyThrownTestException("Fails every time"); };

            var stopWatch = Stopwatch.StartNew();
            try
            {
                robustness.Robustly(action);
                Assert.Fail("Expected 'Fails every time' exception");
            }
            catch (ExplicitlyThrownTestException e)
            {
                Assert.That(e.Message, Is.EqualTo("Fails every time"));
            }
            stopWatch.Stop();

            var actualDuration = stopWatch.ElapsedMilliseconds;
            const int tolerance = retryInterval + When_waiting.ThreadSleepAccuracyMilliseconds;

            Assert.That(actualDuration, Is.InRange(expectedTimeout.Milliseconds - tolerance,
                                                   expectedTimeout.Milliseconds + tolerance));
        }

        [Test]
        public void When_an_Action_throws_an_exception_first_time_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            var robustness = new RetryUntilTimeoutRobustWrapper();
            var tries = 0;
            Action action = () =>
                                {
                                    tries++;
                                    if (tries == 1)
                                        throw new ExplicitlyThrownTestException("Fails first time");
                                };

            robustness.Robustly(action);

            Assert.That(tries, Is.EqualTo(2));
        }

        [Test]
        public void When_an_Action_throws_a_not_supported_exception_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            var robustness = new RetryUntilTimeoutRobustWrapper();
            var tries = 0;
            Action action = () =>
            {
                tries++;
                throw new NotSupportedException("Fails first time");
            };

            Assert.Throws<NotSupportedException>(() => robustness.Robustly(action));
            Assert.That(tries, Is.EqualTo(1));
        }

    }
}