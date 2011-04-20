using System;
using System.Collections.Generic;

namespace Nappybara.API.UnitTests
{
    public class DeferedRobustness : Robustness
    {
        public IList<Action> DeferredActions = new List<Action>();
        public IList<object> DeferredFunctions = new List<object>();

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
            return default(TResult);
        }
    }
}