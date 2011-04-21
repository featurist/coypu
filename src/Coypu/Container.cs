using System;
using System.Collections.Generic;
using Coypu.Drivers;
using Coypu.Robustness;

namespace Coypu
{
	public class Container
	{
		public static Dictionary<Type, Func<object>> Concretes
			= new Dictionary<Type, Func<object>>
			  	{
			  		{typeof (Driver), () => new SeleniumWebDriver()},
			  		{typeof (RobustWrapper), () => new WaitAndRetryRobustWrapper()},
			  	};

		public T Get<T>()
		{
			return (T) Concretes[typeof (T)]();
		}
	}
}