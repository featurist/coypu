using System;
using System.Collections.Generic;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
	public class SpyRobustWrapper : RobustWrapper
	{
		public IList<Action> DeferredActions = new List<Action>();
		public IList<object> DeferredFunctions = new List<object>();
		public IList<object> DeferredQueries = new List<object>();
		
		private readonly IDictionary<Type,object> stubbedResults = new Dictionary<Type, object>();
		private readonly IDictionary<object, object> stubbedWaitForResult = new Dictionary<object, object>();


		public void Robustly(Action action)
		{
			DeferredActions.Add(action);
		}

		public TResult Robustly<TResult>(Func<TResult> function)
		{
			DeferredFunctions.Add(function);
			return (TResult) stubbedResults[typeof(TResult)];
		}

		public T Query<T>(Func<T> query, T expecting)
		{
			DeferredQueries.Add(query);
			return (T) stubbedWaitForResult[expecting];
		}

		public void AlwaysReturnFromRobustly(Type type, object result)
		{
			stubbedResults.Add(type,result);
		}

		public void AlwaysReturnFromWaitFor(bool expected, bool result)
		{
			stubbedWaitForResult[expected] = result;
		}
	}
}