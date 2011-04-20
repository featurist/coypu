using System;
using System.Threading;

namespace Nappybara.API
{
	public class WaitAndRetryRobustness : Robustness
	{
		public TimeSpan Timeout { get; private set; }
		public TimeSpan Interval { get; private set; }

		public WaitAndRetryRobustness(TimeSpan timeout, TimeSpan interval)
		{
			Timeout = timeout;
			Interval = interval;
		}

		public void Robustly(Action action)
		{
			var startTime = DateTime.Now;
			while (true)
			{
				try
				{
					action();
					break;
				}
				catch (Exception)
				{
					Thread.Sleep(Interval);
					if (DateTime.Now - startTime >= Timeout)
					{
						throw;
					}
				}
			}
		}

		public TResult Robustly<TResult>(Func<TResult> function)
		{
			var startTime = DateTime.Now;
			while (true)
			{
				try
				{
					return function();
				}
				catch (Exception)
				{
					Thread.Sleep(Interval);
					if (DateTime.Now - startTime >= Timeout)
					{
						throw;
					}
				}
			}
		}
	}
}