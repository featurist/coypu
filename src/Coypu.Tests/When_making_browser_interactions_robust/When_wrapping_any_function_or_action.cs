using System;
using Coypu.Robustness;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
	[TestFixture]
	public class When_wrapping_any_function_or_action
	{
		[Test]
		public void When_a_Function_throws_a_recurring_exception_It_should_retry_until_the_timeout_is_reached_then_rethrow()
		{
			var expectedTimeout = TimeSpan.FromMilliseconds(200);
			Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
			Configuration.Timeout = expectedTimeout;
			var robustness = new WaitAndRetryRobustWrapper();

			Func<object> function = () => { throw new ExplicitlyThrownTestException("Fails every time"); };

			var startTime = DateTime.Now;
			try
			{
				robustness.Robustly(function);
				Assert.Fail("Expected 'Fails every time' exception");
			}
			catch (ExplicitlyThrownTestException e)
			{
				Assert.That(e.Message, Is.EqualTo("Fails every time"));
			}
			var endTime = DateTime.Now;

			var actualDuration = (endTime - startTime);
			var discrepancy = TimeSpan.FromMilliseconds(50);
			Assert.That(actualDuration, Is.InRange(expectedTimeout, expectedTimeout + discrepancy));
		}

		[Test]
		public void When_a_Function_throws_an_exception_first_time_It_should_retry()
		{
			Configuration.Timeout = TimeSpan.FromMilliseconds(100);
			Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
			var robustness = new WaitAndRetryRobustWrapper();
			var tries = 0;
			var expectedReturnValue = new object();
			Func<object> function = () =>
										{
											tries++;
											if (tries == 1)
												throw new Exception("Fails first time");

											return expectedReturnValue;
										};

			var actualReturnValue = robustness.Robustly(function);

			Assert.That(tries, Is.EqualTo(2));
			Assert.That(actualReturnValue, Is.SameAs(expectedReturnValue));
		}

		[Test]
		public void When_an_Action_throws_a_recurring_exception_It_should_retry_until_the_timeout_is_reached_then_rethrow()
		{
			var timeout = TimeSpan.FromMilliseconds(200);
			Configuration.Timeout = timeout;
			Configuration.RetryInterval = TimeSpan.FromMilliseconds(10);
			var robustness = new WaitAndRetryRobustWrapper();
			Action action = () => { throw new ExplicitlyThrownTestException("Fails every time"); };

			var startTime = DateTime.Now;
			try
			{
				robustness.Robustly(action);
				Assert.Fail("Expected 'Fails every time' exception");
			}
			catch (ExplicitlyThrownTestException e)
			{
				Assert.That(e.Message, Is.EqualTo("Fails every time"));
			}
			var endTime = DateTime.Now;

			var actualDuration = (endTime - startTime);
			var discrepancy = TimeSpan.FromMilliseconds(50);
			Assert.That(actualDuration, Is.LessThan(timeout + discrepancy));
		}

		[Test]
		public void When_an_Action_throws_an_exception_first_time_It_should_retry()
		{
			Configuration.Timeout = TimeSpan.FromMilliseconds(100);
			var robustness = new WaitAndRetryRobustWrapper();
			var tries = 0;
			Action action = () =>
								{
									tries++;
									if (tries == 1)
										throw new ExplicitlyThrownTestException("Fails first time");
								};

			robustness.Robustly(action);

			Assert.That(tries, Is.EqualTo(2));
		}

	}
}