using System;

namespace Nappybara.Robustness
{
	public interface RobustWrapper
	{
		TimeSpan Timeout { get; }
		TimeSpan Interval { get; }
		void Robustly(Action action);
		TResult Robustly<TResult>(Func<TResult> function);
	}
}