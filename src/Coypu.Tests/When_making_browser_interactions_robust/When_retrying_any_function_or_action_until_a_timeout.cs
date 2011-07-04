using System;
using System.Diagnostics;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_retrying_any_function_or_action_until_a_timeout
    {
        [Test]
        public void When_a_Function_succeeds_first_time_It_only_tries_once() 
        {
            var retries = 0;
            var expectedReturnValue = new object();
            Func<object> alwaysSucceeds = () =>
            {
                retries++;
                return expectedReturnValue;
            };

            var actualResult = new RetryUntilTimeoutRobustWrapper().Robustly(alwaysSucceeds);

            Assert.That(retries, Is.EqualTo(1));
            Assert.That(actualResult, Is.SameAs(expectedReturnValue));
        }

        [Test]
        public void When_a_Function_throws_an_exception_first_time_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var tries = 0;
            var expectedReturnValue = new object();
            Func<object> throwsOnce = () =>
                                        {
                                            tries++;
                                            if (tries == 1)
                                                throw new Exception("Fails first time");

                                            return expectedReturnValue;
                                        };

            var actualReturnValue = new RetryUntilTimeoutRobustWrapper().Robustly(throwsOnce);

            Assert.That(tries, Is.EqualTo(2));
            Assert.That(actualReturnValue, Is.SameAs(expectedReturnValue));
        }

        [Test]
        public void When_a_Function_always_throws_an_exception_It_rethrows_eventually()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var retries = 0;
            Func<object> alwaysThrows = () =>
                                            {
                                                retries++;
                                                throw new ExplicitlyThrownTestException("Function fails every time");
                                            };
            Assert.Throws<ExplicitlyThrownTestException>(() => new RetryUntilTimeoutRobustWrapper().Robustly(alwaysThrows));
            Assert.That(retries, Is.GreaterThan(2));
        }

        [Test]
        public void When_a_Function_always_throws_an_exception_It_retries_until_the_timeout_is_reached()
        {
            When_a_Function_always_throws_an_exception_It_retries_until_the_timeout_is_reached(150, 100);
            When_a_Function_always_throws_an_exception_It_retries_until_the_timeout_is_reached(300, 70);
        }

        private void When_a_Function_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds)
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            Stopwatch stopWatch = null;
            long lastCall = 0;
            Func<object> alwaysThrows = () =>
                                            {
                                                lastCall = stopWatch.ElapsedMilliseconds;
                                                throw new ExplicitlyThrownTestException("Function fails every time");
                                            };

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();
            
            try {
                stopWatch = Stopwatch.StartNew();
                retryUntilTimeoutRobustWrapper.Robustly(alwaysThrows);
            }
            catch (ExplicitlyThrownTestException) { stopWatch.Stop(); }

            Assert.That(lastCall, Is.InRange(expectedTimeout.TotalMilliseconds - Configuration.RetryInterval.Milliseconds,
                                             expectedTimeout.TotalMilliseconds + Configuration.RetryInterval.Milliseconds));
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
        public void When_a_Action_succeeds_first_time_It_only_tries_once() 
        {
            var retries = 0;
            Action alwaysSucceeds = () => { retries++;};

            new RetryUntilTimeoutRobustWrapper().Robustly(alwaysSucceeds);

            Assert.That(retries, Is.EqualTo(1));
        }

        [Test]
        public void When_a_Action_throws_an_exception_first_time_It_retries() 
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var tries = 0;
            Action throwsOnce = () =>
            {
                tries++;
                if (tries == 1)
                    throw new Exception("Fails first time");
            };

            new RetryUntilTimeoutRobustWrapper().Robustly(throwsOnce);

            Assert.That(tries, Is.EqualTo(2));
        }

        [Test]
        public void When_a_Action_always_throws_an_exception_It_rethrows_eventually()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var retries = 0;
            Action alwaysThrows = () =>
            {
                retries++;
                throw new ExplicitlyThrownTestException("Action fails every time");
            };
            Assert.Throws<ExplicitlyThrownTestException>(() => new RetryUntilTimeoutRobustWrapper().Robustly(alwaysThrows));
            Assert.That(retries, Is.GreaterThan(2));
        }

        [Test]
        public void When_a_Action_always_throws_an_exception_It_retries_until_the_timeout_is_reached() {
            When_a_Action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(150, 100);
            When_a_Action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(300, 70);
        }

        private void When_a_Action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds) 
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            Stopwatch stopWatch = null;
            long lastCall = 0;
            
            Action alwaysThrows = () =>
            {
                lastCall = stopWatch.ElapsedMilliseconds;
                throw new ExplicitlyThrownTestException("Action fails every time");
            };

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();

            try {
                stopWatch = Stopwatch.StartNew();
                retryUntilTimeoutRobustWrapper.Robustly(alwaysThrows);
            } 
            catch (ExplicitlyThrownTestException) { stopWatch.Stop(); }

            Assert.That(lastCall, Is.InRange(expectedTimeout.TotalMilliseconds - Configuration.RetryInterval.Milliseconds,
                                             expectedTimeout.TotalMilliseconds + Configuration.RetryInterval.Milliseconds));
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