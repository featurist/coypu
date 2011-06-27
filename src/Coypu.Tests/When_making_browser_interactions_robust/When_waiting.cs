using System;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_waiting 
    {
        [TestCase(100)]
        [TestCase(200)]
        public void It_sleeps_for_the_expected_time(int expectedDurationMilliseconds) 
        {
            var waiter = new ThreadSleepWaiter();
            var start = DateTime.UtcNow;

            var expectedDuration = TimeSpan.FromMilliseconds(expectedDurationMilliseconds);
            waiter.Wait(expectedDuration);

            var actualWait = DateTime.UtcNow - start;

            const int tolleranceMilliseconds = 10;
            Assert.That(actualWait, Is.InRange(expectedDuration, expectedDuration + TimeSpan.FromMilliseconds(tolleranceMilliseconds)));
        }

    }
}