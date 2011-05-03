using System;

namespace Coypu.Robustness
{
	public interface RobustWrapper
	{
		void Robustly(Action action);
		TResult Robustly<TResult>(Func<TResult> function);
		T Query<T>(Func<T> query, T expecting);

		// TODO: something like this..
		// void TryUntil(Action tryAction, Func<bool> until);
	}
}