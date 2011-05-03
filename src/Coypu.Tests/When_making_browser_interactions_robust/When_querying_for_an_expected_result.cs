using System;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
	[TestFixture]
	public class When_querying_for_an_expected_result
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
		public void When_the_expected_result_is_found_It_should_return_the_expected_result_immediately()
		{
			When_the_expected_result_is_found_It_should_return_the_expected_result_immediately(true);
			When_the_expected_result_is_found_It_should_return_the_expected_result_immediately(false);
		}

		private void When_the_expected_result_is_found_It_should_return_the_expected_result_immediately(bool expectedResult)
		{
			Func<bool> returnsTrueImmediately = () => expectedResult;

			var actualResult = waitAndRetryRobustWrapper.WaitFor(returnsTrueImmediately, expectedResult);

			Assert.That(actualResult, Is.EqualTo(expectedResult));
		}

		[Test]
		public void When_the_expected_result_is_never_found_It_should_return_the_opposite_result_after_timeout()
		{
			When_the_expected_result_is_never_found_It_should_return_the_opposite_result_after_timeout(true);
			When_the_expected_result_is_never_found_It_should_return_the_opposite_result_after_timeout(false);
		}

		private void When_the_expected_result_is_never_found_It_should_return_the_opposite_result_after_timeout(bool expectedResult)
		{
			var expectedTimeout = TimeSpan.FromMilliseconds(200);
			Configuration.Timeout = expectedTimeout;
			Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

			Func<bool> returnsOppositeImmediately = () => !expectedResult;

			var startTime = DateTime.Now;
			var actualResult = waitAndRetryRobustWrapper.WaitFor(returnsOppositeImmediately, expectedResult);
			var endTime = DateTime.Now;

			Assert.That(actualResult, Is.EqualTo(!expectedResult));

			var actualDuration = (endTime - startTime);
			var discrepancy = TimeSpan.FromMilliseconds(50);
			Assert.That(actualDuration, Is.InRange(expectedTimeout, expectedTimeout + discrepancy));
		}

		[Test]
		public void When_exceptions_are_always_thrown_It_should_rethrow_after_timeout()
		{
			Func<bool> alwaysThrows = () => { throw new ExplicitlyThrownTestException("This query always errors"); };

			Assert.Throws<ExplicitlyThrownTestException>(() => waitAndRetryRobustWrapper.WaitFor(alwaysThrows, true));
		}

		[Test]
		public void When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_expected_result_immediately()
		{
			When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_expected_result_immediately(true);
			When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_expected_result_immediately(false);
		}

		private void When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_expected_result_immediately(bool expectedResult)
		{
			var tries = 0;
			Func<bool> throwsFirstTimeThenReturnsExpectedResult =
				() =>
					{
						tries++;
						if (tries < 3)
						{
							throw new ExplicitlyThrownTestException("This query always errors");
						}
						return expectedResult;
					};

			Assert.That(waitAndRetryRobustWrapper.WaitFor(throwsFirstTimeThenReturnsExpectedResult, expectedResult), Is.EqualTo(expectedResult));
			Assert.That(tries, Is.EqualTo(3));
		}

        [Test]
        public void When_a_not_implemented_exception_is_thrown_It_should_not_retry()
        {
            var tries = 0;
            Func<bool> throwsNotImplemented =
                () =>
                {
                    tries++;
                    throw new NotImplementedException("This query always errors");
                };

            Assert.Throws<NotImplementedException>(() => waitAndRetryRobustWrapper.WaitFor(throwsNotImplemented, true));
            Assert.That(tries, Is.EqualTo(1));
        }

		[Test]
		public void When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_opposite_result_after_timeout()
		{
			When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_opposite_result_after_timeout(true);
			When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_opposite_result_after_timeout(false);
		}

		private void When_exceptions_are_thrown_It_should_retry_And_when_expected_result_found_subsequently_It_should_return_opposite_result_after_timeout(bool expectedResult)
		{
			var expectedTimeout = TimeSpan.FromMilliseconds(250);
			Configuration.Timeout = expectedTimeout;
			Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);

			var oppositeResult = !expectedResult;
			var tries = 0;
			Func<bool> throwsFirstTimeThenReturnOppositeResult =
				() =>
					{
						tries++;
						if (tries < 3)
						{
							throw new ExplicitlyThrownTestException("This query always errors");
						}
						return oppositeResult;
					};

			Assert.That(waitAndRetryRobustWrapper.WaitFor(throwsFirstTimeThenReturnOppositeResult, expectedResult), Is.EqualTo(oppositeResult));
			Assert.That(tries, Is.InRange(4,27));
		}
	}
}