using System;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_retrying_until_a_timeout
    {
        [Test]
        public void When_a_query_succeeds_first_time_It_only_tries_once() 
        {
            var alwaysSucceedsQuery = new AlwaysSucceedsQuery<object>(new object());
            var actualResult = new RetryUntilTimeoutRobustWrapper().Query(alwaysSucceedsQuery);

            Assert.That(alwaysSucceedsQuery.Tries, Is.EqualTo(1));
            Assert.That(actualResult, Is.SameAs(alwaysSucceedsQuery.Result));
        }
        
        [Test]
        public void When_a_query_throws_an_exception_first_time_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var result = new object();
            var query = new ThrowsSecondTimeQuery<object>(result);

            var actualReturnValue = new RetryUntilTimeoutRobustWrapper().Query(query);

            Assert.That(query.Tries, Is.EqualTo(2));
            Assert.That(actualReturnValue, Is.SameAs(query.Result));
        }

        [Test]
        public void When_a_query_always_throws_an_exception_It_rethrows_eventually()
        {
            var query = new AlwaysThrowsQuery<TestException>();

            Assert.Throws<TestException>(() => new RetryUntilTimeoutRobustWrapper().Query(query));
            Assert.That(query.Tries, Is.GreaterThan(2));
        }

        [Test]
        public void When_a_query_always_throws_an_exception_It_retries_until_the_timeout_is_reached()
        {
            When_a_query_always_throws_an_exception_It_retries_until_the_timeout_is_reached(1500, 100);
            When_a_query_always_throws_an_exception_It_retries_until_the_timeout_is_reached(300, 70);
        }

        private void When_a_query_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds)
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            var query = new AlwaysThrowsQuery<TestException>();

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();

            try
            {
                retryUntilTimeoutRobustWrapper.Query(query);
                Assert.Fail("Expecting test exception");
            }
            catch (TestException){}

            Assert.That(query.LastCall, Is.InRange(expectedTimeout.TotalMilliseconds - (Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds),
                                                   expectedTimeout.TotalMilliseconds + Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds));
        }
        
        
        [Test]
        public void When_a_query_throws_a_not_supported_exception_It_does_not_retry()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
            
            var robustness = new RetryUntilTimeoutRobustWrapper();

            var query = new AlwaysThrowsQuery< NotSupportedException>();
            Assert.Throws<NotSupportedException>(() => robustness.Query(query));
            Assert.That(query.Tries, Is.EqualTo(1));
        }

        [Test]
        public void When_an_action_succeeds_first_time_It_only_tries_once()
        {
            var alwaysSucceedsQuery = new AlwaysSucceedsQuery<object>(new object());
            new RetryUntilTimeoutRobustWrapper().Query(new TestDriverAction(alwaysSucceedsQuery));

            Assert.That(alwaysSucceedsQuery.Tries, Is.EqualTo(1));
        }

        [Test]
        public void When_an_action_throws_an_exception_first_time_It_retries()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var result = new object();
            var query = new ThrowsSecondTimeQuery<object>(result);

            new RetryUntilTimeoutRobustWrapper().Query(new TestDriverAction(query));

            Assert.That(query.Tries, Is.EqualTo(2));
        }

        [Test]
        public void When_an_action_always_throws_an_exception_It_rethrows_eventually()
        {
            var query = new AlwaysThrowsQuery<TestException>();

            Assert.Throws<TestException>(() => new RetryUntilTimeoutRobustWrapper().Query(new TestDriverAction(query)));
            Assert.That(query.Tries, Is.GreaterThan(2));
        }

        [Test]
        public void When_an_action_always_throws_an_exception_It_retries_until_the_timeout_is_reached()
        {
            When_an_action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(1500, 100);
            When_an_action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(300, 70);
        }

        private void When_an_action_always_throws_an_exception_It_retries_until_the_timeout_is_reached(int timeoutMilliseconds, int intervalMilliseconds)
        {
            var expectedTimeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            Configuration.Timeout = expectedTimeout;
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(intervalMilliseconds);

            var query = new AlwaysThrowsQuery<TestException>();

            var retryUntilTimeoutRobustWrapper = new RetryUntilTimeoutRobustWrapper();

            try
            {
                retryUntilTimeoutRobustWrapper.Query(new TestDriverAction(query));
                Assert.Fail("Expecting test exception");
            }
            catch (TestException) { }

            Assert.That(query.LastCall, Is.InRange(expectedTimeout.TotalMilliseconds - (Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds),
                                                   expectedTimeout.TotalMilliseconds + Configuration.RetryInterval.Milliseconds + When_waiting.AccuracyMilliseconds));
        }


        [Test]
        public void When_an_action_throws_a_not_supported_exception_It_does_not_retry()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(100);
            Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

            var robustness = new RetryUntilTimeoutRobustWrapper();

            var query = new AlwaysThrowsQuery<NotSupportedException>();
            Assert.Throws<NotSupportedException>(() => robustness.Query(new TestDriverAction(query)));
            Assert.That(query.Tries, Is.EqualTo(1));
        }
    }
}