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
			var interval = (int)Math.Round(Configuration.Timeout.TotalMilliseconds / 10);
			var startTime = DateTime.Now;
			while (true)
			{
				try
				{
					return function();
				}
				catch (Exception)
				{
					if (DateTime.Now - startTime >= Configuration.Timeout)
					{
						throw;
					}
					Thread.Sleep(interval);
				}
			}
		}
	}
}