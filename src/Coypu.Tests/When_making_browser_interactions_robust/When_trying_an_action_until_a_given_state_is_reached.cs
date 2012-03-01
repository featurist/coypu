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

        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(200);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();
        }

        [Test]
        public void When_state_exists_It_returns_immediately()
        {
            var toTry = new CountTriesAction();
            var until = new AlwaysSucceedsQuery<bool>(true,TimeSpan.Zero);
            
            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until,TimeSpan.FromMilliseconds(20));

            Assert.That(toTry.Tries, Is.EqualTo(1));
        }

        [Test]
        public void When_state_exists_after_three_tries_It_tries_three_times()
        {
            var toTry = new CountTriesAction();
            var until = new ThrowsThenSubsequentlySucceedsQuery<bool>(true,true,3,TimeSpan.FromMilliseconds(1000));

            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, TimeSpan.FromMilliseconds(100));

            Assert.That(toTry.Tries, Is.EqualTo(3));
        }

        [Test]
        public void When_state_never_exists_It_fails_after_timeout()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var toTry = new CountTriesAction();
            var until = new AlwaysSucceedsQuery<bool>(false,false,TimeSpan.Zero);

            var stopwatch = Stopwatch.StartNew();
            Assert.Throws<MissingHtmlException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, timeout));

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Assert.That(elapsedMilliseconds, Is.InRange(Configuration.Timeout.TotalMilliseconds - (Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds),
                                                        Configuration.Timeout.TotalMilliseconds + (Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));
        }


        [Test]
        public void It_applies_the_retryAfter_timeout_within_until()
        {
            var timeout = TimeSpan.FromMilliseconds(200);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            
            var toTry = new CountTriesAction();
            var retryAfter = TimeSpan.FromMilliseconds(20);
            var until = new AlwaysThrowsQuery<bool,TestException>(retryAfter);

            Assert.Throws<MissingHtmlException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until,timeout));

            Assert.That(toTry.Tries, Is.GreaterThan(1));
            Assert.That(toTry.Tries, Is.LessThan(12));
        }
    }
}