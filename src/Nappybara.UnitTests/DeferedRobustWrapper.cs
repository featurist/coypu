using System;
using System.Collections.Generic;
using Nappybara.Robustness;

namespace Nappybara.UnitTests
{
    public class DeferedRobustWrapper : RobustWrapper
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