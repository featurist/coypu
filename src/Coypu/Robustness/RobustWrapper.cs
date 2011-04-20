using System;

namespace Coypu.Robustness
{
	public interface RobustWrapper
	{
		TimeSpan Timeout { get; }
		void Robustly(Action action);
		TResult Robustly<TResult>(Func<TResult> function);
	}
}