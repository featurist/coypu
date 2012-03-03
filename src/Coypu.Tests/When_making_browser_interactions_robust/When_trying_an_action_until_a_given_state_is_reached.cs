using System;
using System.Diagnostics;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_trying_an_action_until_a_given_state_is_reached
    {
        private RetryUntilTimeoutRobustWrapper retryUntilTimeoutRobustWrapper;
        private TimeSpan retryInterval;
        private TimeSpan timeout;

        [SetUp]
        public void SetUp()
        {
            timeout = TimeSpan.FromMilliseconds(200);
            retryInterval = TimeSpan.FromMilliseconds(10);
            retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();
        }

        [Test]
        public void When_state_exists_It_returns_immediately()
        {
            var toTry = new CountTriesAction(timeout,retryInterval);
            var until = new AlwaysSucceedsQuery<bool>(true,TimeSpan.Zero,retryInterval);
            
            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until,TimeSpan.FromMilliseconds(20), retryInterval);

            Assert.That(toTry.Tries, Is.EqualTo(1));
        }

        [Test]
        public void When_state_exists_after_three_tries_It_tries_three_times()
        {
            var toTry = new CountTriesAction(timeout,retryInterval);
            var until = new ThrowsThenSubsequentlySucceedsQuery<bool>(true, true, 2, TimeSpan.FromMilliseconds(1000), retryInterval);

            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, TimeSpan.FromMilliseconds(100), retryInterval);

            Assert.That(toTry.Tries, Is.EqualTo(3));
        }

        [Test]
        public void When_state_never_exists_It_fails_after_timeout()
        {
            timeout = TimeSpan.FromMilliseconds(200);
            retryInterval = TimeSpan.FromMilliseconds(10);

            var toTry = new CountTriesAction(timeout,retryInterval);
            var until = new AlwaysSucceedsQuery<bool>(false, false, TimeSpan.Zero, retryInterval);

            var stopwatch = Stopwatch.StartNew();
            Assert.Throws<MissingHtmlException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, timeout, retryInterval));

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Assert.That(elapsedMilliseconds, Is.InRange(timeout.TotalMilliseconds - (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds),
                                                        timeout.TotalMilliseconds + (retryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));
        }


        [Test]
        public void It_applies_the_retryAfter_timeout_within_until()
        {
            timeout = TimeSpan.FromMilliseconds(200);
            retryInterval = TimeSpan.FromMilliseconds(10);
            
            var toTry = new CountTriesAction(timeout,retryInterval);
            var retryAfter = TimeSpan.FromMilliseconds(20);
            var until = new AlwaysThrowsQuery<bool, TestException>(timeout, retryAfter);

            Assert.Throws<TestException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, timeout, retryInterval));

            Assert.That(toTry.Tries, Is.GreaterThan(1));
            Assert.That(toTry.Tries, Is.LessThan(12));
        }
    }
}