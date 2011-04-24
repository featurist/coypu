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
			var interval = Configuration.RetryInterval;
			var startTime = DateTime.Now;
			while (true)
			{
				try
				{
					return function();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					if (TimeoutExceeded(startTime))
					{
						throw;
					}
					Thread.Sleep(interval);
				}
			}
		}

		private bool TimeoutExceeded(DateTime startTime)
		{
			return DateTime.Now - startTime >= Configuration.Timeout;
		}
	}
}