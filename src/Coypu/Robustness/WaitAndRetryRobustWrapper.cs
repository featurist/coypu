using System;
using System.Threading;

namespace Coypu.Robustness
{
	public class WaitAndRetryRobustWrapper : RobustWrapper
	{
		public void Robustly(Action action)
		{
			Robustly<object>(() =>
			         	{
			         		action();
			         		return null;
			         	});
		}

		public TResult Robustly<TResult>(Func<TResult> function)
		{
			return Robustly(function, null);
		}

		public bool WaitFor(Func<bool> query, bool toBecome)
		{
			return Robustly(query, toBecome);
		}

		public TResult Robustly<TResult>(Func<TResult> function, object expectedResult)
		{
			var interval = Configuration.RetryInterval;
			var startTime = DateTime.Now;
			while (true)
			{
				try
				{
					var result = function();
					if (ExpectedResultNotFoundWithinTimeout(expectedResult, result, startTime))
					{
						WaitForInterval(interval);
						continue;
					}
					return result;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					if (TimeoutExceeded(startTime))
					{
						throw;
					}
					WaitForInterval(interval);
				}
			}
		}

		private void WaitForInterval(TimeSpan interval)
		{
			Thread.Sleep(interval);
		}

		private bool ExpectedResultNotFoundWithinTimeout<TResult>(object expectedResult, TResult result, DateTime startTime)
		{
			return expectedResult != null && !result.Equals(expectedResult) && !TimeoutExceeded(startTime);
		}

		private bool TimeoutExceeded(DateTime startTime)
		{
			return DateTime.Now - startTime >= Configuration.Timeout;
		}
	}
}