using System;
using System.Diagnostics;
using Coypu.Timing;
using Xunit;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class When_trying_an_action_until_a_given_state_is_reached
    {
        private RetryUntilTimeoutTimingStrategy _retryUntilTimeoutTimingStrategy;
        private Options options;

        public When_trying_an_action_until_a_given_state_is_reached()
        {
            options = new Options {Timeout = TimeSpan.FromMilliseconds(200), RetryInterval = TimeSpan.FromMilliseconds(10)};
            _retryUntilTimeoutTimingStrategy = new RetryUntilTimeoutTimingStrategy();
        }

        [Fact]
        public void When_state_exists_It_returns_immediately()
        {
            var toTry = new CountTriesAction(options);
            var retryInterval1 = TimeSpan.FromMilliseconds(10);
            var until = new AlwaysSucceedsPredicateQuery(true, TimeSpan.Zero, retryInterval1);
            
            _retryUntilTimeoutTimingStrategy.TryUntil(toTry, until ,new Options{Timeout = TimeSpan.FromMilliseconds(20)});

            Assert.Equal(1, toTry.Tries);
        }

        [Fact]
        public void When_state_exists_after_three_tries_It_tries_three_times()
        {
            options.RetryInterval = TimeSpan.FromMilliseconds(1000);
            var toTry = new CountTriesAction(options);

            var until = new ThrowsThenSubsequentlySucceedsPredicateQuery(true, true, 2, new Options { Timeout = TimeSpan.FromMilliseconds(1000), RetryInterval = options.RetryInterval });

            _retryUntilTimeoutTimingStrategy.TryUntil(toTry, until, new Options{Timeout = TimeSpan.FromMilliseconds(200)});

            Assert.Equal(3, toTry.Tries);
        }

        [Fact]
        public void When_state_never_exists_It_fails_after_timeout()
        {
            var toTry = new CountTriesAction(options);
            var until = new AlwaysSucceedsPredicateQuery(false, TimeSpan.Zero,  options.RetryInterval);

            var stopwatch = Stopwatch.StartNew();
            var timeout1 = TimeSpan.FromMilliseconds(200);
            Assert.Throws<MissingHtmlException>(() => _retryUntilTimeoutTimingStrategy.TryUntil(toTry, until, new Options{Timeout = timeout1}));

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Assert.True(elapsedMilliseconds 
                > (timeout1.TotalMilliseconds -
                    (options.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));

            Assert.True(elapsedMilliseconds
                < (timeout1.TotalMilliseconds + 
                    (options.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds)));
        }

        [Fact]
        public void When_state_never_exists_It_fails_after_timeout_waiting_before_retry()
        {
            var toTry = new CountTriesAction(options);
            var until = new ThrowsThenSubsequentlySucceedsPredicateQuery(true, true, 2, new Options { Timeout = TimeSpan.FromMilliseconds(250), RetryInterval = TimeSpan.FromMilliseconds(200) });

            var timeout1 = TimeSpan.FromMilliseconds(200);
            Assert.Throws<TestException>(() => _retryUntilTimeoutTimingStrategy.TryUntil(toTry, until, new Options { Timeout = timeout1 }));
        }

        [Fact]
        public void It_applies_the_retryAfter_timeout_within_until()
        {
            var toTry = new CountTriesAction(options);
            var retryAfter = TimeSpan.FromMilliseconds(20);
            var until = new AlwaysThrowsPredicateQuery<TestException>(TimeSpan.Zero, retryAfter);

            Assert.Throws<TestException>(() => _retryUntilTimeoutTimingStrategy.TryUntil(toTry, until, new Options{Timeout = options.Timeout}));

            Assert.InRange(toTry.Tries, 2, 11);
        }
    }
}