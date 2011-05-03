using System;
using Coypu.Drivers;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
	[TestFixture]
    public class When_trying_an_action_until_a_given_state_is_reached
	{
        private WaitAndRetryRobustWrapper waitAndRetryRobustWrapper;

        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(200);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            waitAndRetryRobustWrapper = new WaitAndRetryRobustWrapper();
        }

        [Test]
        public void When_state_exists_it_should_return_immediately()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => true;

            waitAndRetryRobustWrapper.TryUntil(toTry, until);

            Assert.That(tries, Is.EqualTo(1));
        }

        [Test]
        public void When_state_exists_after_three_tries_it_should_try_three_times()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => tries >= 3;

            waitAndRetryRobustWrapper.TryUntil(toTry, until);

            Assert.That(tries, Is.EqualTo(3));
        }

        [Test]
        public void When_state_never_exists_it_should_fail_after_timeout()
        {
            var tries = 0;

            Action toTry = () => tries++;
            Func<bool> until = () => false;

            Assert.Throws<MissingHtmlException>(() => waitAndRetryRobustWrapper.TryUntil(toTry, until));

            Assert.That(tries, Is.GreaterThan(5));
        }

	}
}