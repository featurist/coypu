using System;
using System.Threading;

namespace Coypu.Robustness
{
	public class WaitAndRetryRobustWrapper : RobustWrapper
	{
		public WaitAndRetryRobustWrapper(TimeSpan timeout)
		{
			Timeout = timeout;
		}

		public TimeSpan Timeout { get; private set; }

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
					if (DateTime.Now - startTime >= Timeout)
					{
						throw;
					}
				}
			}
		}
	}
}