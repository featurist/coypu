using System;
using System.Threading;
using Coypu.Drivers;
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
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => true;

            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, Configuration.Timeout);

            Assert.That(tries, Is.EqualTo(1));
        }

        [Test]
        public void When_state_exists_after_three_tries_It_tries_three_times()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => tries >= 3;

            retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, Configuration.Timeout);

            Assert.That(tries, Is.EqualTo(3));
        }

        [Test]
        public void When_state_never_exists_It_fails_after_timeout()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => false;

            Assert.Throws<MissingHtmlException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, Configuration.Timeout));

            Assert.That(tries, Is.GreaterThan(5));
        }

        [Test]
        public void It_applies_seperate_timeouts_to_until()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () =>
                               {
                                   Thread.Sleep(Configuration.Timeout);
                                   throw new MissingHtmlException("Timeout finding until result");
                               };

            Assert.Throws<MissingHtmlException>(() => retryUntilTimeoutRobustWrapper.TryUntil(toTry, until, TimeSpan.FromMilliseconds(10)));

            Assert.That(tries, Is.InRange(5,12));
        }

    }
}