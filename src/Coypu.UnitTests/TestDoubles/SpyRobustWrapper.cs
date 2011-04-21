﻿using System;
using System.Collections.Generic;
using Coypu.Robustness;

namespace Coypu.UnitTests.TestDoubles
{
	public class SpyRobustWrapper : RobustWrapper
	{
		public IList<Action> DeferredActions = new List<Action>();
		public IList<object> DeferredFunctions = new List<object>();
		
		private readonly IDictionary<Type,object> stubbedResults = new Dictionary<Type, object>();

		public TimeSpan Timeout
		{
			get { throw new NotImplementedException(); }
		}

		public TimeSpan Interval
		{
			get { throw new NotImplementedException(); }
		}

		public void Robustly(Action action)
		{
			DeferredActions.Add(action);
		}

		public TResult Robustly<TResult>(Func<TResult> function)
		{
			DeferredFunctions.Add(function);
			return (TResult) stubbedResults[typeof(TResult)];
		}

		public void AlwaysReturn(object result)
		{
			stubbedResults.Add(result.GetType(),result);
		}
	}
}