using System;
using System.Diagnostics;
using Coypu.Timing;
using Xunit;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class When_waiting 
    {
        public const int AccuracyMilliseconds = 30;

        [Fact]
        public void It_sleeps_for_the_expected_time_Case_1()
        {
            It_sleeps_for_the_expected_time_theory(100);
        }

        [Fact]
        public void It_sleeps_for_the_expected_time_Case_2()
        {
            It_sleeps_for_the_expected_time_theory(200);
        }

        private void It_sleeps_for_the_expected_time_theory(int expectedDurationMilliseconds) 
        {
            var waiter = new StopwatchWaiter();
            var stopWatch = Stopwatch.StartNew();
            var expectedDuration = TimeSpan.FromMilliseconds(expectedDurationMilliseconds);

            waiter.Wait(expectedDuration);

            var actualWait = stopWatch.ElapsedMilliseconds;

            const int toleranceMilliseconds = AccuracyMilliseconds;

            Assert.InRange(
                actualWait,
                (expectedDurationMilliseconds - toleranceMilliseconds),
                (expectedDurationMilliseconds + toleranceMilliseconds));
        }

    }
}