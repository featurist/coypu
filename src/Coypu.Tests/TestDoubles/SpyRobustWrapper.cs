using System;
using System.Collections.Generic;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
	public class SpyRobustWrapper : RobustWrapper
	{
		public IList<Action> DeferredActions = new List<Action>();
		public IList<object> DeferredFunctions = new List<object>();
		public IList<Func<bool>> DeferredWaitForQueries = new List<Func<bool>>();
		
		private readonly IDictionary<Type,object> stubbedResults = new Dictionary<Type, object>();
		private readonly IDictionary<bool, bool> stubbedWaitForResult = new Dictionary<bool, bool>();


		public void Robustly(Action action)
		{
			DeferredActions.Add(action);
		}

		public TResult Robustly<TResult>(Func<TResult> function)
		{
			DeferredFunctions.Add(function);
			return (TResult) stubbedResults[typeof(TResult)];
		}

		public bool WaitFor(Func<bool> query, bool toBecome)
		{
			DeferredWaitForQueries.Add(query);
			return stubbedWaitForResult[toBecome];
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