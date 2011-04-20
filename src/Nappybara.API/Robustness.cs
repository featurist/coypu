using System;

namespace Nappybara.API
{
	public interface Robustness
	{
		TimeSpan Timeout { get; }
		TimeSpan Interval { get; }
		void Robustly(Action action);
		TResult Robustly<TResult>(Func<TResult> function);
	}
}